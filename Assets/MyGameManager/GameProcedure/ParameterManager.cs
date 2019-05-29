using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{

    public class ParameterManager : SingletonPlant<ParameterManager>
    {

        public ParameterManager()
        {

        }

        /// <summary>
        /// 当前流程
        /// </summary>
        private GameProcedure _procedure = GameProcedure.None;

        /// <summary>
        /// 从等待进入开始流程
        /// </summary>
        public void InsetSatrt()
        {
            _procedure = GameProcedure.Start;
        }

        /// <summary>
        /// 从开始进入进行流程
        /// </summary>
        public void InsetPlay()
        {
            _procedure = GameProcedure.Playing;
        }

        /// <summary>
        /// 从进行流程进行到结束流程
        /// </summary>
        public void InsetEnd()
        {
            _procedure = GameProcedure.End;
        }

        /// <summary>
        /// 从结束流程进行到等待流程
        /// </summary>
        public void InsetWait()
        {
            _procedure = GameProcedure.Waiting;
        }

        /// <summary>
        /// 进入默认流程
        /// </summary>
        public void InsetNone()
        {
            _procedure = GameProcedure.None;
        }

        /// <summary>
        /// 进入加载流程
        /// </summary>
        public void InsetLoading()
        {
            _procedure = GameProcedure.Loading;
        }

        /// <summary>
        /// 判断当前流程是否为目标流程
        /// </summary>
        /// <param name="target">目标流程</param>
        /// <returns></returns>
        public bool IsTargetProcedure(GameProcedure target)
        {
            return _procedure == target;
        }
    }
}