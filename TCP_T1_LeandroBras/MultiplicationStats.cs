using Newtonsoft.Json;

namespace TCP_T1_LeandroBras
{
    public class MultiplicationStats
    {
        [JsonProperty("ETime")]
        public double ETime { get; private set; }
        [JsonProperty("MSize")]
        public string MSize { get; private set; }
        [JsonProperty("MType")]
        public string MType { get; private set; }
        [JsonProperty("IsTaskBased")]
        public bool IsTaskBased { get; private set; }

        public MultiplicationStats(double eTime, string mSize, MultiplicationType mType, bool taskBased)
        {
            ETime = eTime;
            MSize = mSize;
            MType = mType.ToString();
            IsTaskBased = taskBased;
        }

        public override string ToString()
        {
            return $"Size: {MSize}, Time: {ETime.ToString("0.00")} ms, Type: {MType}, IsTaskBased: {IsTaskBased}";
        }
    }
}