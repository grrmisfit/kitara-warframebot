using Unity;
using Unity.Resolution;
using Warframebot.Storage;

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
            
            _container.RegisterSingleton<ILogger, Logger>();
            
        }
        
        public static T Resolve<T>()
        {
            return (T) Container.Resolve(typeof(T), string.Empty, new CompositeResolverOverride());
        }
    }
}