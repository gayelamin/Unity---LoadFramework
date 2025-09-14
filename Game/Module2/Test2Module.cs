using LoadFramework;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Module2
{

    public class Test2Module : AbstractGameMonoModule
    {
        public List<GameObject> gameObjects = new List<GameObject>();

        public AbstractLoader loader;
        public override string moduleName => "Test2";

        protected override void OnCreateLoader(ILoadInfo info)
        {
            if(loader != null)
            {
                Destroy(loader);
            }
            Test2LoadInfo loadInfo = info as Test2LoadInfo;
            loader = gameObject.AddComponent<Test2Loader>();
            Test2Loader test2Loader = loader as Test2Loader;
            test2Loader.Init(loadInfo, this);
        }

    }
}