using CSVEditor;

#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using System;
    using UnityEditor;
    using UnityEngine;
    using SSSEditor.Utilities;
    using System.IO;
    using MoonCommonLib;
    using ToolLib;
    using Aspose.Cells;
    using System.Collections.Generic;

    //[InitializeOnLoad]
    public class RPGEditorWindow : OdinMenuEditorWindowExt
    {
        static void CheckGitHook()
        {
            string dataPath = Application.dataPath;
            string gitDir = dataPath.Substring(0, dataPath.Length - 26);
            string commitMsgFilePath = gitDir + ".git/hooks/commit-msg";
            if (!System.IO.File.Exists(commitMsgFilePath))
            {
                System.IO.File.Copy(gitDir + "commit-msg", commitMsgFilePath);
            }
        }

        public static bool needSaveData = false;
        private TabBase m_currentTab;
        public bool RefreshMenuTree = false;

        public static RPGEditorWindow MainWindow;

        public TabBase CurrentTab
        {
            get { return m_currentTab; }
        }
        [MenuItem("欢乐编辑器/打开编辑器", priority = 0)]
        public static void Open()
        {
            if (Application.isPlaying == true)
            {
                UnityEditor.EditorWindow.focusedWindow.ShowNotification(new GUIContent("游戏正在运行中"));
                Debug.LogError("游戏正在运行中");
                return;
            }
            //EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            MainWindow = GetWindow<RPGEditorWindow>();
            MainWindow.position = GUIHelper.GetEditorWindowRect().AlignCenter(1280, 800);
            MainWindow.minSize = new Vector2(1280, 800);
            MainWindow.titleContent = new GUIContent("主窗口");//, GlobalConfig.window_logo);
            //EditorSceneManager.OpenScene(GlobalConfig.def_scene_path);
            EditorWindowFocusMgr.Instance().PushEditorWindow(MainWindow, (int)TriggerWindowType.Event);
            MainWindow.LoadEditorConfig();
            //MainWindow.OnAssetDynLoadFinish += MainWindow.AssetDynLoadFinished;
            MainWindow.Show();
            MainWindow.Init();
        }

        [MenuItem("欢乐编辑器/AttrDecisionImport", priority = 24)]
        public static void AttrDeisionImport()
        {

            TriggerTypeConfig config = AssetDatabase.LoadAssetAtPath<TriggerTypeConfig>("Assets/artres/w3/Tools/SSSEditor/Data/trigger_config_type/TriggerPlayerAttributeConfig.asset");
            if (config == null)
            {
                return;
            }

            string fileName = Path.Combine(Util.MoonClientConfigPath, "Table/CSV/AttrDecision.csv");
            if (!File.Exists(fileName))
            {
                return;
            }

            Workbook workBook = new Workbook(fileName, new LoadOptions(LoadFormat.CSV));
            if (workBook == null)
            {
                return;
            }

            List<TypeTemplate> temp = config.myTypeTempalte;
            config.myTypeTempalte.Clear();

            TypeTemplate typeTemplate = new TypeTemplate();
            typeTemplate.id = 0;
            typeTemplate.type = "RO属性";
            typeTemplate.description = "RO属性";
            typeTemplate.category = config.ItemName;
            typeTemplate.has_preset = true;
            typeTemplate.is_hide = false;
            typeTemplate.link_type_name = string.Empty;
            typeTemplate.object_type_name = string.Empty;

            List<PresetTemplate> presetsList = new List<PresetTemplate>();
            foreach (Worksheet workSheet in workBook.Worksheets)
            {
                for (int i = 2; i <= workSheet.Cells.MaxDataRow; ++i)
                {
                    string value = workSheet.Cells[i, 0].StringValue;
                    string name = workSheet.Cells[i, 1].StringValue;

                    var preset = new PresetTemplate();
                    preset.id = name;
                    preset.description = name;
                    preset.typeName = "整数";
                    preset.name = name;
                    preset.comment = name;
                    preset.defaultValue = value;
                    preset.code = value;
                    presetsList.Add(preset);
                }

                break;
            }

            typeTemplate.presets = presetsList.ToArray();
            config.myTypeTempalte.Add(typeTemplate);

            config.dirty = true;
            AssetDatabase.SaveAssets();
        }

        [MenuItem("欢乐编辑器/ConstTypeConfigCopy", priority = 24)]
        public static void ConstTypeConfigCopy()
        {
            TriggerTypeConfig src = AssetDatabase.LoadAssetAtPath<TriggerTypeConfig>("Assets/artres/w3/Tools/SSSEditor/Data/trigger_config_type/TriggerConstTypeConfig1.asset");
            if (src == null)
            {
                return;
            }

            TriggerTypeConfig des = AssetDatabase.LoadAssetAtPath<TriggerTypeConfig>("Assets/artres/w3/Tools/SSSEditor/Data/trigger_config_type/TriggerConstTypeConfig.asset");
            if (des == null)
            {
                return;
            }

            string addType = "装备部位";

            for (int n = 0; n < des.myTypeTempalte.Count; n++)
            {
                var desTypeTemplate = des.myTypeTempalte[n];
                if (desTypeTemplate.type == addType)
                {
                    des.myTypeTempalte.RemoveAt(n);
                    break;
                }
            }


            for (int n = 0; n < src.myTypeTempalte.Count; n++)
            {
                var srcTypeTemplate = src.myTypeTempalte[n];

                if (srcTypeTemplate.type != addType)
                {
                    continue;
                }

                TypeTemplate desTypeTemplate = new TypeTemplate();
                desTypeTemplate.id = srcTypeTemplate.id;
                desTypeTemplate.description = srcTypeTemplate.description;
                desTypeTemplate.category = srcTypeTemplate.category;
                desTypeTemplate.has_preset = srcTypeTemplate.has_preset;
                desTypeTemplate.default_value = srcTypeTemplate.default_value;
                desTypeTemplate.extra_selector = srcTypeTemplate.extra_selector;
                desTypeTemplate.input_default_value = srcTypeTemplate.input_default_value;
                desTypeTemplate.internal_type = srcTypeTemplate.internal_type;
                desTypeTemplate.is_can_from_other = srcTypeTemplate.is_can_from_other;
                desTypeTemplate.is_hide = srcTypeTemplate.is_hide;
                desTypeTemplate.link_type_name = srcTypeTemplate.link_type_name;
                desTypeTemplate.object_type_name = srcTypeTemplate.object_type_name;
                desTypeTemplate.type = srcTypeTemplate.type;

                List<PresetTemplate> presetsList = new List<PresetTemplate>();
                for (int i = 0; i < srcTypeTemplate.presets.Length; i++)
                {
                    var srcPreset = srcTypeTemplate.presets[i];
                    var desPreset = new PresetTemplate();
                    desPreset.classification = srcPreset.classification;
                    desPreset.code = srcPreset.code;
                    desPreset.comment = srcPreset.comment;
                    desPreset.defaultValue = srcPreset.defaultValue;
                    desPreset.description = srcPreset.description;
                    desPreset.has_locals = srcPreset.has_locals;
                    desPreset.id = srcPreset.id;
                    desPreset.name = srcPreset.name;
                    desPreset.typeName = srcPreset.typeName;

                    desPreset.varirable_locals = new LocalsParamTamplate[srcPreset.varirable_locals.Length];
                    for (int j = 0; j < srcPreset.varirable_locals.Length; j++)
                    {
                        desPreset.varirable_locals[j] = srcPreset.varirable_locals[j].Clone();
                    }

                    presetsList.Add(desPreset);
                }

                desTypeTemplate.presets = presetsList.ToArray();

                des.myTypeTempalte.Add(desTypeTemplate);
            }



            des.dirty = true;
            AssetDatabase.SaveAssets();
        }

        private void Init()
        {
//            WwiseEditorInit.LoadWwiseBank();
        }

        //配置容器，保存后运行时切换不用重新加载静态数据
        [NonSerialized]
        public SysFuncTemplateContaner functionContainer;
        [NonSerialized]
        public TypeTemplateContaner typeContainer;
        [NonSerialized]
        public SceneItemTemplateCantainer itemContainer;
        [NonSerialized]
        public UserFunctionContaner userFunctionContainer;
        //[NonSerialized]
        //public AITemplateContainer aiTemplateContainer;
        [NonSerialized]
        public TriggerColorConfig colorContainer;

        private void LoadEditorConfig()
        {
            TriggerColorConfig.Instance().LoadColorCfg();
            if(GlobalConfig.autoTriggerTexDict == null) GlobalConfig.InitAutoTriggerTex();
            //技能部分可能用到此配置，暂时在这里提前加载
            if (itemContainer == null)
            {
                itemContainer = SceneItemTemplateCantainer.Instance();
                itemContainer.LoadAssets();
            }
        }

        /// <summary>
        /// 加载触发器配置文件
        /// 函数配置，类型配置，模板配置，自定义函数配置，ai模板加载
        /// </summary>
        public void LoadTriggerConfig()
        {
            bool clearBar = false;
            if (functionContainer == null || functionContainer.HasDataInvalid())
            {
                clearBar = true;
                EditorUtility.DisplayProgressBar("加载", "加载函数配置", 0f);
                functionContainer = SysFuncTemplateContaner.Instance();
                functionContainer.LoadFuncTemplateAsset();
            }
            if (typeContainer == null || typeContainer.HasDataInvalid())
            {
                clearBar = true;
                EditorUtility.DisplayProgressBar("加载", "加载类型配置", 0.5f);
                typeContainer = TypeTemplateContaner.Instance();
                typeContainer.LoadTypeTemplateAsset();
                EditorUtility.DisplayProgressBar("加载", "加载触发器配置", 1f);
            }
            if(clearBar) EditorUtility.ClearProgressBar();
        }

        public void LoadTriggerDialogConfig()
        {
            bool clearBar = false;

            if (itemContainer == null)
            {
                clearBar = true;
                EditorUtility.DisplayProgressBar("加载", "加载模板配置", 0f);
                itemContainer = SceneItemTemplateCantainer.Instance();
                itemContainer.LoadAssets();
            }

            if (userFunctionContainer == null)
            {
                clearBar = true;
                EditorUtility.DisplayProgressBar("加载", "加载自定义函数配置", 0.4f);
                userFunctionContainer = UserFunctionContaner.Instance();
                userFunctionContainer.LoadGlobalUserFunctions();
                userFunctionContainer.LoadGlobalAIUserFunctions();

            }
            else
            {
                if (userFunctionContainer.HasDataIsNull(userFunctionContainer.GlobalUserFunctionDict))
                {
                    Debug.Log("---->userFunctionContainer.LoadGlobalUserFunctions");
                    userFunctionContainer.LoadGlobalUserFunctions();
                }
                if (userFunctionContainer.HasDataIsNull(userFunctionContainer.GlobalAIUserFunctionDict))
                {
                    Debug.Log("---->userFunctionContainer.LoadGlobalAIUserFunctions");
                    userFunctionContainer.LoadGlobalAIUserFunctions();
                }
            }

//            if (aiTemplateContainer == null)
//            {
//                clearBar = true;
//                EditorUtility.DisplayProgressBar("加载", "加载AI配置", 0.8f);
//                aiTemplateContainer = AITemplateContainer.Instance();
//                aiTemplateContainer.LoadAITemplateAsset();
//                EditorUtility.DisplayProgressBar("加载", "加载配置完成", 1f);
//            }
//            else
//            {
//                if (aiTemplateContainer.HasDataIsNull())
//                {
//                    aiTemplateContainer.LoadAITemplateAsset();
//                }
//            }

            if(clearBar) EditorUtility.ClearProgressBar();
        }

        private static void PlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.EnteredPlayMode)
            {
                if (MainWindow)
                {
                    MainWindow._rendering = false;
                }
            }
        }

        protected override void OnDestroy()
        {
            m_currentTab.OnDisable();
            EditorApplication.playModeStateChanged -= PlayModeState;
            if( Application.isPlaying )
            {
                return;
            }
            //SceneView.onSceneGUIDelegate -= OnSceneGUI;
            // 先关闭子界面，在做这个界面的清理
            CloseAllWindow();
            base.OnDestroy();
            SceneItemTemplateCantainer.Instance().Clear();
            EditorWindowFocusMgr.Instance().PopEditorWindow(this, (int)TriggerWindowType.Event);
            if(RPGEditorWindow.needSaveData)
            {
                RPGEditorWindow.needSaveData = false;
                AssetDatabase.SaveAssets();
            }
        }

        protected void CloseAllWindow()
        {
            if (ScenesEditorWindow.MainWindow != null )
            {
                ScenesEditorWindow.MainWindow.Close();
            }
            ScenesEditorWindow.MainWindow = null;
            if(SceneTrigerRunWindow.MainWindow != null)
            {
                SceneTrigerRunWindow.MainWindow.Close();
            }
            SceneTrigerRunWindow.MainWindow = null;
            // 关闭技能编辑器界面
//            if(SkillAnimationEditor.WindowIsInit) 
//            {
//                SkillAnimationEditor.CurrentWindow.Close();
//            }
            // 关闭资源面板
            if (ResourceMgrWindow.WindowIsInit) 
            {
                ResourceMgrWindow.CurrentWindow.Close();
            }

            for(int i=0; i<3; i++)
            {
                var userWindowHandle = GetWindow<UserFunctionWindow>();
                if(userWindowHandle != null)
                {
                    userWindowHandle.Close();
                }
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = m_currentTab.BuildMenuTree(this.isDirty);
            tree.EnumerateTree().ForEach(x => {x.OnDrawItem -= DrawOdinMenuItemExt; x.OnDrawItem += DrawOdinMenuItemExt;});
            return tree;
        }

        private void DrawOdinMenuItemExt(OdinMenuItem odinMenuItem) 
        {
            // var obj = odinMenuItem?.Value as CustomItemObject;
            // if(obj != null)
            // {
            //     if(obj.obsolete) 
            //     {
            //         var rect = odinMenuItem.Rect;
            //         GUIHelper.PushColor(Color.yellow);
            //         SirenixEditorGUI.DrawBorders(rect.AlignRight(rect.width - rect.height).AlignCenterY(4f), 2);
            //         GUIHelper.PopColor();
            //         GUI.DrawTexture(rect.AlignLeft(rect.height), GlobalConfig.obj_obstacle_tex);
            //     }
            // }

            OdinMenuItemExt menuExt = odinMenuItem as OdinMenuItemExt;
            if(menuExt != null && !string.IsNullOrEmpty(menuExt.AssetPath)) 
            {
                var extData = EditorDataPersistentConfig.Instance.GetDataByAssetPath(menuExt.AssetPath);
                if(extData != null && extData.obsolete) 
                {
                    var rect = odinMenuItem.Rect;
                    GUIHelper.PushColor(Color.yellow);
                    SirenixEditorGUI.DrawBorders(rect.AlignRight(rect.width - rect.height).AlignCenterY(4f), 2);
                    GUIHelper.PopColor();
                    GUI.DrawTexture(rect.AlignLeft(rect.height), GlobalConfig.obj_obstacle_tex);
                }
            }
        }

        public void RemoveSelectedItemTrigger()
        {
            m_currentTab.DeleteTrigger();
        }

        protected const string options1 = "文件" ;
        protected const string options2 = "编辑" ;
        protected const string options3 = "设置" ;
        protected const string options4 = "扩展" ;
        protected const string options5 = "资源" ;

        protected Rect buttonRect1 = new Rect(0, -5, "文件".Length* 25, 24);
        protected Rect buttonRect2 = new Rect(50, -5, "编辑".Length* 25, 24);
        protected Rect buttonRect3 = new Rect(100, -5, "设置".Length* 25, 24);
        protected Rect buttonRect4 = new Rect(150, -5, "设置".Length * 25, 24);
        protected Rect buttonRect5 = new Rect(200, -5, "资源".Length * 25, 24);

        public static string[] ToolbarContentArr = new string[] {"技能", "效果", "函数配置", "类型配置", };
        public TabBase[] TabInstances;

        private RPGEditorWindow()
        {
            EditorApplication.playModeStateChanged -= PlayModeState;
            EditorApplication.playModeStateChanged += PlayModeState;
        }

        protected int selectindex = 0;
        public CommonMenuWindow menu = null;

        public int select = 0;
        
        private void OnFocus()
        {
            if (m_currentTab != null)
                m_currentTab.OnFocus();
            //Debug.Log("OnFocus");
            EditorWindowFocusMgr.Instance().FoucusWindow(this, (int)TriggerWindowType.Event);
        }

        private void OnLostFocus()
        {
            if (m_currentTab != null)
            {
                m_currentTab.OnLostFocus();
            }
        }

        protected void OnMouseChangePosition()
        {
            Vector2 mousePosition = Event.current.mousePosition;
            if (selectindex > 0)
            {
                if (buttonRect1.Contains(mousePosition) && selectindex != 1)
                {
                    menu.CloseAndChild();
                    menu.Close();
                    CreatMenuFile();
                }

                if (buttonRect2.Contains(mousePosition) && selectindex != 2)
                {
                    menu.CloseAndChild();
                    menu.Close();
                    CreatMenuEdit();
                }

                if (buttonRect3.Contains(mousePosition) && selectindex != 3)
                {
                    menu.CloseAndChild();
                    menu.Close();
                    CreatMenuSet();
                }

                if (buttonRect4.Contains(mousePosition) && selectindex != 4)
                {
                    menu.CloseAndChild();
                    menu.Close();
                    CreatMenuExtend();
                }

                if (buttonRect5.Contains(mousePosition) && selectindex != 5)
                {
                    menu.CloseAndChild();
                    menu.Close();
                    CreatResMgrMenu();
                }
                this.Repaint();
            }
        }
        protected void CreatMenuFile()
        {
            menu = new CommonMenuWindow();
            menu.AddItem("文件/打开", 0, MenuFunction.MenuSave);
            menu.AddItem("文件/保存所有", 0, MenuFunction.MenuSave);
            menu.AddItem("文件/保存并检测", 0, MenuFunction.MenuFile);
            menu.AddSeparator();
            menu.AddItem("导出语言文件", 0, MenuFunction.MenuFile);
            menu.AddItem("载入语言文件", 0, MenuFunction.MenuFile);
            menu.AddSeparator();
            menu.AddItem("检测无用资源", 0, MenuFunction.MenuFile);
            menu.AddItem("导出资源", 0, MenuFunction.MenuFile);
            menu.AddItem("播放", 0, MenuFunction.MenuFile);
            selectindex = 1;
            menu.DropDown(buttonRect1, position, delegate () {
                selectindex = 0;
                menu = null;
            });
        }
        protected void CreatMenuEdit()
        {
            menu = new CommonMenuWindow();
            menu.AddItem("编辑", 0, MenuFunction.MenuFile);
            menu.AddItem("拷贝文本", 0, MenuFunction.MenuSave);
            selectindex = 2;
            menu.DropDown(buttonRect2, position, delegate () {
                selectindex = 0;
                menu = null;
            });
        }
        protected void CreatMenuSet()
        {
            menu = new CommonMenuWindow();
            menu.AddItem("设置/1", 0, MenuFunction.MenuFile);
            menu.AddItem("设置/2", 0, MenuFunction.MenuFile);
            menu.AddItem("游戏参数", 0, MenuFunction.MenuSave);
            menu.AddItem("扩展参数", 0, MenuFunction.MenuSave);
            menu.AddItem("定义地图块", 0, MenuFunction.MenuSave);
            menu.AddItem("定义动作", 0, MenuFunction.MenuSave);
            selectindex = 3;
            menu.DropDown(buttonRect3, position, delegate () {
                selectindex = 0;
                menu = null;
            });
        }
        protected void CreatMenuExtend()
        {
            menu = new CommonMenuWindow();
            menu.AddItem("扩展/1/1", 0, MenuFunction.MenuFile,"ctrl + a");
            menu.AddItem("扩展/1/2", 0, MenuFunction.MenuFile, "ctrl + alt + x");
            menu.AddItem("扩展/1/3", 0, MenuFunction.MenuFile, "ctrl + c");
            menu.AddItem("扩展/1/4", 0, MenuFunction.MenuFile, "ctrl + v");
            menu.AddItem("扩展/2/2", 0, MenuFunction.MenuFile, "ctrl + b");
            menu.AddSeparator();
            menu.AddItem("配置/导出/单位", 0, AssetToExcel.ExportUnit, "ctrl + u");
            menu.AddItem("配置/导出/技能", 0, AssetToExcel.ExportSkill, "ctrl + s");
            menu.AddItem("配置/导出/军团", 0, AssetToExcel.ExportLegion, "");
            menu.AddItem("配置/导出/物品", 0, AssetToExcel.ExportItem, "ctrl + i");
            menu.AddItem("配置/导出/掉落组", 0, AssetToExcel.ExportDrop, "");
            menu.AddItem("配置/导出/外围系统", 0, AssetToExcel.ExportPeripheralSystem, "");
            menu.AddItem("配置/导出/匹配系统", 0, AssetToExcel.ExportMatchSystem, "");
            menu.AddItem("配置/导出/主城建筑", 0, AssetToExcel.ExportBuilding, "");
            menu.AddItem("配置/导出/沙盘数据", 0, AssetToExcel.ExportSandbox, "");
            menu.AddItem("配置/导入/单位", 0, AssetToExcel.ImportUnit, "ctrl + shift + u");
            menu.AddItem("配置/导入/技能", 0, AssetToExcel.ImportSkill, "ctrl + shift + s");
            menu.AddItem("配置/导入/军团", 0, AssetToExcel.ImportLegion, "");
            menu.AddItem("配置/导入/物品", 0, AssetToExcel.ImportItem, "ctrl + shift + i");
            menu.AddItem("配置/导入/掉落组", 0, AssetToExcel.ImportDrop, "");
            menu.AddItem("配置/导入/外围系统", 0, AssetToExcel.ImportPeripheralSystem, "");
            menu.AddItem("配置/导入/匹配系统", 0, AssetToExcel.ImportMatchSystem, "");
            menu.AddItem("配置/导入/主城建筑", 0, AssetToExcel.ImprotBuilding, "");
            menu.AddItem("配置/导入/沙盘数据", 0, AssetToExcel.ImprotSandbox, "");
            menu.AddSeparator();
            menu.AddItem("脚本编辑器", 0, MenuFunction.MenuSave, "ctrl + s");
            menu.AddItem("脚本编辑器1", 0, MenuFunction.MenuSave, "ctrl + d");
            menu.AddItem("脚本编辑器2", 0, MenuFunction.MenuSave, "ctrl + f");
            selectindex = 4;
            menu.DropDown(buttonRect4, position, delegate ()
            {
                selectindex = 0;
                menu = null;
            });
        }
        protected void CreatResMgrMenu()
        {
            // menu = new CommonMenuWindow();
            menu = ScriptableObject.CreateInstance<CommonMenuWindow>();
            menu.AddItem("打开资源管理器", 0, OpenResMgr);

            selectindex = 5;
            menu.DropDown(buttonRect5, position, delegate ()
            {
                selectindex = 0;
                menu = null;
            });
        }

        private void OpenResMgr() 
        {
            ResourceMgrWindow.Popup("", false, null, ResMgrSelectType.All);
        }

        private bool _rendering = false;
        protected override void OnGUI()
        {
            MainWindow = this;
            EditorStyles.popup.fontSize = 12;
            EditorStyles.popup.fixedHeight = 18;
            EditorStyles.popup.alignment = TextAnchor.MiddleCenter;
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                if (EditorGUILayout.DropdownButton(new GUIContent(options1),FocusType.Passive, selectindex == 1 ? CustomEditorStyle.dropdownButtonSelected : CustomEditorStyle.dropdownButtonNormal, GUILayout.Width(options1.Length * 25), GUILayout.Height(24)))
                {
                    CreatMenuFile();
                }
                if (EditorGUILayout.DropdownButton(new GUIContent(options2), FocusType.Passive, selectindex == 2 ? CustomEditorStyle.dropdownButtonSelected : CustomEditorStyle.dropdownButtonNormal, GUILayout.Width(options2.Length * 25), GUILayout.Height(24))) {
                    CreatMenuEdit();
                }
                if (EditorGUILayout.DropdownButton(new GUIContent(options3), FocusType.Passive, selectindex == 3 ? CustomEditorStyle.dropdownButtonSelected : CustomEditorStyle.dropdownButtonNormal, GUILayout.Width(options3.Length * 25), GUILayout.Height(24)))
                {
                    CreatMenuSet();
                }
                if (EditorGUILayout.DropdownButton(new GUIContent(options4), FocusType.Passive, selectindex == 4 ? CustomEditorStyle.dropdownButtonSelected : CustomEditorStyle.dropdownButtonNormal, GUILayout.Width(options4.Length * 25), GUILayout.Height(24)))
                {
                    CreatMenuExtend();
                }
                if (EditorGUILayout.DropdownButton(new GUIContent(options5), FocusType.Passive, selectindex == 5 ? CustomEditorStyle.dropdownButtonSelected : CustomEditorStyle.dropdownButtonNormal, GUILayout.Width(options5.Length * 25), GUILayout.Height(24)))
                {
                    CreatResMgrMenu();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
            OnMouseChangePosition();
            select = GUILayout.Toolbar(select, ToolbarContentArr, CustomEditorStyle.toolbarButton);
            TabBase selected = TabInstances[select];
            if (m_currentTab == null)
            {
                m_currentTab = selected;
                m_currentTab.Owner = this;
                m_currentTab.OnEnable();
            }
            else
            {
                if (m_currentTab != selected)
                {                            
                    m_currentTab.OnDisable();
                    m_currentTab = selected;
                    m_currentTab.Owner = this;
                    m_currentTab.OnEnable();
                    RefreshMenuTree = true;
                }
            }
            m_currentTab.OnGUI();
            base.OnGUI();
            if (RefreshMenuTree)
            {
                this.ForceMenuTreeRebuild();
                RefreshMenuTree = false;
            }
            if (!_rendering)
            {
                _rendering = true;
                //LoadEditorConfig();
            }
        }

        protected override void OnBeginDrawEditors()
        {
            m_currentTab.OnBeginDrawEditors();
        }

        protected override void OnEndDrawEditors()
        {
            m_currentTab.OnEndDrawEditors();
        }

        protected override void OnEnable()
        {            
            TabInstances = new TabBase[4];
            TabInstances[0] = new TabSkill(eMenuType.Skill);
            //TabInstances[1] = new TabBuff(eMenuType.Buff);
            TabInstances[1] = new BuffSuperTab();
            TabInstances[2] = new TabFunction(eMenuType.FunctionConfig);
            TabInstances[3] = new TabType(eMenuType.TypeConfig);

            //            TabInstances[0] = new TabScene(eMenuType.Scene);
            //            TabInstances[1] = new TabUnit(eMenuType.Player);
            //            TabInstances[2] = new TabLegion(eMenuType.Legion);
            //            TabInstances[3] = new TabSkill(eMenuType.Skill);
            //            TabInstances[5] = new TabArea(eMenuType.Area);
            //            TabInstances[6] = new TabItem(eMenuType.Item);
            //            TabInstances[7] = new TabGlobalData();
            //TabInstances[10] = new TabGlobalConfig(eMenuType.GlobalConfig);
            //            TabInstances[11] = new TabAI(eMenuType.AI);
            //TabInstances[12] = new TabUI(eMenuType.UI);
            //TabInstances[13] = new TabProtocol(eMenuType.Protocol);

            // CheckGitHook();
            _rendering = false;
            base.OnEnable();
        }
    }
}
#endif
