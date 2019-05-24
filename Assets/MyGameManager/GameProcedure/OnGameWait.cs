using SimpleGameFramework;
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
        public override void OnUpdate(Fsm<ProcedureManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //在等待流程中当流程变为start则变换流程为开始流程
            if (GameManagerParameter.Singleton.IsTargetProcedure(GameProcedure.Start))
            {
                //进入正在进行流程
                ChangeState<OnGameStart>(fsm);
            }
        }
    }
}