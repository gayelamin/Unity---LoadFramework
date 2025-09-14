using System;
using System.Collections;
using UnityEngine;

namespace LoadFramework
{
    /*
     这里的加载器除了获取type，删除，发送，这三个函数之外，其他函数都是负责加载的，继承的加载器中，你需要把加载代码写在对应的函数里面。
     这里的批次逻辑是这样：
        首先加载器会分轮次，每一轮次加载器执行完毕之后才会执行下一轮次的加载器。
        而这里是一个加载器内部，当前批次的加载器进行加载时，会按照这里的顺序进行加载。
        但是要注意！！！
            一个轮次的加载器，会首先执行所有加载器的LoadBatch1，然后才会执行所有加载器的LoadBatch2，以此类推。
     */
    /*
     *此外加载器主要使用monobehaviour，是为了统一格式，首先是加载场景需使用协程，而这需要使用monobehavior。此外一些比如说预制体的实例化也需要使用monobehaviour。
     *所以说加载管理器中 全部统一使用协程，而Loader这里也推荐全部使用monobehaviour。
     */
    public interface ILoader
    {
        int LoadRoundIndex { get; set; }//注意加载轮次是从0开始的，依次增大。
        /// <summary>
        /// 获得加载器类型
        /// </summary>
        /// <returns></returns>
        Type GetLoaderType();//注意的是，这里使用GetType是直接获取的继承的加载器的类名，比如这个加载器叫做PlayerLoader，那么GetType返回的就是"PlayerLoader"，在loadinfo中定义的type也要和这个保持一致

        /// <summary>
        /// 第一批次中调用（因为场景最好是先加载，所以这里可以用于场景加载）
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadBatch1();
        /// <summary>
        /// 第二批次中调用（可以用来读取资源（如图片、音频、配置等））
        /// </summary>
        IEnumerator LoadBatch2();
        /// <summary>
        /// 第三批次中调用（可以用来读取预制体）
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadBatch3();
        /// <summary>
        /// 第四批次调用（可以实例化预制体）
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadBatch4();
        /// <summary>
        /// 第五个批次中调用（可以用来场景载入后初始化）
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadBatch5();
        /// <summary>
        /// 第六个批次中调用
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadBatch6();
        /// <summary>
        /// 删除这个加载器
        /// </summary>
        void DestroyItself();
        /// <summary>
        /// 将这个Loader发送到加载器上
        /// </summary>
        void SendingLoader();
    }

    public abstract class AbstractLoader : MonoBehaviour, ILoader
    {
        protected int _LoadRoundIndex = 1;

        public int LoadRoundIndex
        {
            get => _LoadRoundIndex;
            set => _LoadRoundIndex = value;
        }

        public virtual Type GetLoaderType()
        {
            // 默认返回类名，便于识别和调试
            return this.GetType();
        }
        public virtual IEnumerator LoadBatch1()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch2()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch3()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch4()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch5()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch6()
        {
            yield break;
        }
        public virtual void DestroyItself()
        {
            Destroy(this);
        }
        /// <summary>
        /// 注意初始化后需要调用这个函数将自己发送到LoadManager
        /// </summary>
        public virtual void SendingLoader()
        {
            new SendLoaderCommand(this).Execute();
        }
    }

    // 这里提供了一个非Mono加载器，但是我的项目一只没有使用过，我也不知道它能不能用，暂时是保留在这里，如果你需要，可以尝试使用它
    
    public abstract class AbstractNonMonoLoader : ILoader
    {
        protected int _LoadRoundIndex = 1;

        public int LoadRoundIndex
        {
            get => _LoadRoundIndex;
            set => _LoadRoundIndex = value;
        }

        public virtual Type GetLoaderType()
        {
            // 默认返回类名，便于识别和调试
            return this.GetType();
        }
        public virtual IEnumerator LoadBatch1()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch2()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch3()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch4()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch5()
        {
            yield break;
        }
        public virtual IEnumerator LoadBatch6()
        {
            yield break;
        }
        public virtual void SendingLoader()
        {
            new SendLoaderCommand(this).Execute();
        }

        public virtual void DestroyItself()
        {
            return;
        }
    }
}