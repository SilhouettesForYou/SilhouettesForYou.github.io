using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CSVEditor;
using MoonCommonLib;
using ToolLib.Excel;
using LitJson;
using Serializer;
using ToolLib.CSV;
using W4Editor.QuickEnter;

namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    [Serializable]
    public class SkillShowData : CustomTriggerData
    {
        void OnEnable()
        {
            _type = eCustomItemObjectType.Skill;
            triggerFoldPath = GlobalConfig.skill_trigger_dir;
        }

        public override void OnInit(int UID, string _tableName = "SkillTable")
        {
            tableName = _tableName;
            this.ItemName = "skill" + UID.ToString();
            base.OnInit(UID);
        }

        #region 字段信息

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("唯一技能ID")]
        public int Id;

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("技能名字(策划用)")]
        public string NameDesign;

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("描述")]
        public string Desc;

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("技能名字")]
        public string Name;

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("使用新流程")]
        public bool UseNewSystem;

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("是否沿用端游翻译")]
        public bool IsIP;

        [FoldoutGroup("基本属性", expanded: true)]
        [ShowInInspector, LabelText("技能标识ID")]
        [PropertyTooltip("通过buff换技能用（不同等级ID相同）")]
        public int SkillUuid;

        [FoldoutGroup("技能特效", expanded: true)]
        [ShowInInspector, LabelText("技能ID数组")]
        [PropertyTooltip("每个等级一个ID,主动读SkillEffectTable,被动读SkillPassiveTable")]
        [ListDrawerSettings(DraggableItems = false)]
        public int[] EffectIDs;

        [HideInInspector]
        public enum TargetTypeEnum
        {
            [LabelText("被动")]
            Passive = -1,     // 被动
            [LabelText("所有")]
            All = 0,          // 所有
            [LabelText("自己")]
            Self = 1,         // 自己
            [LabelText("友方")]
            Friends = 2,      // 友方
            [LabelText("队伍")]
            Team = 3,         // 队伍
            [LabelText("敌人")]
            Enemy = 4,        // 敌人
            [LabelText("死亡的友方")]
            DeadFriend = 5,   // 死亡的友方
            [LabelText("自己的宠物")]
            Pet = 6,          // 自己的宠物
            [LabelText("自己的最终召唤者")]
            FinalSummoner = 7 // 自己的最终召唤者
        }
        [FoldoutGroup("技能类型", expanded: false)]
        [ShowInInspector, LabelText("技能目标类型")]
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

        public enum SkillScriptTypeEnum
        {
            [LabelText("已有的技能")]
            Self = 0,
            [LabelText("宠物的技能")]
            Pet = 1
        }
        [FoldoutGroup("技能类型", expanded: false)]
        [ShowInInspector, LabelText("技能来自")]
        public SkillScriptTypeEnum _SkillScriptType;
        [HideInInspector]
        public int SkillScriptType
        {
            get
            {
                return (int)_SkillScriptType;
            }
            set
            {
                _SkillScriptType = (SkillScriptTypeEnum)value;
            }
        }

        public enum SpecialSkillTypeEnum
        {
            [LabelText("非宠物技能")]
            NonePetSkill = 0,
            [LabelText("宠物普通攻击")]
            PetNormalAttack = 1,
            [LabelText("宠物主动技能")]
            PetPositiveSkill = 2,
            [LabelText("宠物特殊技能")]
            PetSpecialSkill = 3
        }
        [FoldoutGroup("技能类型", expanded: false)]
        [ShowInInspector, LabelText("宠物技能类型")]
        public SpecialSkillTypeEnum _SpecialSkillType;
        [HideInInspector]
        public int SpecialSkillType
        {
            get
            {
                return (int)_SpecialSkillType;
            }
            set
            {
                _SpecialSkillType = (SpecialSkillTypeEnum)value;
            }
        }

        [HideInInspector]
        public enum SkillTargetRangeTypeEnum
        {
            [LabelText("被动")]
            Passive = -1,   // 被动
            [LabelText("目标")]
            Target = 0,     // 目标
            [LabelText("方向")]
            Direction = 1,  // 方向
            [LabelText("坐标")]
            Coordinate = 2, // 坐标
            [LabelText("自己")]
            Self = 3        // 自己
        }
        [FoldoutGroup("技能类型", expanded: false)]
        [ShowInInspector, LabelText("技能目标范围类型")]
        public SkillTargetRangeTypeEnum _SkillTargetRangeType;
        [HideInInspector]
        public int SkillTargetRangeType
        {
            get
            {
                return (int)_SkillTargetRangeType;
            }
            set
            {
                _SkillTargetRangeType = (SkillTargetRangeTypeEnum)value;
            }
        }

        [HideInInspector]
        public enum SkillRangeTypeEnum
        {
            [LabelText("近战")]
            Melee = 0,   // 近战
            [LabelText("远程")]
            Remote = 1,  // 远程
            [LabelText("随释放时距离目标的距离而定")]
            Distance = 2 // 随释放时距离目标的距离而定 大于等于4米远程  小于4米近战 距离global表配置
        }

        [FoldoutGroup("技能类型", expanded: false)]
        [ShowInInspector, LabelText("技能射程类型")]
        [PropertyTooltip("随释放时距离目标的距离而定(大于等于4米远程,小于4米近战(距离global表配置)")]
        public SkillRangeTypeEnum _SkillRangeType;
        [HideInInspector]
        public int SkillRangeType
        {
            get
            {
                return (int)_SkillRangeType;
            }
            set
            {
                _SkillRangeType = (SkillRangeTypeEnum)value;
            }
        }

        [HideInInspector]
        public enum SkillHatedTypeEnum
        {
            [LabelText("被动")]
            Passive = -1,    // 被动
            [LabelText("伤害")]
            Impair = 1,      // 伤害
            [LabelText("支援")]
            Backup = 2,      // 支援
            [LabelText("嘲讽类")]
            Scorned = 3,     // 嘲讽类
            [LabelText("伤害+支援类")]
            ImpairBackup = 4 // 伤害+支援类
        }
        [FoldoutGroup("技能类型", expanded: false)]
        [ShowInInspector, LabelText("技能仇恨类型")]
        public SkillHatedTypeEnum _SkillHatedType;
        [HideInInspector]
        public int SkillHatedType
        {
            get
            {
                return (int)_SkillHatedType;
            }
            set
            {
                _SkillHatedType = (SkillHatedTypeEnum)value;
            }
        }

        [HideInInspector]
        public enum SkillTypeEnum
        {
            [LabelText("普通技能")]
            Common = 0,           // 普通技能
            [LabelText("偷窃类技能")]
            Steal = 1,            // 偷窃类技
            [LabelText("怪物互击")]
            MonsterFistfight = 2, // 怪物互击
            [LabelText("位移类技能")]
            Shift = 3,            // 位移类技能
            [LabelText("解控类技能")]
            Control = 4,          // 解控类技能
            [LabelText("骑乘类技能")]
            Ride = 5              // 骑乘类技能
        }
        [FoldoutGroup("技能类型", expanded: false)]
        [ShowInInspector, LabelText("技能类型")]
        public SkillTypeEnum _SkillType;
        [HideInInspector]
        public int SkillType
        {
            get
            {
                return (int)_SkillType;
            }
            set
            {
                _SkillType = (SkillTypeEnum)value;
            }
        }

        [HideInInspector]
        public enum SkillDamTypeEnum
        {
            [LabelText("被动")]
            Passive = -1,       // 被动
            [LabelText("普攻")]
            Attack = 0,         // 普攻
            [LabelText("物理")]
            PhysicalAttack = 1, // 物理
            [LabelText("魔法")]
            MagicalAttack = 2,  // 魔法
            [LabelText("混合")]
            MixedAttack = 3,    // 混合
        }
        [FoldoutGroup("技能类型", expanded: false)]
        [ShowInInspector, LabelText("技能伤害类型")]
        public SkillDamTypeEnum _SkillDamType;
        [HideInInspector]
        public int SkillDamType
        {
            get
            {
                return (int)_SkillDamType;
            }
            set
            {
                _SkillDamType = (SkillDamTypeEnum)value;
            }
        }

        [HideInInspector]
        public enum IsAffectedASPDEnum
        {
            [LabelText("被动")]
            Passive = -1,     // 被动
            [LabelText("不被影响")]
            NoInfluenced = 0, // 不被影响
            [LabelText("被影响")]
            Influenced = 1    // 被影响
        }
        [FoldoutGroup("技能技能状态", expanded: false)]
        [ShowInInspector, LabelText("是否被ASPD影响")]
        public IsAffectedASPDEnum _IsAffectedASPD;
        [HideInInspector]
        public int IsAffectedASPD
        {
            get
            {
                return (int)_IsAffectedASPD;
            }
            set
            {
                _IsAffectedASPD = (IsAffectedASPDEnum)value;
            }
        }

        [FoldoutGroup("技能状态", expanded: false)]
        [ShowInInspector, LabelText("ASPD影响百分比")]
        public float ASPDPercent;

        [FoldoutGroup("技能技能状态", expanded: false)]
        [ShowInInspector, LabelText("技能产生的共CD是否受到ASPD加成")]
        public bool IsAffectedASPDExtra;

        [FoldoutGroup("技能类型", expanded: false)]
        [ShowInInspector, LabelText("是否AOE技能")]
        public bool IsAoe;

        [HideInInspector]
        public enum PushInterruptEnum
        {
            [LabelText("不可被击退")]
            CannotRepelled = 0,          // 不可被击退
            [LabelText("可以被击退但不打断")]
            RepelledCannotInterrupt = 1, // 可以被击退但不打断
            [LabelText("可以被击退并且打断技能状态")]
            RepelledInterruptible = 2    // 可以被击退并且打断技能状态
        }
        [FoldoutGroup("技能状态", expanded: false)]
        [ShowInInspector, LabelText("技能状态被打断配置")]
        public PushInterruptEnum _PushInterupt;
        [HideInInspector]
        public int PushInterupt
        {
            get
            {
                return (int)_PushInterupt;
            }
            set
            {
                _PushInterupt = (PushInterruptEnum)value;
            }
        }

        [FoldoutGroup("技能状态", expanded: false)]
        [ShowInInspector, LabelText("技能属性")]
        [PropertyTooltip("被动技能填-1")]
        public int SkillAttr;

        [FoldoutGroup("技能状态", expanded: false)]
        [ShowInInspector, LabelText("是否可以击飞")]
        public bool IsKnockBack;

        [FoldoutGroup("释放限制", expanded: true)]
        [ShowInInspector, LabelText("技能失效范围")]
        [PropertyTooltip("填0时默认为20")]
        public int Length;

        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("是否受射程属性影响")]
        public bool IsRangeAttrImpact;

        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("是否和武器比较射程")]
        public bool IsRangeWeaponAttrImpact;

        [FoldoutGroup("释放限制", expanded: false)]
        [ShowInInspector, LabelText("武器限制")]
        [PropertyTooltip("不填无限制，填则走EquipWeaponTable定义的值，可多填")]
        public int[] WeaponTypeLimit;

        [FoldoutGroup("释放限制", expanded: true)]
        [ShowInInspector, LabelText("副手武器限制")]
        [PropertyTooltip("不填无限制，填则走EquipWeaponTable定义的值，可多填")]
        public int[] SecondWeaponTypeLimit;

        [FoldoutGroup("释放限制", expanded: true)]
        [ShowInInspector, LabelText("技能同时存在个数上限")]
        [PropertyTooltip("无则不填")]
        public string Limits;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("是否隐藏CD")]
        public bool IsDisableCD;

        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("额外的技能ID数组")]
        [PropertyTooltip("每个等级一个ID")]
        public int[] EffectExIDs;

        [HideInInspector]
        public MSeq<int> SkillPanelPos;
        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("技能位置")]
        public List<int> _SkillPanelPos = new List<int>();

        [FoldoutGroup("释放限制", expanded: false)]
        [ShowInInspector, LabelText("特殊前置条件")]
        [PropertyTooltip("0任务")]
        public int[][] SpePreSkillRequired;

        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("释放后是否自动普攻")]
        public bool LinkAutoAtack;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("释放技能是否改变目标颜色")]
        public bool IsChangeColor;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("释放技能是否改变目标颜色")]
        [PropertyTooltip("填写模板名则改变，不填写则不变")]
        public string ChangeColor;

        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("可强制打断的技能ID")]
        [PropertyTooltip("可配置多个")]
        public int[] InteruptSkill;

        [HideInInspector]
        public MSeq<int> IsUseArrow;
        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("是否使用箭矢及消耗数量")]
        public List<int> _IsUseArrow = new List<int>();

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("额外的图标")]
        [PropertyTooltip("当额外技能是独立技能时有用")]
        public string IconEx;

        [FoldoutGroup("释放限制", expanded: false)]
        [ShowInInspector, LabelText("指定场景ID不可释放")]
        public int[] LimitScene;

        [FoldoutGroup("释放限制", expanded: true)]
        [ShowInInspector, LabelText("指定场景类型不可释放")]
        public int[] LimitSceneType;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("是否需要显示施法范围")]
        public bool NeedEffectRange;

        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("额外技能是否为独立的技能")]
        public bool IsEffectExStandalone;

        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("是否为被动技能")]
        public bool IsPassive;

        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("自动战斗释放优先级")]
        [PropertyTooltip("默认为0，数字越大优先级越高")]
        public int AutoBattlePRI;

        [FoldoutGroup("释放限制", expanded: false)]
        [ShowInInspector, LabelText("技能能量")]
        [PropertyTooltip("填SkillEffectTable表中对应技能的不同等级的最大值，没有则不填")]
        public int[] SkillEffectElementEnergy;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("能量不足时的提示")]
        [PropertyTooltip("只有能量类技能才会显示，对应ChineseTable字段")]
        public string EnergyTips;

        [HideInInspector]
        public enum AnimationTypeEnum
        {
            [LabelText("被动技能")]
            Passive = -1, // 被动技能
            [LabelText("通用")]
            Common = 0,   // 通用
            [LabelText("职业")]
            Career = 1    // 职业
        }
        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("动作文件夹")]
        public AnimationTypeEnum _AnimationType;
        [HideInInspector]
        public int AnimationType
        {
            get
            {
                return (int)_AnimationType;
            }
            set
            {
                _AnimationType = (AnimationTypeEnum)value;
            }
        }

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("动作播放顺序")]
        [PropertyTooltip("等于-1随机 等于0按序 大于0按照配置顺序的序号播放")]
        public int SingingAnimation;

        [Serializable]
        public class Seq
        {
            [ShowInInspector]
            public int a = 0;
            [ShowInInspector]
            public int b = 0;
            [ShowInInspector]
            public int c = 0;
        }
        [HideInInspector]
        public MSeq<int>[] PreSkillRequired;
        [FoldoutGroup("释放限制", expanded: false)]
        [ShowInInspector, LabelText("前置技能和等级")]
        [PropertyTooltip("id=lv,id=lv")]
        public List<Seq> _PreSkillRequired = new List<Seq>();


        [HideInInspector]
        public enum SkillTypeIconEnum
        {
            [LabelText("物理")]
            Physical = 0, // 物理
            [LabelText("法术")]
            Magical = 1,  // 法术
            [LabelText("支援")]
            Backup = 2,   // 支援
            [LabelText("被动")]
            Passive = 3,  // 被动
        }
        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("技能类别图标")]
        public SkillTypeIconEnum _SkillTypeIcon;
        [HideInInspector]
        public int SkillTypeIcon
        {
            get
            {
                return (int)_SkillTypeIcon;
            }
            set
            {
                _SkillTypeIcon = (SkillTypeIconEnum)value;
            }
        }

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("图集名字")]
        public string Atlas;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("图标")]
        public string Icon;

        [FoldoutGroup("配置加载", expanded: false)]
        [ShowInInspector, LabelText("技能脚本路径")]
        [PropertyTooltip("人的填，怪的不填")]
        public string SkillDataPath;

        [FoldoutGroup("配置加载", expanded: false)]
        [ShowInInspector, LabelText("技能自动战斗AI")]
        [PropertyTooltip("人的填，怪的不填。要自动放的填，不自动放的不填")]
        public string AITreeName;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("技能描述")]
        public string SkillDesc;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("技能额外描述")]
        public string SkillDescAdd;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("播放器用技能简述")]
        public string SimpleSkillDesc;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("卡片主动技能描述")]
        public string ActiveCardDesc;

        [FoldoutGroup("前端显示用", expanded: false)]
        [ShowInInspector, LabelText("无法拖动到UI快捷栏")]
        [PropertyTooltip("能显示在技能面板，但是无法拖动到UI快捷栏 0表示不做处理 1表示无法拖动进UI 默认是0")]
        public int AllowUI;

        [FoldoutGroup("杂项", expanded: false)]
        [ShowInInspector, LabelText("本地化区域划分")]
        [PropertyTooltip("0所有 1中 2日 4香港 8韩 16北美，多地区时数字叠加，例：日韩都上就配4 + 8 = 12")]
        public string LocalizationArea;

        #endregion

        protected override void OnLoadCSV()
        {
            base.OnLoadCSV();

            #region 特殊数据类型处理
            if (SkillPanelPos != null)
            {
                _SkillPanelPos.Clear();
                for (var i = 0; i < SkillPanelPos.Capacity; i++)
                {
                    _SkillPanelPos.Add(SkillPanelPos[i]);
                }
            }

            if (IsUseArrow != null)
            {
                _IsUseArrow.Clear();
                for (var i = 0; i < IsUseArrow.Capacity; i++)
                {
                    _IsUseArrow.Add(IsUseArrow[i]);
                }
            }

            if (PreSkillRequired != null)
            {
                _PreSkillRequired.Clear();
                foreach (var item in PreSkillRequired)
                {
                    _PreSkillRequired.Add(new Seq { a = item[0], b = item[1], c = item[2] });
                }
            }
            
            #endregion
        }

        protected override void OnSaveCSV()
        {
            #region 特殊数据类型处理
            if (_SkillPanelPos.Count != 0)
            {
                SkillPanelPos = new MSeq<int>(2);
            }
            if (SkillPanelPos != null)
            {
                for (var i = 0; i < _SkillPanelPos.Count; i++)
                {
                    SkillPanelPos[i] = _SkillPanelPos[i];
                }
            }

            if (_IsUseArrow.Count != 0)
            {
                IsUseArrow = new MSeq<int>(2);
            }
            if (IsUseArrow != null)
            {
                for (var i = 0; i < _IsUseArrow.Count; i++)
                {
                    IsUseArrow[i] = _IsUseArrow[i];
                }
            }

            if (_PreSkillRequired.Count != 0)
            {
                PreSkillRequired = new MSeq<int>[_PreSkillRequired.Count];
                for (var i = 0; i < _PreSkillRequired.Count; i++)
                {
                    PreSkillRequired[i] = new MSeq<int>(3);
                }
            }
            if (PreSkillRequired != null)
            {
                for (var i = 0; i < _PreSkillRequired.Count; i++)
                {
                    PreSkillRequired[i][0] = _PreSkillRequired[i].a;
                    PreSkillRequired[i][1] = _PreSkillRequired[i].b;
                    PreSkillRequired[i][2] = _PreSkillRequired[i].c;
                }
            }
            
            #endregion

            base.OnSaveCSV();
        }

        #region 保存&加载
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

        [ButtonGroup]
        [Button("快速进入游戏", ButtonSizes.Medium), GUIColor(151 / 255.0f, 219 / 255.0f, 174 / 255.0f)]
        private void QuickEnterGame()
        {
            QuickEnterMgr.QuickSkillTest(Id);
            RPGEditorWindow.MainWindow.Close();
        }


        #endregion

        public override void CreateTrigger()
        {
            if (this.trigger == null)
            {
                string defineApi = "custom_skill";
                this.CreateTrigger(defineApi);
            }
        }
    }
}