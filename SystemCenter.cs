using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SystemCenter : Architecture<SystemCenter>
    {
        protected override void Init()
        {
            RegisterSystem<Game.LogicModule.TestSystem>(new Game.LogicModule.TestSystem());
            //RegisterSystem<ILoadSystem>(new LoadSystem());
            //RegisterUtility<IMapInfoLoader>(new MapInfoLoader());
            //RegisterModel<IMapModel>(new MapModel());
            //RegisterSystem<IMapSystem>(new MapSystem());
        }
    }
}
