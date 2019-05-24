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

        public override void OnUpdate(Fsm<ProcedureManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //在正在流程中当流程变为end则变换流程为结束流程
            if (GameManagerParameter.Singleton.IsTargetProcedure(GameProcedure.End))
            {
                //进入结束流程
                ChangeState<OnGameEnd>(fsm);
            }
        }
    }
}