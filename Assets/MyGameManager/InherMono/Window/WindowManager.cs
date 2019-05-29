using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework;
using UnityEngine;

namespace MyGameManager
{

    public abstract class WindowManager
    {
        public string ActionName;

        public Transform ParentTransform;

        public TabItemView Item;
        //public string NodeName;

        private DataNode _node;

        public DataNode Node
        {
            get { return _node; }
        }

        public int SerialId { get; protected set; }

        public bool Done { get; protected set; }

        /// <summary>
        /// 先发送请求,之后解析接收到的数据,显示窗口,最后设置数据
        /// </summary>
        public virtual void OpenWindow()
        {
            var url = GetSendUrl();
            //ReadJson.Instance.SendData(url);
        }


        public virtual void CreateNode()
        {

        }


        protected virtual string GetSendUrl()
        {
            string str = "";
            if (string.IsNullOrEmpty(ActionName))
                return str;
            return str;
        }

        public void StartTask()
        {
            throw new System.NotImplementedException();
        }
    }
}