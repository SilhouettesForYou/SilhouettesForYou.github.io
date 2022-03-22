using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Sirenix.OdinInspector.Demos.RPGEditor;
using ToolLib.CSV;
using UnityEngine;
using PbLocal.Xml;
using Com.JoyEntertainment.RO;
using MoonCommonLib;
using ToolLib.Excel;
using LitJson;
using Serializer;

namespace CSVEditor
{

    [Serializable]
    public class SkillEffectShowData : CustomTriggerData
    {
        void OnEnable()
        {
            _type = eCustomItemObjectType.SkillEffect;
            triggerFoldPath = GlobalConfig.skill_effect_trigger_dir;
        }
        public override void OnInit(int UID, string _csvName = "SkillEffectTable")
        {
            this.ItemName = "skillEffect" + UID.ToString();
            base.OnInit(UID);
        }

        #region 字段信息
        [ShowInInspector, LabelText("编号"), PropertyTooltip(""), LabelWidth(150), HorizontalGroup("37b8c0")]
        public int Id;

        [ShowInInspector, LabelText("等级"), PropertyTooltip(""), LabelWidth(150), HorizontalGroup("37b8c0")]
        public int Level;

        [ShowInInspector, LabelText("技能名字"), PropertyTooltip(""), LabelWidth(150), HorizontalGroup("763bc4")]
        public string Name = string.Empty;

        [ShowInInspector, LabelText("技能备注"), PropertyTooltip(""), LabelWidth(150), HorizontalGroup("763bc4")]
        public string Skill_Text = string.Empty;

        [ShowInInspector, LabelText("对应skilltable的技能id"), PropertyTooltip(""), LabelWidth(150), FoldoutGroup("基础设置", true), HorizontalGroup("基础设置/fa04c9")]
        [Com.JoyEntertainment.W3.SelectResourceInlineButton(typeof(SkillShowData), "Id", null, "", "选择关联资源")]
        public int SkillID;

        [ShowInInspector, LabelText("技能效果编号"), PropertyTooltip("（策划用）同一个技能的多个效果"), LabelWidth(150), FoldoutGroup("基础设置", true), HorizontalGroup("基础设置/fa04c9")]
        public string SkillEffectNum = string.Empty;

        [ShowInInspector, LabelText("技能脚本前缀"), PropertyTooltip("技能脚本前缀"), LabelWidth(150), FoldoutGroup("基础设置", true), HorizontalGroup("基础设置/23d1de")]
        public string SkillScriptPrefix = string.Empty;

        [ShowInInspector, LabelText("技能脚本后缀"), PropertyTooltip("（武器类型）"), LabelWidth(150), FoldoutGroup("基础设置", true), HorizontalGroup("基础设置/23d1de")]
        public string[] SkillScriptPostfix;

        [ShowInInspector, LabelText("技能伤害计算脚本"), PropertyTooltip(""), LabelWidth(150), FoldoutGroup("技能效果", true), HorizontalGroup("技能效果/ca4d54")]
        public string DamageScript = string.Empty;
                
        [HideInInspector]
        public bool BreakHiding;
        public enum BreakHidingValue
        {
            [LabelText("0 不打破——不打破")]
            Unmounted = 0,
            [LabelText("1 打破——打破")]
            Mounted = 1,
        };
        [ShowInInspector, LabelText("是否打破隐匿"), PropertyTooltip("（0不打破，1打破，默认1）"), LabelWidth(150), FoldoutGroup("技能效果", true), HorizontalGroup("技能效果/ca4d54")]
        public BreakHidingValue _BreakHiding;

        [ShowInInspector, LabelText("每个result目标数量上限"), PropertyTooltip("【真子弹的result这里必须填为1】"), LabelWidth(150), FoldoutGroup("技能效果", true)]
        public int[] AttackCount;

        [Serializable]
        public class SeqCreateBuff
        {
            [ShowInInspector]
            public int a;
            [ShowInInspector]
            public int b;
        }
        [HideInInspector]
        public MSeq<int>[] CreateBuff;
        [ShowInInspector]
        [LabelText("产生Buff")]
        [PropertyTooltip("(ID=概率)加buff在技能伤害结束后")]
        [LabelWidth(150)]
        [FoldoutGroup("技能效果", true)]
        public List<SeqCreateBuff> _CreateBuff = new List<SeqCreateBuff>();

        [ShowInInspector]
        [LabelText("PVE伤害系数")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("技能效果", true)]
        public float[] PVEDamageRadio;

        [ShowInInspector]
        [LabelText("PVE固定伤害")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("技能效果", true)]
        public float[] PVEStaticDam;

        [ShowInInspector]
        [LabelText("PVP伤害系数")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("技能效果", true)]
        public float[] PVPDamageRadio;

        [ShowInInspector]
        [LabelText("PVP固定伤害")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("技能效果", true)]
        public float[] PVPStaticDam;

        [ShowInInspector]
        [LabelText("PVE可变吟唱时间")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/7d01d6")]
        public float PVEFloatSingingTime;

        [ShowInInspector]
        [LabelText("PVP可变吟唱时间")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/7d01d6")]
        public float PVPFloatSingingTime;

        [ShowInInspector]
        [LabelText("PVE固定吟唱时间")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/b3634e")]
        public float PVEFixSingingTime;

        [ShowInInspector]
        [LabelText("PVP固定吟唱时间")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/b3634e")]
        public float PVPFixSingingTime;

        [ShowInInspector]
        [LabelText("PVE冷却时间")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/3d2c46")]
        public float PVECoolTime;

        [ShowInInspector]
        [LabelText("PVP冷却时间")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/3d2c46")]
        public float PVPCoolTime;

        [ShowInInspector]
        [LabelText("PVE公共冷却时间组")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/b38bbd")]
        public int PVEGroupCoolTimeType;

        [ShowInInspector]
        [LabelText("PVP公共冷却时间组")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/b38bbd")]
        public int PVPGroupCoolTimeType;

        [ShowInInspector]
        [LabelText("PVE技能产生公共冷却时间")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/ff5f5a")]
        public float PVEGroupCoolTime;

        [ShowInInspector]
        [LabelText("PVP技能产生公共冷却时间")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("吟唱冷却", true)]
        [HorizontalGroup("吟唱冷却/ff5f5a")]
        public float PVPGroupCoolTime;

        [ShowInInspector]
        [LabelText("最小施法范围")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("目标仇恨", true)]
        [HorizontalGroup("目标仇恨/22cdf9")]
        public float LowRange;

        [ShowInInspector]
        [LabelText("最大施法范围")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("目标仇恨", true)]
        [HorizontalGroup("目标仇恨/22cdf9")]
        public float FarRange;

        [ShowInInspector]
        [LabelText("对目标造成的仇恨值")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("目标仇恨", true)]
        [HorizontalGroup("目标仇恨/f5c475")]
        public int[] HatredToTarget;

        [ShowInInspector]
        [LabelText("对目标造成的仇恨百分比")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("目标仇恨", true)]
        [HorizontalGroup("目标仇恨/f5c475")]
        public int[] HatredPercentToTarget;

        [ShowInInspector]
        [LabelText("目标对我的仇恨值")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("目标仇恨", true)]
        [HorizontalGroup("目标仇恨/fd29b0")]
        public int[] HatredToMe;

        [ShowInInspector]
        [LabelText("目标对我的仇恨值百分比")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("目标仇恨", true)]
        [HorizontalGroup("目标仇恨/fd29b0")]
        public int[] HatredPercentToMe;

        [ShowInInspector]
        [LabelText("仇恨系数")]
        [PropertyTooltip("(默认为1）")]
        [LabelWidth(150)]
        [FoldoutGroup("目标仇恨", true)]
        public float[] Hatredcoefficient;

        [HideInInspector]
        public MSeq<int> SkillCost;
        [ShowInInspector]
        [LabelText("基础消耗")]
        [PropertyTooltip("(属性ID=Value，不填只走CD)")]
        [LabelWidth(150)]
        [FoldoutGroup("释放消耗", true)]
        public List<int> _SkillCost = new List<int>();

        [ShowInInspector]
        [LabelText("技能释放所需能量")]
        [PropertyTooltip("（字段值ElementAttr|Value）")]
        [LabelWidth(150)]
        [FoldoutGroup("释放消耗", true)]
        public int[] SkillElementEnergy;

        [ShowInInspector]
        [LabelText("技能施放判定")]
        [PropertyTooltip("（Lua）")]
        [LabelWidth(150)]
        [FoldoutGroup("释放消耗", true)]
        public string ConfirmScript;

        [ShowInInspector]
        [LabelText("特殊消耗")]
        [PropertyTooltip("（LUA）")]
        [LabelWidth(150)]
        [FoldoutGroup("释放消耗", true)]
        public string CostScript;

        [ShowInInspector]
        [LabelText("特殊消耗描述")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("释放消耗", true)]
        public string CostDesc;

        [ShowInInspector]
        [LabelText("消耗气弹个数")]
        [PropertyTooltip("（浮动时填0）")]
        [LabelWidth(150)]
        [FoldoutGroup("释放消耗", true)]
        [HorizontalGroup("释放消耗/6afc24")]
        public int CostSpiritSpheres;

        [ShowInInspector]
        [LabelText("检查lua置灰按钮")]
        [PropertyTooltip("（0为不需要，1为需要）")]
        [LabelWidth(150)]
        [FoldoutGroup("释放消耗", true)]
        [HorizontalGroup("释放消耗/6afc24")]
        public bool AshByLua;

        public enum BattleVehicleValue
        {
            [LabelText("0 只有马下技能——只有马下技能")]
            Unmounted = 0,
            [LabelText("1 只有马上技能——只有马上技能")]
            Mounted = 1,
            [LabelText("2 两者都有——两者都有")]
            Both = 2,
        }
        [ShowInInspector]
        [LabelText("载具战斗判断")]
        [PropertyTooltip("（0为只有马下技能，1为只有马上技能，2为2者都有）")]
        [LabelWidth(150)]
        [FoldoutGroup("战斗预警", true)]
        [HorizontalGroup("战斗预警/b4c935")]
        public BattleVehicleValue _BattleVehicle;
        [HideInInspector]
        public int BattleVehicle
        {
            get
            {
                return (int)_BattleVehicle;
            }
            set
            {
                _BattleVehicle = (BattleVehicleValue)value;
            }
        }

        public enum EffectShapeValue
        {
            [LabelText("0 无——默认")]
            None = 0,
            [LabelText("1 圆形——圆形")]
            Circle = 1,
            [LabelText("2 矩形——矩形")]
            Rectangle = 2,
            [LabelText("3 扇形——扇形")]
            Fan = 3,
            [LabelText("4 箭头——箭头")]
            Arrows = 4,
            [LabelText("5 箭头带宽度——箭头带宽度")]
            ArrowsWithLength = 5,
        }

        [ShowInInspector]
        [LabelText("作用范围形状")]
        [PropertyTooltip("（0.无，1.圆形，2. 矩形，3.扇形，4.箭头，5箭头带宽度）")]
        [LabelWidth(150)]
        [FoldoutGroup("战斗预警", true)]
        [HorizontalGroup("战斗预警/b4c935")]
        public EffectShapeValue _EffectShape;
        [HideInInspector]
        public int EffectShape
        {
            get
            {
                return (int)_EffectShape;
            }
            set
            {
                _EffectShape = (EffectShapeValue)value;
            }
        }

        [ShowInInspector]
        [LabelText("作用范围参数")]
        [PropertyTooltip("（圆形为半径，矩形为长宽，扇形为角度|半径，新箭头为长宽）")]
        [LabelWidth(150)]
        [FoldoutGroup("战斗预警", true)]
        public float[] EffectRange;

        [ShowInInspector]
        [LabelText("技能描述替换")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("特殊设置", true)]
        [HorizontalGroup("特殊设置/477580")]
        public string[] SkillDescRep;

        [Serializable]
        public class SeqCreateGroupBuff
        {
            [ShowInInspector]
            public int a;
            [ShowInInspector]
            public int b;
        }
        [HideInInspector]
        public MSeq<int>[] CreateGroupBuff;
        [ShowInInspector]
        [LabelText("链接型特效")]
        [PropertyTooltip("填buffID")]
        [LabelWidth(150)]
        [FoldoutGroup("特殊设置", true)]
        [HorizontalGroup("特殊设置/477580")]
        public List<SeqCreateGroupBuff> _CreateGroupBuff = new List<SeqCreateGroupBuff>();

        [ShowInInspector]
        [LabelText("技能时机触发调用lua")]
        [PropertyTooltip("只填写Lua名")]
        [LabelWidth(150)]
        [FoldoutGroup("特殊设置", true)]
        [HorizontalGroup("特殊设置/477580")]
        public int SkillTriggerID;

        [ShowInInspector]
        [LabelText("配置格式为key1=value1=key2=value2=....=keyn=valuen=.....")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("特殊设置", true)]
        public string PVEConfigData = string.Empty;

        [ShowInInspector]
        [LabelText("配置格式为key1=value1=key2=value2=....=keyn=valuen=.....")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("特殊设置", true)]
        public string PVPConfigData = string.Empty;

        [ShowInInspector]
        [LabelText("程序用，不用填值，保持在最后一列")]
        [PropertyTooltip("")]
        [LabelWidth(150)]
        [FoldoutGroup("特殊设置", true)]
        public string[] ServerUUIDData;

        #endregion

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
        #endregion

        #region 序列化&反序列化&其他生命周期
        public override void CreateTrigger()
        {
            
        }

        protected override void OnLoadCSV()
        {
            base.OnLoadCSV();

            #region 特殊数据类型处理
            if (CreateBuff != null)
            {
                foreach (var item in CreateBuff)
                {
                    _CreateBuff.Add(new SeqCreateBuff { a = item[0], b = item[1] });
                }
            }
            if (SkillCost != null)
            {
                for (var i = 0; i < SkillCost.Capacity; i++)
                {
                    _SkillCost.Add(SkillCost[i]);
                }
            }
            
            if (CreateGroupBuff != null)
            {
                foreach (var item in CreateGroupBuff)
                {
                    _CreateGroupBuff.Add(new SeqCreateGroupBuff { a = item[0], b = item[1] });
                }
            }
            #endregion
        }

        protected override void OnSaveCSV()
        {
            #region 特殊数据类型处理
            if (_CreateBuff.Count != 0)
            {
                CreateBuff = new MSeq<int>[_CreateBuff.Count];
                for (var i = 0; i < _CreateBuff.Count; i++)
                {
                    CreateBuff[i] = new MSeq<int>(2);
                }
            }
            if (CreateBuff != null)
            {
                for (var i = 0; i < _CreateBuff.Count; i++)
                {
                    CreateBuff[i][0] = _CreateBuff[i].a;
                    CreateBuff[i][1] = _CreateBuff[i].b;
                }
            }

            if (_SkillCost.Count != 0)
            {
                SkillCost = new MSeq<int>(2);
            }
            if (SkillCost != null)
            {
                for (var i = 0; i < _SkillCost.Count; i++)
                {
                    SkillCost[i] = _SkillCost[i];
                }
            }

            if (_CreateGroupBuff.Count != 0)
            {
                CreateGroupBuff = new MSeq<int>[_CreateGroupBuff.Count];
                for (var i = 0; i < _CreateGroupBuff.Count; i++)
                {
                    CreateGroupBuff[i] = new MSeq<int>(2);
                }
            }
            if (CreateGroupBuff != null)
            {
                for (var i = 0; i < _CreateGroupBuff.Count; i++)
                {
                    CreateGroupBuff[i][0] = _CreateGroupBuff[i].a;
                    CreateGroupBuff[i][1] = _CreateGroupBuff[i].b;
                }
            }

            #endregion

            base.OnSaveCSV();
        }

        #endregion
    }
}
