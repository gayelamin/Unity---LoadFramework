using Framework;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using LoadFramework;

namespace Game
{
    // 需要用到Load里面的ILoadModule
    public abstract class AbstractMonoModule<T> : MonoSingleton<T>, IModuleLoadHandler where T : MonoSingleton<T>, IModuleLoadHandler
    {
        public virtual void Awake()
        {
            this.RegisterLoadEvent();
        }

        protected override void OnDestroy()
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
    }
}