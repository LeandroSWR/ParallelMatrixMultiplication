using System;
using System.Text.Json;
using System.IO;

namespace TCP_T1_LeandroBras
{
    public class Menu
    {
        private MatrixMultiplier matrixMultiplier;
        private StatsDisplay statsDisplay;

        private string[] titleSprite =
        {
            @" __  __       _        _        __  __       _ _   _       _ _",
            @"|  \/  |     | |      (_)      |  \/  |     | | | (_)     | (_)",
            @"| \  / | __ _| |_ _ __ ___  __ | \  / |_   _| | |_ _ _ __ | |_  ___ _ __",
            @"| |\/| |/ _` | __| '__| \ \/ / | |\/| | | | | | __| | '_ \| | |/ _ \ '__|",
            @"| |  | | (_| | |_| |  | |>  <  | |  | | |_| | | |_| | |_) | | |  __/ |",
            @"|_|  |_|\__,_|\__|_|  |_/_/\_\ |_|  |_|\__,_|_|\__|_| .__/|_|_|\___|_|",
            @"                                                    | |",
            @"                                                    |_|"
        };

        private string[] menuBox =
        {
            "\t\t  |-----------------------------|",
            "\t\t  |\t\t\t\t|",
            "\t\t  |      1 - Test All           |",
            "\t\t  |\t\t\t\t|",
            "\t\t  |      2 - Parallel Only      |",
            "\t\t  |\t\t\t\t|",
            "\t\t  |      3 - Linear Only        |",
            "\t\t  |\t\t\t\t|",
            "\t\t  |      4 - Check Stats        |",
            "\t\t  |\t\t\t\t|",
            "\t\t  |      5 - Exit               |",
            "\t\t  |-----------------------------|",
        };

        public Menu() 
        {
            matrixMultiplier = new MatrixMultiplier();
            statsDisplay = new StatsDisplay(matrixMultiplier.MStats);
        }


        public void DrawMenu()
        {
            // Make sure console is empty before drawing the menu
            Console.Clear();

            for (int i = 0; i < titleSprite.Length; i++)
            {
                Console.WriteLine(titleSprite[i]);
            }
            for (int i = 0; i < menuBox.Length; i++)
            {
                Console.WriteLine(menuBox[i]);
            }

            int choice;
            char c;
            // Lock until the user chooses a valid option
            do
            {
                c = Console.ReadKey().KeyChar;
            } while (
                !int.TryParse(c.ToString(), out choice) &&
                choice < 1 && 
                choice > 5);

            CheckChoice(choice);
        }

        private void CheckChoice(int selection)
        {
            Console.Clear();

            switch (selection)
            {
                default:
                case 1:
                    matrixMultiplier.DoAllMultiplications();
                    break;
                case 2:
                    matrixMultiplier.DoTaskMultiplications();
                    break;
                case 3:
                    matrixMultiplier.DoLinearMultiplications();
                    break;
                case 4:
                    statsDisplay.DisplayStats();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }

            Console.WriteLine("\nPress Any Key to go back to the Menu...");
            Console.ReadKey();
            DrawMenu();
        }
    }
}