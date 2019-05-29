using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace MyGameManager
{

    public class ReadJson : MonoBehaviour
    {

        public static ReadJson Instance;

        void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        [HideInInspector]
        public string ErrorStr = "";

        /// <summary>
        /// 给服务器发送消息
        /// </summary>
        /// <param name="url"></param>
        public void SendData(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.Log("传入发送的url为空!");
                return;
            }
            StartCoroutine(GetJosnData(url));
        }

        private IEnumerator GetJosnData(string url)
        {
            //退出默认流程进入加载流程

            GameData.Singleton.GameJsonData = null;
            ErrorStr = "";
            UnityWebRequest web = UnityWebRequest.Get(url);

            yield return web.Send();
            if (web.isError)
            {
                //发送失败
                ErrorStr = web.error;
            }
            else
            {
                if (web.responseCode == 200)
                {
                    var json = web.downloadHandler.text;

                    //json = json.Remove(0, 0);移除字符,但不知道什么用,先隐藏
                    GameStatic.Singleton.GetJsonData(json);
                }
                else
                    ErrorStr = "error code:" + web.responseCode.ToString();
            }

            //退出加载流程进入默认流程
        }
    }
}