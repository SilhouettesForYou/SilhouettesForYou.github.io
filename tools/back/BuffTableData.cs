
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector.Demos.RPGEditor;
using ToolLib.CSV;
using UnityEditor;
using UnityEngine;
using CSVEditor;
using MoonCommonLib;

namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    [Serializable]
    public class NormalFx
    {
        [HideInInspector]
        public string LableName;
        [HideInInspector]
        public bool IsShowStreamer = false;

        [LabelText("特效ID"), HorizontalGroup("group"), LabelWidth(40)]
        public int FxID;

        [ShowIfGroup("IsShowStreamer")]
        [LabelText("流光特效预警时间"), HorizontalGroup("IsShowStreamer/Group base"), LabelWidth(104)]
        [SuffixLabel("秒", Overlay = true)]
        public float FxWarningTime;
        [LabelText("特效参数1"), HorizontalGroup("IsShowStreamer/Group base"), LabelWidth(79)]
        [PropertyTooltip("矩形：长|圆形：外半径")]
        public float FxParam1;
        [LabelText("特效参数2"), HorizontalGroup("IsShowStreamer/Group base"), LabelWidth(79)]
        [PropertyTooltip("矩形：宽|圆形：内半径")]
        public float FxParam2;
        [LabelText("特效参数3"), HorizontalGroup("IsShowStreamer/Group base"), LabelWidth(79)]
        [PropertyTooltip("矩形：无需填写|圆形：角度")]
        public float FxParam3;

        [LabelText("特效挂点"), HorizontalGroup("Group base1"), LabelWidth(52)]
        [PropertyTooltip("注意：特效挂点跟包围盒挂点只能选其一，而且多特效必须要统一！")]
        public string FxPoint;
        [MinValue(0)]
        [MaxValue(1)]
        [LabelText("包围盒挂点"), HorizontalGroup("Group base1"), LabelWidth(65)]
        [EnableIf("@string.IsNullOrEmpty(this.FxPoint)")]
        public float FxPointHightPercent;
    }
    public class LevelFx
    {
        [HideInInspector]
        public string level;
        [ShowInInspector, LabelText("@this.level + \"特效列表\""), LabelWidth(85)]
        [ListDrawerSettings(CustomAddFunction = "AddNormalFx", OnBeginListElementGUI = "BeginDrawNormalEffectsElement", ListElementLabelName = "LableName")]
        [HideReferenceObjectPicker]
        public List<NormalFx> normalFxs = new List<NormalFx>();
        private NormalFx AddNormalFx()
        {
            return new NormalFx();
        }
        public void BeginDrawNormalEffectsElement(int index)
        {
            normalFxs[index].IsShowStreamer = (level == "第1层" && CommonTool.GetCellValue("EffectTable", normalFxs[index].FxID.ToString(), "UseVector3OneScale") == "1");
            normalFxs[index].LableName = "特效" + index;
        }
    }
    [Serializable]
    public class BuffShowData : CustomTriggerData
    {
        void OnEnable()
        {
            _type = eCustomItemObjectType.Buff;
            triggerFoldPath = GlobalConfig.buff_trigger_dir;
        }

        public void OnInit(int UID, string _csvName = "BuffTable")
        {
            tableNamePrefix = "BuffTable/";
            tableName = "BuffTable";
            this.ItemName = "buff" + UID.ToString();
            base.OnInit(UID, _csvName);
        }
        #region 字段信息
        #region buff基础信息
        [ShowInInspector, LabelText("BuffID"), HorizontalGroup("Group 1"), LabelWidth(42)]
        public int Id;

        [ShowInInspector, LabelText("备注"), HorizontalGroup("Group 1"), LabelWidth(26)]
        public string Name;

        [ShowInInspector, LabelText("Buff程序ID"), HorizontalGroup("Group 2"), LabelWidth(68)]
        public int BuffId;

        [ShowInInspector, LabelText("Buff等级"), HorizontalGroup("Group 2"), LabelWidth(54)]
        public int Level;

        [ShowInInspector, LabelText("游戏内Buff名"), HorizontalGroup("Group 3"), LabelWidth(80)]
        public string InGameName;

        //[ShowInInspector, LabelText("是否沿用端游翻译"), HorizontalGroup("Group 3"), LabelWidth(96)]
        //public bool IsIP
        //{
        //    get
        //    {
        //        return rowData.IsIP == "1";
        //    }
        //    set
        //    {
        //        rowData.IsIP = value ? "1" : "0";
        //    }
        //}

        [ShowInInspector, LabelText("是否在游戏中显示"), HorizontalGroup("Group 3"), LabelWidth(96)]
        [PropertyTooltip("请注意，勾选后必须填写游戏内Buff名、Buff描述【词缀不需要】和Buff图标")]
        public bool IsVisible;

        [ShowInInspector, LabelText("Buff描述"), LabelWidth(54)]
        public string Description;

        //[ShowInInspector, LabelText("词条描述"), LabelWidth(54)]
        //public string TipsForEquip
        //{
        //    get
        //    {
        //        return rowData.TipsForEquip;
        //    }
        //    set
        //    {
        //        rowData.TipsForEquip = value;
        //    }
        //}
        [HideInInspector]
        public string IconAtlas;
        [HideInInspector]
        public string Icon;
        [ShowInInspector, LabelText("Buff图标"), HorizontalGroup("Group 3"), LabelWidth(54)]
        public Sprite _BuffIcon
        {
            get
            {
                if (!string.IsNullOrEmpty(Icon))
                {
                    return UIAtlasReplaceTools.GetSprite(IconAtlas, Icon);
                }
                return null;
            }
            set
            {
                Icon = value.name;
                IconAtlas = value.texture.name;
            }
        }
        //[ShowInInspector, LabelText("本地化区域划分"), LabelWidth(96)]
        //[PropertyTooltip("0所有 1中 2日 4香港 8韩 16北美，多地区时数字叠加，例：日韩都上就配4 + 8 = 12")]
        //public string LocalizationArea
        //{
        //    get
        //    {
        //        return rowData.LocalizationArea;
        //    }
        //    set
        //    {
        //        rowData.LocalizationArea = value.ToString();
        //    }
        //}
        #endregion
        #region 基础设置
        [HideInInspector]
        public MSeq<int> ReplaceRule;
        [HideInInspector]
        public enum ReplaceRuleType
        {
            同源同ID替换 = 1,
            不同源同ID替换 = 2,
            同源同类别替换 = 3,
            不同源同类别替换 = 4,
            堆叠 = 5,
            特殊状态属性替换 = 6,
            特殊状态替换并且自身可以叠加 = 7,
            同源同ID堆叠 = 8,
            不同源同ID堆叠 = 9,
            同源同ID时间叠加规则 = 10,
            无条件叠加 = 11
        }
        ReplaceRuleType _replaceRuleType = ReplaceRuleType.同源同ID替换;
        [TitleGroup("基础设置", boldTitle: true)]
        [ShowInInspector, LabelText("替换规则"), HorizontalGroup("基础设置/Group base1"), LabelWidth(52)]
        public ReplaceRuleType _ReplaceRule
        {
            get
            {
                switch (_replaceRuleType)
                {
                    case ReplaceRuleType.同源同ID替换:
                    case ReplaceRuleType.不同源同ID替换:
                    case ReplaceRuleType.同源同类别替换:
                    case ReplaceRuleType.不同源同类别替换:
                    case ReplaceRuleType.特殊状态属性替换:
                    case ReplaceRuleType.无条件叠加:
                        _isReplaceRuleParamEnable = false;
                        _replaceRuleParamInfo = "固定为0";
                        break;
                    case ReplaceRuleType.堆叠:
                    case ReplaceRuleType.特殊状态替换并且自身可以叠加:
                    case ReplaceRuleType.同源同ID堆叠:
                    case ReplaceRuleType.不同源同ID堆叠:
                    case ReplaceRuleType.同源同ID时间叠加规则:
                        _isReplaceRuleParamEnable = true;
                        _replaceRuleParamInfo = "堆叠层数";
                        break;
                }
                return _replaceRuleType;
            }
            set
            {
                _replaceRuleType = value;
            }
        }
        private void ReplaceRuleParamChange()
        {
            if (_isReplaceRuleParamEnable && _IsChangeByNumber)
            {
                while (Fxs.Count < _replaceRuleParam) Fxs.Add(new LevelFx());
                if (Fxs.Count > _replaceRuleParam)
                {
                    Fxs.RemoveRange(ReplaceRuleParam, Fxs.Count - ReplaceRuleParam);
                }
            }
            else
            {
                if (Fxs.Count > 1)
                {
                    Fxs.RemoveRange(1, Fxs.Count - 1);
                }
            }
        }
        int _replaceRuleParam;
        bool _isReplaceRuleParamEnable;
        string _replaceRuleParamInfo;
        [ShowInInspector, LabelText("$_replaceRuleParamInfo"), HorizontalGroup("基础设置/Group base1"), LabelWidth(52)]
        [EnableIf("@this._isReplaceRuleParamEnable == true")]
        [OnValueChanged("ReplaceRuleParamChange")]
        public int ReplaceRuleParam
        {
            get
            {
                return _replaceRuleParam;
            }
            set
            {
                _replaceRuleParam = value;
            }
        }

        [ShowInInspector, LabelText("Buff类别"), HorizontalGroup("基础设置/Group base1"), LabelWidth(54)]
        [ShowIf("@this._replaceRuleType == ReplaceRuleType.同源同类别替换 || this._replaceRuleType == ReplaceRuleType.不同源同类别替换")]
        public uint Type;

        [HideInInspector]
        public enum BuffEffectType
        {
            正常状态 = 0,
            霸体 = 1,
            无敌 = 2,
            隐匿 = 3,
            连接 = 4,
            变身 = 5,
            buff的firer不跟随Owner = 6,
            A类状态起始值 = 100,
            中毒 = 101,
            流血 = 102,
            灼烧 = 103,
            诅咒 = 104,
            黑暗 = 105,
            沉默 = 106,
            减速_策划在用 = 107,
            真剑百破道状态 = 108,
            A类状态结束值 = 199,
            B类状态起始值 = 200,
            恐惧 = 201,
            定身 = 202,
            睡眠 = 203,
            B类状态结束值 = 299,
            C类状态起始值 = 300,
            冰冻 = 301,
            石化 = 302,
            即将石化 = 303,
            麻痹 = 304,
            C类状态结束值 = 399,
            D类状态起始值 = 400,
            眩晕 = 401,
            D类状态结束值 = 499,
            免疫所有异常状态 = 10000
        }
        [ShowInInspector, LabelText("状态效果"), HorizontalGroup("基础设置/Group base2"), LabelWidth(52)]
        public BuffEffectType _BuffEffect;
        [HideInInspector]
        public int BuffEffect
        {
            get
            {
                return (int)_BuffEffect;
            }
            set
            {
                _BuffEffect = (BuffEffectType)value;
            }
        }

        [HideInInspector]
        public uint TransfigureDeleteType;
        [HideInInspector]
        public enum TransDelType
        {
            变身后直接删除该buff = 0,
            变身时删除该buff = 1,
            变身状态保留该buff = 2
        }
        [ShowInInspector, LabelText("变身删除类型"), HorizontalGroup("基础设置/Group base21"), LabelWidth(78)]
        [PropertyTooltip("变身时删除该buff，变身后还原，变身过程中时间流逝。变身状态时，上该BUFF不会生效")]
        public TransDelType _TransfigureDeleteType
        {
            get
            {
                return (TransDelType)TransfigureDeleteType;
            }
            set
            {
                TransfigureDeleteType = (uint)value;
            }
        }

        [HideInInspector]
        public enum EnumDestroyType
        {
            无 = 0,
            时间到 = 1,
            不吸收溢出伤害的hp = 2,
            生效次数 = 3,
            吸收溢出伤害的hp = 4,
            SkillID按skilltime销毁 = 5
        }

        [HideInInspector]
        public enum StrDestroyParamInfo
        {
            无 = 0,
            持续时间 = 1,
            护盾强度_不吸 = 2,
            生效次数 = 3,
            护盾强度_吸 = 4,
            SkillID = 5
        };

        [Serializable]
        public class SeqDestroyTiming
        {
            [HideInInspector]
            public double DestroyType;
            [ShowInInspector, LabelText("销毁类型"), HorizontalGroup("Group 1"), PropertyOrder(0), LabelWidth(40)]
            public EnumDestroyType _DestroyType
            {
                get
                {
                    return (EnumDestroyType)(int)DestroyType;
                }
                set
                {
                    DestroyParamInfo = ((StrDestroyParamInfo)value).ToString();
                    DestroyType = (double)value;
                }
            }

            [HideInInspector]
            public string DestroyParamInfo;
            [ShowInInspector, LabelText("$DestroyParamInfo"), HorizontalGroup("Group 1"), PropertyOrder(1), LabelWidth(40)]
            [ShowIf("@this._DestroyType != EnumDestroyType.无")]
            public double DestroyParam;
        }
        [HideInInspector]
        public MSeq<double>[] DestroyTiming;
        [ShowInInspector, LabelText("销毁方法"), HorizontalGroup("基础设置/Group base3"), LabelWidth(52)]
        public List<SeqDestroyTiming> _DestroyTiming = new List<SeqDestroyTiming>();

        [ShowInInspector, LabelText("是否减益"), HorizontalGroup("基础设置/Group base4"), LabelWidth(52)]
        public bool IsDebuff;

        [HideInInspector]
        public bool IsDeathClear;
        [ShowInInspector, LabelText("死亡删除"), HorizontalGroup("基础设置/Group base4"), LabelWidth(52)]
        public bool _IsDeathClear
        {
            get
            {
                return !IsDeathClear;
            }
            set
            {
                IsDeathClear = !value;
            }
        }

        [HideInInspector]
        public int IsOffLineClear;
        [ShowInInspector, LabelText("下线删除"), HorizontalGroup("基础设置/Group base4"), LabelWidth(52)]
        public bool _IsOffLineClear
        {
            get
            {
                return IsOffLineClear == 0;
            }
            set
            {
                IsOffLineClear = value ? 0 : 1;
            }
        }

        [ShowInInspector, LabelText("过场景删除"), HorizontalGroup("基础设置/Group base4"), LabelWidth(65)]
        public bool IsChangeMapClear;

        [ShowInInspector, LabelText("可被主动取消"), HorizontalGroup("基础设置/Group base4"), LabelWidth(78)]
        public bool CouldCancel;

        [ShowInInspector, LabelText("可被召唤兽继承"), HorizontalGroup("基础设置/Group base4"), LabelWidth(90)]
        public bool MobCanInherit;
        #endregion
        #region 特效设置
        private LevelFx AddLevelFx()
        {
            return new LevelFx();
        }
        private void BeginDrawlevelEffectsElement(int index)
        {
            if (Fxs[index] == null)
            {
                Fxs[index] = new LevelFx();
            }
            Fxs[index].level = $"第{index + 1}层";
        }
        private NormalFx AddNormalFx()
        {
            return new NormalFx();
        }
        private void BeginDrawBeginFxsElement(int index)
        {
            if (BeginFxs[index] == null)
            {
                BeginFxs[index] = new NormalFx();
            }
            BeginFxs[index].LableName = "特效" + index;
        }
        private void BeginDrawDestoryFxsElement(int index)
        {
            if (DestroyFxs[index] == null)
            {
                DestroyFxs[index] = new NormalFx();
            }
            DestroyFxs[index].LableName = "特效" + index;
        }

        [HideInInspector]
        public int IsChangeByNumber;
        [TitleGroup("特效设置", boldTitle: true)]
        [ShowInInspector, LabelText("是否根据层数改变特效"), LabelWidth(130)]
        [ShowIf("@this._isReplaceRuleParamEnable == true")]
        [OnValueChanged("ReplaceRuleParamChange")]
        public bool _IsChangeByNumber
        {
            get
            {
                return IsChangeByNumber == 1;
            }
            set
            {
                IsChangeByNumber = value ? 1 : 0;
            }
        }

        [HideInInspector]
        public float[] FxWarningTime;
        [HideInInspector]
        public MSeq<float>[] ScaleFac; //vector<Sequence<float, 3>>
        [HideInInspector]
        public string[] FxPoint;
        [HideInInspector]
        public float[] FxPointHightPercent;

        [HideInInspector]
        public int[][] Fx;
        [TitleGroup("特效设置")]
        [ShowInInspector, LabelText("持续特效列表"), LabelWidth(78)]
        [ListDrawerSettings(CustomAddFunction = "AddLevelFx", OnBeginListElementGUI = "BeginDrawlevelEffectsElement", ListElementLabelName = "level")]
        [HideReferenceObjectPicker]
        [OnValueChanged("ReplaceRuleParamChange")]
        public List<LevelFx> Fxs = new List<LevelFx>();

        [HideInInspector]
        public int[] BeginFx;
        [HideInInspector]
        public string[] BeginFxPoint;
        [HideInInspector]
        public float[] BeginFxPointHightPercent;
        [TitleGroup("特效设置")]
        [ShowInInspector, LabelText("开始特效列表"), LabelWidth(78)]
        [ListDrawerSettings(CustomAddFunction = "AddNormalFx", OnEndListElementGUI = "BeginDrawBeginFxsElement", ListElementLabelName = "LableName")]
        [HideReferenceObjectPicker]
        public List<NormalFx> BeginFxs = new List<NormalFx>();

        [HideInInspector]
        public int[] DestroyFx;
        [HideInInspector]
        public string[] DestroyFxPoint;
        [HideInInspector]
        public float[] DestroyFxPointHightPercent;
        [TitleGroup("特效设置")]
        [ShowInInspector, LabelText("销毁特效列表"), LabelWidth(78)]
        [ListDrawerSettings(CustomAddFunction = "AddNormalFx", OnBeginListElementGUI = "BeginDrawDestoryFxsElement", ListElementLabelName = "LableName")]
        [HideReferenceObjectPicker]
        public List<NormalFx> DestroyFxs = new List<NormalFx>();
        #endregion
        #region 音效设置
        [TitleGroup("音效设置", boldTitle: true)]
        [ShowInInspector, LabelText("开始音效"), HorizontalGroup("音效设置/Group base1"), LabelWidth(52)]
        public int BeginAudio;
        [ShowInInspector, LabelText("持续音效"), HorizontalGroup("音效设置/Group base1"), LabelWidth(52)]
        public int Audio;

        [ShowInInspector, LabelText("销毁音效"), HorizontalGroup("音效设置/Group base1"), LabelWidth(52)]
        public int DestroyAudio;
        #endregion
        #region 其他设置
        [HideInInspector]
        public int[] TimePass;

        [HideInInspector]
        public enum TimePassType
        {
            下线不流逝 = 0,
            死亡不流逝 = 1,
            切副本不流逝 = 2
        }
        [TitleGroup("其它设置", boldTitle: true)]
        [ShowInInspector, LabelText("时间流逝类型"), HorizontalGroup("其它设置/Group base1"), LabelWidth(78)]
        public List<TimePassType> _TimePass = new List<TimePassType>();

        
        [ShowInInspector, LabelText("替换buffID"), HorizontalGroup("其它设置/Group base1"), LabelWidth(68)]
        public int ReplaceBuffId;

        [HideInInspector]
        public bool IsRelateBuff;
        [HideInInspector]
        public int RelateBuffId;
        [ShowInInspector, LabelText("关联buffID"), HorizontalGroup("其它设置/Group base1"), LabelWidth(68)]
        public int _RelateBuffId
        {
            get
            {
                return RelateBuffId;
            }
            set
            {
                RelateBuffId = value;
                IsRelateBuff = value > 0;
            }
        }

        [ShowInInspector, LabelText("护盾生效优先级类型"), HorizontalGroup("其它设置/Group base2"), LabelWidth(117)]
        public string BuffPriorityType;
        [ShowInInspector, LabelText("护盾生效优先级"), HorizontalGroup("其它设置/Group base2"), LabelWidth(91)]
        public string BuffPriorityValue;
        [TitleGroup("脚本设置", boldTitle: true)]
        [ShowInInspector, LabelText("是否删除"), LabelWidth(114)]
        public bool IsRemovableGain;

        [ShowInInspector, LabelText("BUFF新系统"), LabelWidth(114)]
        public bool UseNewSystem;
        #endregion
        #endregion


        /// <summary>
        /// 展示数据之前进行的数据处理
        /// </summary>
        /// <returns></returns>
        public void OnLoadCSV()
        {
            base.OnLoadCSV();

            #region 特殊数据类型处理
            //ReplaceRule处理
            if (ReplaceRule.Capacity == 2)
            {
                _replaceRuleType = (ReplaceRuleType)ReplaceRule[0];
                _replaceRuleParam = ReplaceRule[1];
            }
            //DestroyTiming处理
            if (DestroyTiming != null)
            {
                _DestroyTiming.Clear();
                foreach (var item in DestroyTiming)
                {
                    _DestroyTiming.Add(new SeqDestroyTiming { DestroyType = item[0], DestroyParam = item[1] });
                }
            }

            if (TimePass != null)
            {
                _TimePass.Clear();
                for (int i = 0; i < TimePass.Length; i++)
                {
                    _TimePass.Add((TimePassType)TimePass[i]);
                }
            }

            //持续特效
            if (Fx != null)
            {
                //Fx处理
                Fxs.Clear();
                for (int level = 0; level < Fx.Length; ++level)
                {
                    var subSplitList = Fx[level];
                    var levelFx = new LevelFx();
                    levelFx.normalFxs = new List<NormalFx>(Fx[level].Length);
                    for (int i = 0; i < subSplitList.Length; ++i)
                    {
                        var normalFx = new NormalFx();
                        normalFx.FxID = subSplitList[i];
                        levelFx.normalFxs.Add(normalFx);
                    }
                    Fxs.Add(levelFx);
                }
                //FxWarningTime ScaleFac FxPoint FxPointHightPercent处理
                if (Fx.Length > 0)
                {
                    var fxWarningTimeList = FxWarningTime;
                    var scaleFacList = ScaleFac;
                    var fxPointList = FxPoint;
                    var fxPointHighPercentList = FxPointHightPercent;
                    for (int i = 0; i < Fxs[0].normalFxs.Count; ++i)
                    {
                        var normalFx = Fxs[0].normalFxs[i];
                        if (normalFx.IsShowStreamer)
                        {
                            normalFx.FxWarningTime = fxWarningTimeList[i];
                            var scaleFacParamList = scaleFacList[i];
                            normalFx.FxParam1 = scaleFacParamList[0];
                            normalFx.FxParam2 = scaleFacParamList[1];
                            normalFx.FxParam3 = scaleFacParamList[2];
                        }
                        if (i < fxPointList.Length)
                        {
                            normalFx.FxPoint = fxPointList[i];
                        }
                        else if (i < fxPointHighPercentList.Length)
                        {
                            normalFx.FxPointHightPercent = fxPointHighPercentList[i];
                        }
                    }
                }
            }
            if (BeginFx != null && BeginFxPoint != null && BeginFxPointHightPercent != null)
            {
                //BeginFx BeginFxPoint BeginFxPointHightPercent处理
                BeginFxs.Clear();
                var beginFxList = BeginFx;
                var beginFxPointList = BeginFxPoint;
                var beginFxPointHighPercentList = BeginFxPointHightPercent;
                for (int i = 0; i < beginFxList.Length; ++i)
                {
                    var normalFx = new NormalFx();
                    normalFx.FxID = beginFxList[i];
                    if (i < beginFxList.Length)
                    {
                        normalFx.FxPoint = beginFxPointList[i];
                    }
                    else if (i < beginFxPointHighPercentList.Length)
                    {
                        normalFx.FxPointHightPercent = beginFxPointHighPercentList[i];
                    }

                    BeginFxs.Add(normalFx);
                }
            }
            if (DestroyFx != null && DestroyFxPoint != null && DestroyFxPointHightPercent != null)
            {
                //DestroyFx DestroyFxPoint DestroyFxPointHightPercent处理
                DestroyFxs.Clear();
                var destroyFxList = DestroyFx;
                var destroyFxPointList = DestroyFxPoint;
                var destroyFxPointHighPercentList = DestroyFxPointHightPercent;
                for (int i = 0; i < destroyFxList.Length; ++i)
                {
                    var normalFx = new NormalFx();
                    normalFx.FxID = destroyFxList[i];
                    if (i < destroyFxList.Length)
                    {
                        normalFx.FxPoint = destroyFxPointList[i];
                    }
                    else if (i < destroyFxPointHighPercentList.Length)
                    {
                        normalFx.FxPointHightPercent = destroyFxPointHighPercentList[i];
                    }
                    DestroyFxs.Add(normalFx);
                }
            }
            ////TimePass处理
            //if (!string.IsNullOrEmpty(rowData.TimePass))
            //{
            //    splitList = rowData.TimePass.Split('=');
            //    foreach (var it in splitList)
            //    {
            //        int.TryParse(it, out type);
            //        TimePass.Add((TimePassType)type);
            //    }
            //}
            ////BuffPriority处理
            //if (!string.IsNullOrEmpty(rowData.BuffPriority))
            //{
            //    splitList = rowData.BuffPriority.Split('=');
            //    if (splitList.Length == 2)
            //    {
            //        BuffPriorityType = splitList[0];
            //        BuffPriorityValue = splitList[1];
            //    }
            //}
            #endregion
        }
        public void OnSaveCSV()
        {
            #region 特殊数据类型处理
            StringBuilder stringBuilders;
            int idx;
            //ReplaceRule处理
            if (ReplaceRule.Capacity == 2)
            {
                ReplaceRule[0] = (int)_replaceRuleType;
                ReplaceRule[1] = _replaceRuleParam;
            }
            //DestroyTiming处理
            for (var i = 0; i < _DestroyTiming.Count; i++)
            {
                DestroyTiming[i][0] = _DestroyTiming[i].DestroyType;
                DestroyTiming[i][1] = _DestroyTiming[i].DestroyParam;
            }

            TimePass = new int[_TimePass.Count];
            for (int i = 0; i < _TimePass.Count; i++)
            {
                TimePass[i] = (int)_TimePass[i];
            }

            //Fx 处理
            Fx = new int[Fxs.Count][];
            for (int level = 0; level < Fxs.Count; ++level)
            {
                var levelFx = Fxs[level];
                Fx[level] = new int[levelFx.normalFxs.Count];
                for (int i = 0; i < levelFx.normalFxs.Count; ++i)
                {
                    Fx[level][i] = levelFx.normalFxs[i].FxID;
                }
            }

            //FxWarningTime ScaleFac FxPoint FxPointHightPercent处理
            bool isUseFxPoint;
            if (Fxs.Count > 0)
            {
                var normalFxsCount = Fxs[0].normalFxs.Count;
                FxWarningTime = new float[normalFxsCount];
                ScaleFac = new MSeq<float>[normalFxsCount];

                var fxPointBuiders = new StringBuilder();
                var fxPointHighPercentBuiders = new StringBuilder();
                isUseFxPoint = true;
                if (isUseFxPoint)
                {
                    FxPoint = new string[normalFxsCount];
                }
                else
                {
                    FxPointHightPercent = new float[normalFxsCount];

                }
                for (int i = 0; i < normalFxsCount; ++i)
                {
                    var normalFx = Fxs[0].normalFxs[i];
                    FxWarningTime[i] = normalFx.FxWarningTime;
                    ScaleFac[i] = new MSeq<float>(3);
                    ScaleFac[i][0] = normalFx.FxParam1;
                    ScaleFac[i][1] = normalFx.FxParam2;
                    ScaleFac[i][2] = normalFx.FxParam3;
                    if (isUseFxPoint)
                    {
                        if (string.IsNullOrEmpty(normalFx.FxPoint))
                        {
                            EditorUtility.DisplayDialog("持续特效第一层 配置错误！", "特效挂点配置不统一！", "知道了马上改");
                            return;
                        }
                        fxPointBuiders.Append(i > 0 ? "|" : "" + normalFx.FxPoint);
                        FxPoint[i] = normalFx.FxPoint;
                    }
                    else
                    {
                        FxPointHightPercent[i] = normalFx.FxPointHightPercent;
                    }
                }
            }
            //BeginFx BeginFxPoint BeginFxPointHightPercent处理
            isUseFxPoint = true;
            if (BeginFxs.Count > 0 && string.IsNullOrEmpty(BeginFxs[0].FxPoint))
            {
                isUseFxPoint = false;
            }
            BeginFx = new int[BeginFxs.Count];
            if (isUseFxPoint)
            {
                BeginFxPoint = new string[BeginFxs.Count];
            }
            else
            {
                BeginFxPointHightPercent = new float[BeginFxs.Count];
            }

            for (int i = 0; i < BeginFxs.Count; ++i)
            {
                var normalFx = BeginFxs[i];
                BeginFx[i] = normalFx.FxID;
                if (isUseFxPoint)
                {
                    if (string.IsNullOrEmpty(normalFx.FxPoint))
                    {
                        EditorUtility.DisplayDialog("开始特效配置错误！", "特效挂点配置不统一！", "知道了马上改");
                    }
                    BeginFxPoint[i] = normalFx.FxPoint;
                }
                else
                {
                    BeginFxPointHightPercent[i] = normalFx.FxPointHightPercent;
                }
            }
            //DestroyFx DestroyFxPoint DestroyFxPointHightPercent处理
            var destroyFxBuilders = new StringBuilder();
            var destroyFxPointBuilders = new StringBuilder();
            var destroyFxPointHightPercentBuilders = new StringBuilder();
            isUseFxPoint = true;
            if (DestroyFxs.Count > 0 && string.IsNullOrEmpty(DestroyFxs[0].FxPoint))
            {
                isUseFxPoint = false;
            }
            DestroyFx = new int[BeginFxs.Count];
            if (isUseFxPoint)
            {
                DestroyFxPoint = new string[DestroyFxs.Count];
            }
            else
            {
                DestroyFxPointHightPercent = new float[DestroyFxs.Count];
            }
            for (int i = 0; i < DestroyFxs.Count; ++i)
            {
                var normalFx = DestroyFxs[i];
                DestroyFx[i] = normalFx.FxID;
                if (isUseFxPoint)
                {
                    if (string.IsNullOrEmpty(normalFx.FxPoint))
                    {
                        EditorUtility.DisplayDialog("开始特效配置错误！", "特效挂点配置不统一！", "知道了马上改");
                        return;
                    }
                    DestroyFxPoint[i] = normalFx.FxPoint;
                }
                else
                {
                    DestroyFxPointHightPercent[i] = normalFx.FxPointHightPercent;
                }
            }
            ////TimePass处理
            //stringBuilders = new StringBuilder();
            //idx = 0;
            //foreach (var it in TimePass)
            //{
            //    stringBuilders.Append(idx > 0 ? "=" : "" + (int)it);
            //    idx++;
            //}
            //rowData.TimePass = stringBuilders.ToString();
            ////BuffPriority处理
            //rowData.BuffPriority = BuffPriorityType + "=" + BuffPriorityValue;

            #endregion
            base.OnSaveCSV();
        }

        #region 保存加载
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

        #region 触发器
        public override void CreateTrigger()
        {
            if (this.trigger == null)
            {
                string defineApi = "custom_buff";
                this.CreateTrigger(defineApi);
            }
        }
        #endregion
    }
}
