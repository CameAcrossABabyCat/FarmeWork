﻿using SimpleGameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{
    /// <summary>
    /// 等待流程
    /// </summary>
    public class OnGameWait : ProcedureBase
    {
        /// <summary>
        /// 当进入等待流程
        /// </summary>
        /// <param name="fsm"></param>
        public override void OnEnter(Fsm<ProcedureManager> fsm)
        {
            base.OnEnter(fsm);
        }

        public override void OnUpdate(Fsm<ProcedureManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //在等待流程中当流程变为start则变换流程为开始流程
            if (ParameterManager.Singleton.IsTargetProcedure(GameProcedure.Start))
            {
                //进入开始流程
                ChangeState<OnGameStart>(fsm);
            }
        }
    }
}