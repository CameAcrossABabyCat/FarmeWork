using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameManager
{

    public class GameManagerParameter
    {

        public GameManagerParameter()
        {

        }

        private static GameManagerParameter _singleton;

        public static GameManagerParameter Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    return new GameManagerParameter();
                }
                return _singleton;
            }
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


        public bool IsTargetProcedure(GameProcedure target)
        {
            return _procedure == target;
        }
    }
}