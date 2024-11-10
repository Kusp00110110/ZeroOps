using System.Threading.Tasks;

namespace MusicIndustries.ProductLoader.Notifications
{
    public interface IProcessNotifications
    {
        Task SendNotificationAsync(
            string message,
            string title = null,
            string url = null,
            string urlTitle = null,
            string sound = null,
            int? priority = null,
            string attachmentPath = null);
    }
}
