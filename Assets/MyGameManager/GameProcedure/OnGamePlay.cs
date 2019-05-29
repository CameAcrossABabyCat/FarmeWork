using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{
    /// <summary>
    /// 正在玩的流程
    /// </summary>
    public class OnGamePlay : ProcedureBase
    {
        /// <summary>
        /// 当进入进行流程
        /// </summary>
        /// <param name="fsm"></param>
        public override void OnEnter(Fsm<ProcedureManager> fsm)
        {
            base.OnEnter(fsm);
        }

        public override void OnUpdate(Fsm<ProcedureManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //在正在流程中当流程变为end则变换流程为结束流程
            if (ParameterManager.Singleton.IsTargetProcedure(GameProcedure.End))
            {
                //进入结束流程
                ChangeState<OnGameEnd>(fsm);
            }
        }
    }
}