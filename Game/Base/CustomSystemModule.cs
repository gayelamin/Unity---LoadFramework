using Framework;
using LoadFramework;

namespace Game {
    public abstract class AbstractCustomSystem : AbstractSystem, IModuleLoadHandler {
        protected override void OnInit()
        {
            this.RegisterLoadEvent();
        }

        protected virtual void OnDestroy()
        {
            this.UnregisterLoadEvent();
        }

        void IModuleLoadHandler.CreateLoader(ILoadInfo info)
        {
            OnCreateLoader(info);
        }

        void IModuleLoadHandler.ReceiptLoadInfo(ILoadEventInfo infos)
        {
            OnReceiptLoadInfo(infos);
        }

        protected virtual void OnCreateLoader(ILoadInfo info)
        {
            return;
        }
        public virtual string moduleName { get; } = "";
        protected virtual void OnReceiptLoadInfo(ILoadEventInfo infos)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                return;
            }
            ILoadInfo info = infos.GetInfo(moduleName);
            if (info != null)
            {
                OnCreateLoader(info);
            }
        }

        public abstract void DestroyLoader();
    }
}