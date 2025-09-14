using Game.Module2;
using LoadFramework;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Module3
{
    public class Test3Module : AbstractGameMonoModule
    {
        [InjectModule]
        public Test2Module test2Module;

        public List<GameObject> gameObjects = new List<GameObject>();

        public AbstractLoader loader;
        public override string moduleName => "Test3";
        protected override void OnCreateLoader(ILoadInfo info)
        {
            if(loader != null)
            {
                Destroy(loader);
            }
            if (info.LoaderType == typeof(Test3Unloader))
            {
                Test3UnloadInfo unloadInfo = info as Test3UnloadInfo;
                loader = gameObject.AddComponent<Test3Unloader>();
                Test3Unloader test3unloader = loader as Test3Unloader;
                test3unloader.Init(unloadInfo, this);
            }
            else if (info.LoaderType == typeof(Test3Loader))
            {
                Test3LoadInfo loadInfo = info as Test3LoadInfo;
                loader = gameObject.AddComponent<Test3Loader>();
                Test3Loader test3loader = loader as Test3Loader;
                test3loader.Init(loadInfo, this);
            }
        }
    }
}