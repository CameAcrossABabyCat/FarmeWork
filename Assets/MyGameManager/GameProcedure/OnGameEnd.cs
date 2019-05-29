using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{
    /// <summary>
    /// 结束流程
    /// </summary>
    public class OnGameEnd : ProcedureBase
    {
        /// <summary>
        /// 当进入结束流程
        /// </summary>
        /// <param name="fsm"></param>
        public override void OnEnter(Fsm<ProcedureManager> fsm)
        {
            base.OnEnter(fsm);
        }

        public override void OnUpdate(Fsm<ProcedureManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //在结束流程中当流程变为wait则变换流程为等待流程
            if (ParameterManager.Singleton.IsTargetProcedure(GameProcedure.Waiting))
            {
                //进入等待流程
                ChangeState<OnGameWait>(fsm);
            }
        }
    }
}