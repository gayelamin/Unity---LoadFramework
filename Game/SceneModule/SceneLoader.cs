using LoadFramework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Game.SceneModule { 
    public class SceneLoader : AbstractLoader
    {
        private string sceneName;
        private AsyncOperation asyncOperation;

        public void Init(SceneLoadInfo info)
        {
            this.sceneName = info.sceneName;
            this.LoadRoundIndex = info.LoadRoundIndex;
            SendingLoader();
        }

        public override IEnumerator LoadBatch1()
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
    }
}