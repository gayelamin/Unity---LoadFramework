
namespace LoadFramework
{
    public interface IModuleLoadHandler { 
        void CreateLoader(ILoadInfo info);
        void ReceiptLoadInfo(ILoadEventInfo infos);
    }

    //拓展方法，实现接口的注册和注销方法
    public static class ModuleLoadHandlerExtensions
    {
        public static void RegisterLoadEvent(this IModuleLoadHandler moduleLoadHandler)
        {
            LoadingEvent.Register(moduleLoadHandler.ReceiptLoadInfo);
        }
        public static void UnregisterLoadEvent(this IModuleLoadHandler moduleLoadHandler)
        {
            LoadingEvent.UnRegister(moduleLoadHandler.ReceiptLoadInfo);
        }
    }
}