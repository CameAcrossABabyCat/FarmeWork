using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{
    /// <summary>
    /// 开始流程
    /// </summary>
    public class OnGameStart : ProcedureBase
    {
        /// <summary>
        /// 当进入开始流程
        /// </summary>
        /// <param name="fsm"></param>
        public override void OnEnter(Fsm<ProcedureManager> fsm)
        {
            base.OnEnter(fsm);
        }

        public override void OnUpdate(Fsm<ProcedureManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //在开始流程中当流程变为play则变换流程为进行流程
            if (ParameterManager.Singleton.IsTargetProcedure(GameProcedure.Playing))
            {
                //进入正在进行流程
                ChangeState<OnGamePlay>(fsm);
            }
        }
    }
}