namespace MusicIndustries.ProductLoader.MessageHandling.Messages
{
    public class SendProgressNotification
    {
        public string? Message { get; set; }
        public int ProgressPercentage { get; set; }
    }
}
