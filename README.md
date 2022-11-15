# Parallel Matrix Multiplication

Simple program for a task based parallel multiplication of a 2000x2000 matrix

## Author

- Leandro Brás a22100770

## Architecture

We have 3 different types of multiplication for this program, `Double Index`,
`Linear Index` and `Transposed Linear Index`. Taking this into account I
created 3 public methods that can be called to perform the matrix calculations,
these methods all function in a similar way.

First we create a new `struct` that contains the index limits of our matrix so
we can later use these indexes to go trough the "partitioned matrix", this is a
very simple struct with only the elements necessary to make it easy to send this
data do the multiplication task.

```csharp
struct MatLimit
{
    public int fLine, lLine;
    public int fCol, lCol;

    public MatLimit(int fCol, int lCol, int fLine, int lLine)
    {
        this.fLine = fLine;
        this.lLine = lLine;
        this.fCol = fCol;
        this.lCol = lCol;
    }
}
```

Using this struct we can create 4 limits that will represent 4 chunks of the final
matrix.

```csharp
MatLimit matLim1 = new MatLimit(0, x / 2, 0, y / 2);
MatLimit matLim2 = new MatLimit(0, x / 2, y / 2, y);
MatLimit matLim3 = new MatLimit(x / 2, x, 0, y / 2);
MatLimit matLim4 = new MatLimit(x / 2, x, y / 2, y);

tasks[0] = Task.Factory.StartNew(() => MultiplyDoubleIndex(matLim1, matA, matB));
tasks[1] = Task.Factory.StartNew(() => MultiplyDoubleIndex(matLim2, matA, matB));
tasks[2] = Task.Factory.StartNew(() => MultiplyDoubleIndex(matLim3, matA, matB));
tasks[3] = Task.Factory.StartNew(() => MultiplyDoubleIndex(matLim4, matA, matB));
```

Just looking at it it's hard to spot what's happening, what we're doing here is
vertically dividing the A Matrix and Horizontally dividing the B matrix, then we
can create the 4 parallel tasks tha will do the multiplication like this:

- Left of A * Top of B
- Left of A * Bottom of B
- Right of A * Top of B
- Right of A * Bottom of B

Notice how the matrix is never really partitioned and we only work with indexes,
this makes it easier to compose the final matrix from the parallel multiplications.

The multiplications it self is really then just a normal matrix multiplication using
our limits for the `double for loop`.

```csharp
private void MultiplyDoubleIndex(MatLimit data, float[,] matA, float[,] matB)
{
    Parallel.For(data.fLine, data.lLine, i =>
    {
        for (int j = data.fCol; j < data.lCol; j++)
        {
            float temp = 0;
            for (int k = 0; k < x; k++)
            {
                temp += matA[i, k] * matB[k, j];
            }
            retMdMat[i, j] = temp;
        }
    });
}
```

We grab our limits from the data variable and use it to determine the value at
which our loops should start and should end. We then save each multiplication
to the correct location on our return matrix.

For even faster performance I chose to use the `Parallel.For()` method to
further parallelize the multiplication.

## Results

The tests where done on my own computer with the following specs:

- Ryzen 9 5900X (12core 24thread)
- 32Gb (4x8Gb) 3600Mhz Cl16

In order to get the most accurate results the tests ran 4 times each (Linear
and Parallel), and the results where averaged:

### Average Time for Linear Multiplications

| Size      | Double Index | Linear Index | Transposed |
| :-------: | :----------: | :----------: | :--------: |
| 2000x2000 |  46323.08ms  |  21315.44ms  | 20444.37ms |

### Average Time for Parallel Multiplications

| Size      | Double Index | Linear Index | Transposed |
| :-------: | :----------: | :----------: | :--------: |
| 2000x2000 |  4013.54ms   |  2281.87ms   | 2120.13ms  |

From these results we can see there's a massive increase in performance,
being `11.54` faster on the `Double Index`, `9.34` faster on the
`Linear Index` and `9.64` times faster in the `Transposed` multiplications,
for a total average of `10.47` times faster when doing the multiplications
in parallel *(Note that these results will be different depending on the CPU
and the total number of cores)*.

We can also conclude that even the way the multiplications are done can
have a good impact on performance with `Linear Index` being `2.17` times
faster than `Double Index` and `Transposed` being `1.04` times faster than
`Linear Index`.

## Future Works

For the future I think I'll need to reconsider how the time was measured,
this is because it's already know that multiplications with a transposed
matrix are much faster than just a Linearized Index Matrix, but my results
here only show a 4% increase in speed, i believe this is caused by the way
the time is measured in my code where I'm also measuring the transposing of
the matrix it self and not only the multiplication.

## Thanks

I'd like to thank Professor José Rogado for all the help during the creation
of this program and for making it clear that I should've been using Index
partition instead of actually partitioning the matrix it self from the start!
