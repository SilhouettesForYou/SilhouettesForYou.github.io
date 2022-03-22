
using System;
using System.Collections.Generic;
using UnityEngine;
using CSVEditor;

namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    [Serializable]
    public class BuffEffectShowData : CustomTriggerData
    {
        private void OnEnable()
        {
            _type = eCustomItemObjectType.Buff;
            triggerFoldPath = GlobalConfig.buff_trigger_dir;
        }

        public override void OnInit(int UID, string _csvName = "BuffEffectTable")
        {
            tableNamePrefix = "BuffEffectTable/";
            tableName = "BuffEffectTable";
            ItemName = "buff" + UID.ToString();
            base.OnInit(UID, _csvName);
        }

        #region 字段信息
        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("效果id")]
        public int EffectId;

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("效果说明")]
        [PropertyTooltip("策划用")]
        public string Comment;

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("间隔时间")]
        [PropertyTooltip("配合触发时机1使用, 单位秒")]
        public float IntervalTime;

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("使用新流程")]
        public bool UseNewSystem;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("内置触发CD")]
        public float TriggerCd;

        [FoldoutGroup("范围配置", expanded: false)]
        [ShowInInspector, LabelText("buff范围")]
        [PropertyTooltip("光环类Buff, 配合触发时机为范围时生效, -1为无限大，6代表半径为6的圆形，6|5代表长6宽5的方形")]
        public float[] BuffRange;

        [FoldoutGroup("范围配置", expanded: false)]
        [ShowInInspector, LabelText("buff范围内生效人数")]
        [PropertyTooltip("触发时机为8.暗壁类时填写，范围内随机此数量")]
        public int BuffRangeCount;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("属性监听")]
        public int[] AttrListener;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("触发时机")]
        public uint[] TriggerTiming;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("触发条件")]
        [PropertyTooltip("lua脚本")]
        public string TriggerCondition;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("触发音效")]
        public int TriggerAudio;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("效果")]
        [PropertyTooltip("详见对应sheet")]
        public int EffectType;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("效果参数")]
        public string EffectParam;

        [HideInInspector]
        public enum TargetTypeEnum
        {
            [LabelText("自己")]
            Self = 0,         // 自己
            [LabelText("友方包括自己")]
            Friends = 1,      // 友方包括自己
            [LabelText("敌方")]
            Enemy = 2,        // 敌方
            [LabelText("所有人")]
            All = 3,          // 所有人
            [LabelText("死亡的友方")]
            DeadFriend = 4,   // 死亡的友方
            [LabelText("队友包括自己")]
            Team = 5,         // 队友包括自己
            [LabelText("友方不包括自己")]
            TeamExSelf = 6,          // 友方不包括自己
            [LabelText("最初召唤者")]
            InitialSummoner = 7, // 最初召唤者
            [LabelText("自己的宠物")]
            SelfPet = 8 // 自己的宠物
        }
        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("目标类型")]
        public TargetTypeEnum _TargetType;
        [HideInInspector]
        public int TargetType
        {
            get
            {
                return (int)_TargetType;
            }
            set
            {
                _TargetType = (TargetTypeEnum)value;
            }
        }

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("触发特效")]
        [PropertyTooltip("特效id=挂点类型【0具体挂点，1身高百分比】=对应类型的值【具体挂点或百分比】")]
        public string Fx;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("是否通知客户端触发时机")]
        [PropertyTooltip("1，通知，0，不通知。默认不通知")]
        public bool NeedOnTriggerSync;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("触发特效")]
        [Obsolete("废弃")]
        public int TriggerFx;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("内置触发共CD")]
        public float TriggerGroupCd;

        [FoldoutGroup("触发配置", expanded: false)]
        [ShowInInspector, LabelText("内置触发CD组")]
        public uint TriggerCdGroupType;
        #endregion

        protected override void OnLoadCSV()
        {
            base.OnLoadCSV();
        }

        protected override void OnSaveCSV()
        {
            base.OnSaveCSV();
        }

        #region 保存&加载&其他生命周期
        public override void CreateTrigger()
        {

        }

        [ButtonGroup]
        [Button("保存", ButtonSizes.Medium), GUIColor(120 / 255.0f, 151 / 255.0f, 171 / 255.0f)]
        private void SaveCSV()
        {
            OnSaveCSV();
            UnityEditor.EditorWindow.focusedWindow.ShowNotification(new GUIContent("保存成功"));
        }

        [ButtonGroup]
        [Button("加载", ButtonSizes.Medium), GUIColor(216 / 255.0f, 133 / 255.0f, 163 / 255.0f)]
        private void LoadCSV()
        {
            OnLoadCSV();
            UnityEditor.EditorWindow.focusedWindow.ShowNotification(new GUIContent("加载成功"));
        }
        #endregion
    }
}
