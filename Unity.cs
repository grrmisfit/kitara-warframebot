using Unity;
using Unity.Resolution;
using Warframebot.Storage;
using Warframebot.Storage.Implementation;

namespace Warframebot
{
    public class Unity
    {
        private static UnityContainer _container;

        public static UnityContainer Container
        {
            get
            {
                if(_container == null)
                    RegisterTypes();
                return _container;
            }
        }


        public static void RegisterTypes()
        {
            _container = new UnityContainer();
            _container.RegisterSingleton<IDataStorage, JsonStorage>();
            _container.RegisterSingleton<ILogger, Logger>();
            //_container.RegisterSingleton<Discord.Connection>();
           // _container.RegisterType<DiscordSocketConfig>(new InjectionFactory(i => SocketConfig.GetDefault()));
           // _container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
          //  _container.RegisterSingleton<Discord.Entities.Connection>();
        }
        
        public static T Resolve<T>()
        {
            return (T) Container.Resolve(typeof(T), string.Empty, new CompositeResolverOverride());
        }
    }
}