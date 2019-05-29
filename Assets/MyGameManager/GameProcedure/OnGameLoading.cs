using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{
    /// <summary>
    /// 开始流程
    /// </summary>
    public class OnGameLoading : ProcedureBase
    {
        /// <summary>
        /// 当进入加载流程
        /// </summary>
        /// <param name="fsm"></param>
        public override void OnEnter(Fsm<ProcedureManager> fsm)
        {
            base.OnEnter(fsm);
        }

        public override void OnUpdate(Fsm<ProcedureManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //在加载流程中当流程变为none则变换流程为默认流程
            if (ParameterManager.Singleton.IsTargetProcedure(GameProcedure.None))
            {
                //进入默认流程
                ChangeState<OnGameNone>(fsm);
            }
        }
    }
}