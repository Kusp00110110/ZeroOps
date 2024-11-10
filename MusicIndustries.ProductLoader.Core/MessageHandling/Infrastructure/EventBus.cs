using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicIndustries.ProductLoader.MessageHandling.Infrastructure
{
    public interface IEventBus
    {
        Task Subscribe<T>(Action<Task<T>> handler);
        Task Publish<T>(Task<T> @event);
    }

    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new();

        public async Task Subscribe<T>(Action<Task<T>> handler)
        {
            if (!_handlers.ContainsKey(typeof(T)))
                _handlers[typeof(T)] = new List<Delegate>();

            _handlers[typeof(T)].Add(handler);
        }

        public async Task Publish<T>(Task<T> @event)
        {
            if (_handlers.ContainsKey(typeof(T)))
            {
                foreach (var handler in _handlers[typeof(T)].Cast<Action<Task<T>>>())
                {
                    handler(@event);
                }
            }
        }
    }
}
