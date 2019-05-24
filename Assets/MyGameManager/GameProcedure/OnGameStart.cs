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
        public override void OnUpdate(Fsm<ProcedureManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //在开始流程中当流程变为play则变换流程为进行流程
            if (GameManagerParameter.Singleton.IsTargetProcedure(GameProcedure.Playing))
            {
                //进入等待流程
                ChangeState<OnGamePlay>(fsm);
            }
        }
    }
}