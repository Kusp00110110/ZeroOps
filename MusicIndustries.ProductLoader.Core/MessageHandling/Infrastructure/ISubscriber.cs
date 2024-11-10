using System.Threading.Tasks;

namespace MusicIndustries.ProductLoader.MessageHandling.Infrastructure
{
    public interface ISubscriber<in TMessage>
    {

        Task HandleAsync(TMessage message);
    }
}
