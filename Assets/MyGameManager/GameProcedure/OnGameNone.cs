using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{
    /// <summary>
    /// 开始流程
    /// </summary>
    public class OnGameNone : ProcedureBase
    {
        /// <summary>
        /// 当进入默认流程
        /// </summary>
        /// <param name="fsm"></param>
        public override void OnEnter(Fsm<ProcedureManager> fsm)
        {
            base.OnEnter(fsm);
        }

        public override void OnUpdate(Fsm<ProcedureManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //在默认流程中当流程变为load则变换流程为加载流程
            if (ParameterManager.Singleton.IsTargetProcedure(GameProcedure.Loading))
            {
                //进入加载流程
                ChangeState<OnGameLoading>(fsm);
            }
        }
    }
}