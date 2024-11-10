using System.Threading.Tasks;

namespace MusicIndustries.ProductLoader.MessageHandling.Infrastructure
{
    public interface IPublisher<in TMessage>
    {
        Task PublishAsync(TMessage message);
    }
}
