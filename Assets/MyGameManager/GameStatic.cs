using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MyGameManager
{

    public class GameStatic : SingletonPlant<GameStatic>
    {
        #region 获取Json格式数据
        public void GetJsonData(string json)
        {
            DeleteJson();
            CreateJson(json);
            var arrList = LoadJson();
            var str = "";
            foreach (string arr in arrList)
            {
                str += arr;
            }

            GameData.Singleton.GameJsonData = JsonMapper.ToObject(str);
        }

        /// <summary>
        /// 删除已有的Json文件
        /// </summary>
        private void DeleteJson()
        {
            File.Delete(Application.persistentDataPath + "//gamejson.tex");
        }

        /// <summary>
        /// 创建一个新的Json文件
        /// </summary>
        private void CreateJson(string json)
        {
            StreamWriter sw;
            FileInfo t = new FileInfo(Application.persistentDataPath + "//gamejson.tex");

            if (!t.Exists)
            {
                sw = t.CreateText();
            }
            else
            {
                sw = t.AppendText();
            }
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
        }

        /// <summary>
        /// 读取Json文件
        /// </summary>
        /// <returns></returns>
        private ArrayList LoadJson()
        {
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(Application.persistentDataPath + "//gamejson.tex");
            }
            catch
            {
                return null;
            }
            string line;
            var arrList = new ArrayList();
            while ((line = sr.ReadLine()) != null)
            {
                arrList.Add(line);
            }
            sr.Close();
            sr.Dispose();
            return arrList;
        }
        #endregion

        public void GetSendUrl(string action)
        {
            var str = "";
            str += GameParameter.Singleton.Url + action;
        }

        public void GetSendUrl(string action, string node, string name)
        {
            var str = "";
            str += GameParameter.Singleton.Url + action;
            str += "&" + name + "=" + GameData.Singleton.DataManager.GetData<string>(name);
        }


        public void GetSendUrl(string action, string node, string[] names)
        {

        }
    }
}