using System;
using DefaultNamespace.Core.Model;

namespace CoreAssembly
{
    public class GameArchitecture : Architecture<GameArchitecture>
    {
        protected override void Init()
        {
            // this.RegisterSystem<IBattleSystem>(new BattleSystem());
            //
            this.RegisterModel<IInputModel>(new InputModel());
        }
    }

}
