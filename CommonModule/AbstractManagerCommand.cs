using Framework;

namespace GameFramework {
    //这里的命令的主要功能是，自动获取继承了 MonoSingleton<T> 的单例类的实例，因为这里规定 MonoSingleton 是一定要被大模块才能继承的，当然你可以根据需求修改
    public interface IManagerCommand 
	{
		/// <summary>
		/// 执行命令
		/// </summary>
		void Execute();
	}

    public abstract class AbstractManagerCommand<TManager> : IManagerCommand where TManager : MonoSingleton<TManager>
    {

        protected TManager Manager => GetManager();

        /// <summary>
        /// 执行命令
        /// </summary>
        protected virtual TManager GetManager()
        {
            return MonoSingleton<TManager>.Instance;
        }

        public abstract void Execute();
    }
}