using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEditor;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEditor.VersionControl;

namespace CommonFrame
{
    class Styles
    {
        //public GUIContent invalidColorSpaceMessage = new GUIContent("In order to build a player go to 'Player Settings...' to resolve the incompatibility between the Color Space and the current settings.", EditorGUIUtility.GetHelpIcon(MessageType.Warning));
        //public GUIContent invalidLightmapEncodingMessage = new GUIContent("In order to build a player go to 'Player Settings...' to resolve the incompatibility between the selected Lightmap Encoding and the current settings.", EditorGUIUtility.GetHelpIcon(MessageType.Warning));
        public GUIStyle selected = "OL SelectedRow";
        public GUIStyle box = "OL Box";
        public GUIStyle title = EditorStyles.boldLabel;
        public GUIStyle evenRow = "CN EntryBackEven";
        public GUIStyle oddRow = "CN EntryBackOdd";
        public GUIStyle platformSelector = "PlayerSettingsPlatform";
        public GUIStyle toggle = "Toggle";
        public GUIStyle levelString = "PlayerSettingsLevel";
        public GUIStyle levelStringCounter = "RightAlignedLabel";
        public Vector2 toggleSize;

        //public GUIContent becauseYouAreNot = new GUIContent("Because you are not a member of this project this build will not access Unity services.", EditorGUIUtility.GetHelpIcon(MessageType.Warning));
        public GUIContent noSessionDialogText = new GUIContent("In order to publish your build to UDN, you need to sign in via the AssetStore and tick the 'Stay signed in' checkbox.");
        public GUIContent platformTitle = new GUIContent("Platform", "Which platform to build for");
        public GUIContent switchPlatform = new GUIContent("Switch Platform");
        public GUIContent build = new GUIContent("Build");
        public GUIContent buildAndRun = new GUIContent("Build And Run");
        public GUIContent scenesInBuild = new GUIContent("Scenes In Build", "Which scenes to include in the build");
        public GUIContent checkOut = new GUIContent("Check out");
        public GUIContent addOpenSource = new GUIContent("Add Open Scenes");
        // public string noModuleLoaded = L10n.Tr("No {0} module loaded.");
        public GUIContent openDownloadPage = new GUIContent("Open Download Page");
        //public string infoText = L10n.Tr("{0} is not included in your Unity Pro license. Your {0} build will include a Unity Personal Edition splash screen.\n\nYou must be eligible to use Unity Personal Edition to use this build option. Please refer to our EULA for further information.");
        public GUIContent eula = new GUIContent("Eula");
        //public string addToYourPro = L10n.Tr("Add {0} to your Unity Pro license");
        public GUIContent useNativeCompilation = new GUIContent("Use native compilation (no Mono runtime)");

        //public Texture2D activePlatformIcon = EditorGUIUtility.IconContent("BuildSettings.SelectedIcon").image as Texture2D;

        public const float kButtonWidth = 110;

        public string shopURL = "https://store.unity3d.com/shop/";
        const string kDownloadURL = "http://unity3d.com/unity/download/";
        const string kMailURL = "http://unity3d.com/company/sales?type=sales";
        const string kPlatformInstallationURL = "https://unity3d.com/platform-installation";

        public GUIContent GetDownloadErrorForTarget(BuildTarget target)
        {
            return null;
        }

        // string and matching enum values for standalone subtarget dropdowm
        public GUIContent debugBuild = new GUIContent("Development Build");
        public GUIContent datalessBuild = new GUIContent("Dataless Build");
        public GUIContent profileBuild = new GUIContent("Autoconnect Profiler");
        public GUIContent vrRemoteStremaing = new GUIContent("VR Remote Streaming");
        public GUIContent allowDebugging = new GUIContent("Script Debugging");
        public GUIContent waitForManagedDebugger = new GUIContent("Wait For Managed Debugger", "Show a dialog where you can attach a managed debugger before any script execution.");
        public GUIContent explicitNullChecks = new GUIContent("Explicit Null Checks");
        public GUIContent explicitDivideByZeroChecks = new GUIContent("Divide By Zero Checks");
        public GUIContent explicitArrayBoundsChecks = new GUIContent("Array Bounds Checks");
        public GUIContent learnAboutUnityCloudBuild = new GUIContent("Learn about Unity Cloud Build");
        public GUIContent compressionMethod = new GUIContent("Compression Method", "Compression applied to Player data (scenes and resources).\nDefault - none or default platform compression.\nLZ4 - fast compression suitable for Development Builds.\nLZ4HC - higher compression rate variance of LZ4, causes longer build times. Works best for Release Builds.");

        //public Compression[] compressionTypes =
        //{
        //    Compression.None,
        //    Compression.Lz4,
        //    Compression.Lz4HC
        //};

        public GUIContent[] compressionStrings =
        {
                new GUIContent("Default"),
                new GUIContent("LZ4"),
                new GUIContent("LZ4HC"),
            };
    }




    public enum Platform
    {
        Android,
        IOS,
        PC
    }

    public class SettingEditor : EditorWindow
    {
        private const string _defaut = "NewGame_";

        private static SettingEditor window;
        private int _gridId;

        private int _toolBarNum;

        private bool _foldoutValue;

        private List<string> _gameList = new List<string>() { _defaut + 0 };

        private int _gameNum;
        /// <summary>
        /// 版本号
        /// </summary>
        private string _versionStr;
        /// <summary>
        /// 打包的名字
        /// </summary>
        private string _packageName;
        /// <summary>
        /// BundleID
        /// </summary>
        private string _bundleId;
        /// <summary>
        /// 服务器Url
        /// </summary>
        private string _urlStr;
        /// <summary>
        /// Icon的路径
        /// </summary>
        private string _iconPath;
        /// <summary>
        /// 平台
        /// </summary>
        private int _platform;
        /// <summary>
        /// Icon的图片
        /// </summary>
        private Texture _iconImage = new Texture();
        /// <summary>
        /// 平台的枚举
        /// </summary>
        private Platform _platformEnum;
        /// <summary>
        /// 保存的数据的路径
        /// </summary>
        private string _infoPath = "/CommonFrame/SettingInfo.txt";

        private List<string> _scenesList = new List<string>();

        private string _scenesStr;

        private EditorBuildSettingsScene[] _scenes;

        private const int _androidIconNum = 6;

        private const int _iosIconNum = 19;

        [MenuItem("Build/Setting/PlayerSetting")]
        private static void CreateWindow()
        {
            window = (SettingEditor)EditorWindow.GetWindow(typeof(SettingEditor), false, "PleyerSetting");
            window.minSize = new Vector2(500, 500);
            //获取保存的值并赋值
            window.Show();
        }

        private void OnGUI()
        {
            BeginWindows();
            GUILayout.Window(_gridId, new Rect(0, 0, 150, window.position.height), ProjectManager, "项目管理");
            GUILayout.Window(-1, new Rect(150, 0, window.position.width - 150, window.position.height), BuildsInfo, "Setting");
            EndWindows();
        }

        private void ProjectManager(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Button("+");
            GUILayout.Button("-");
            GUILayout.EndHorizontal();
        }

        private const string kEditorBuildSettingsPath = "ProjectSettings/EditorBuildSettings.asset";

        static Styles styles = null;

        BuildPlayerSceneTreeView m_TreeView = null;
        [SerializeField]
        UnityEditor.IMGUI.Controls.TreeViewState m_TreeViewState;
        void ActiveScenesGUI()
        {
            if (m_TreeView == null)
            {
                if (m_TreeViewState == null)
                    m_TreeViewState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_TreeView = new BuildPlayerSceneTreeView(m_TreeViewState);
                m_TreeView.Reload();
            }
            Rect scenesInBuildRect = GUILayoutUtility.GetRect(styles.scenesInBuild, styles.title);
            GUI.Label(scenesInBuildRect, styles.scenesInBuild, styles.title);

            Rect rect = GUILayoutUtility.GetRect(100, position.width, 100, position.height);
            m_TreeView.OnGUI(rect);
        }



        private void BuildsInfo(int id)
        {
            _versionStr = EditorGUILayout.TextField("Version:", _versionStr);
            _packageName = EditorGUILayout.TextField("Package Name:", _packageName);
            _bundleId = EditorGUILayout.TextField("Bundle Id:", _bundleId);
            _urlStr = EditorGUILayout.TextField("Url:", _urlStr);
            _platformEnum = (Platform)EditorGUILayout.EnumPopup("选择发布平台:", _platformEnum, GUILayout.Width(300));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Icon图片:");
            GUILayout.Box(_iconImage, new[] { GUILayout.Height(100), GUILayout.Width(100) });
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            Rect rect = EditorGUILayout.GetControlRect();
            //将上面的框作为文本输入框  
            _iconPath = EditorGUI.TextField(rect, "IconPath:", _iconPath);
            //EditorGUI.LabelField(rect, "IconPath:", _iconPath);

            if (((Event.current.type == EventType.DragUpdated) || (Event.current.type == EventType.DragExited)) && rect.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    GetIconImage(DragAndDrop.paths[0]);
                }
            }


            if (GUILayout.Button("浏览", GUILayout.Width(100)))
            {
                OpenFileName openFileName = new OpenFileName();
                openFileName.structSize = Marshal.SizeOf(openFileName);
                openFileName.filter = "png(*.png)\0*.png";
                openFileName.file = new string(new char[256]);
                openFileName.maxFile = openFileName.file.Length;
                openFileName.fileTitle = new string(new char[64]);
                openFileName.maxFileTitle = openFileName.fileTitle.Length;
                openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
                openFileName.title = "窗口标题";
                openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
                if (LocalDialog.GetOpenFileName(openFileName))
                {
                    GetIconImage(openFileName.file);
                }
            }
            GUILayout.EndHorizontal();


            if (styles == null)
            {
                styles = new Styles();
                styles.toggleSize = styles.toggle.CalcSize(new GUIContent("X"));
            }

            //if (!UnityConnect.instance.canBuildWithUPID)
            //{
            //    ShowAlert();
            //}
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();

            string message = "";
            var buildSettingsLocked = !AssetDatabase.IsOpenForEdit(kEditorBuildSettingsPath, out message, StatusQueryOptions.UseCachedIfPossible);

            using (new EditorGUI.DisabledScope(buildSettingsLocked))
            {
                ActiveScenesGUI();
                // Clear all and Add Current Scene
                GUILayout.BeginHorizontal();
                if (buildSettingsLocked)
                {
                    GUI.enabled = true;

                    if (Provider.enabled && GUILayout.Button(styles.checkOut))
                    {
                        Asset asset = Provider.GetAssetByPath(kEditorBuildSettingsPath);
                        var assetList = new AssetList();
                        assetList.Add(asset);
                        Provider.Checkout(assetList, CheckoutMode.Asset);
                    }
                    GUILayout.Label(message);
                    GUI.enabled = false;
                }
                GUILayout.FlexibleSpace();
                //if (GUILayout.Button(styles.addOpenSource))
                //    AddOpenScenes();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            //ActiveBuildTargetsGUI();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            //ShowBuildTargetSettings();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            //if (GUILayout.Button("Save") && _gridId == 0)
            //{
            //    ApplyInfo();
            //}
            if (GUILayout.Button("Build", GUILayout.Width(100)))
            {
                OnBuildClick();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Apply", GUILayout.Width(100)) && _gridId == 0)
            {
                ApplyInfo();
            }
            if (GUILayout.Button("Cancle", GUILayout.Width(100)))
            {
                Close();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void AddGame(int num)
        {
            _gameList.Add(_defaut + num);
        }

        private void GetIconImage(string path)
        {
            if (!path.EndsWith(".png"))
            {
                _iconImage = null;
                return;
            }
            _iconPath = path;
            Texture2D tx = new Texture2D(100, 100);
            tx.LoadImage(GetImageByte(path));
            _iconImage = tx;
        }

        private byte[] GetImageByte(string path)
        {
            FileStream fs = new FileStream(path, System.IO.FileMode.Open);
            if (fs.Length < 0) return null;
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();
            return img;
        }

        private void ApplyInfo()
        {
            if (string.IsNullOrEmpty(_versionStr) || !Regex.IsMatch(_versionStr, "[0-9].[0-9].[0-9]"))
            {
                Debug.Log("输入的版本号格式不对或者为空,应输入1.0.1");
                return;
            }
            if (string.IsNullOrEmpty(_packageName))
            {
                Debug.Log("打包名不可以为空!");
            }
            if (string.IsNullOrEmpty(_bundleId) || !Regex.IsMatch(_bundleId, "[a-z]*.[a-z]*.[a-z]*"))
            {
                Debug.Log("输入的BundleId格式不对或者为空,应输入xxx.xxxx.xxxx");
                return;
            }
            if (string.IsNullOrEmpty(_urlStr) || !_urlStr.EndsWith("\\"))
            {
                Debug.Log("输入的Url的格式不是以\\结尾,请重新输入!");
                return;
            }
            if (string.IsNullOrEmpty(_iconPath) || !_iconPath.EndsWith(".png"))
            {
                Debug.Log("选择的Icon的格式不是png格式,请重新选择!");
                return;
            }


            if (_platformEnum == Platform.Android)
            {

                var icons = new Texture2D[_androidIconNum];
                for (int i = 0; i < icons.Length; i++)
                {
                    icons[i] = (Texture2D)_iconImage;
                }
                PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, icons);
            }
            if (_platformEnum == Platform.IOS)
            {
                var icons = new Texture2D[_iosIconNum];
                for (int i = 0; i < icons.Length; i++)
                {
                    icons[i] = (Texture2D)_iconImage;
                }
                PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, icons);
            }
            PlayerSettings.bundleVersion = _versionStr;


            //把数据保存到json文件中
            Hashtable table = new Hashtable();
            table.Add("version", _versionStr);
            table.Add("name", _packageName);
            table.Add("bundle", _bundleId);
            table.Add("url", _urlStr);
            table.Add("icon", _iconPath);
            table.Add("platform", (int)_platformEnum);
        }

        private void OnBuildClick()
        {

            if (_platformEnum == Platform.Android && EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);//切换平台方法
            }
            if (_platformEnum == Platform.IOS && EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);//切换平台方法
            }
        }

        private void GetHistoryInfo()
        {
            StreamReader sr;
            var file = new FileInfo(Application.dataPath + _infoPath);
            if (!file.Exists)
                return;
            sr = File.OpenText(Application.dataPath + _infoPath);
            var list = new ArrayList();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                list.Add(line);
            }
            sr.Close();
            sr.Dispose();
            if (list.Count <= 0)
            {
                Debug.Log("SettingInfo文档中没有数据.");
                return;
            }
            string str = null;
            foreach (string item in list)
            {
                str += item;
            }
            JsonData data = JsonMapper.ToObject(str);
            _versionStr = data.Keys.Contains("version") ? data["version"].ToString() : "";//版本号
            _packageName = data.Keys.Contains("name") ? data["name"].ToString() : "";//打包名
            _bundleId = data.Keys.Contains("bundle") ? data["bundle"].ToString() : "";//BundleId
            _urlStr = data.Keys.Contains("url") ? data["url"].ToString() : "";//Url
            _iconPath = data.Keys.Contains("icon") ? data["icon"].ToString() : "";//Icon地址
            _platform = data.Keys.Contains("platform") && string.IsNullOrEmpty(data["platform"].ToString()) ? int.Parse(data["platform"].ToString()) : 0;//打包平台
            _platformEnum = (Platform)_platform;
        }
    }
}