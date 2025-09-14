using LoadFramework;
using System.Collections.Generic;
using UnityEngine;


namespace Game.SceneModule
{
    public class SceneModule : AbstractGameMonoModule
    {
        public SceneLoader sceneLoader;
        public override string moduleName => "Scene";
        protected override void OnCreateLoader(ILoadInfo info)
        {
            if (sceneLoader != null)
            {
                Destroy(sceneLoader);
            }
            SceneLoadInfo loadInfo = info as SceneLoadInfo;
            sceneLoader = gameObject.AddComponent<SceneLoader>();
            sceneLoader.Init(loadInfo);
        }
    }
}
