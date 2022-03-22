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

        #region �ֶ���Ϣ

        [FoldoutGroup("��������", expanded: true)]
        [ShowInInspector, LabelText("Ψһ����ID")]
        public int Id;

        [FoldoutGroup("��������", expanded: true)]
        [ShowInInspector, LabelText("��������(�߻���)")]
        public string NameDesign;

        [FoldoutGroup("��������", expanded: true)]
        [ShowInInspector, LabelText("����")]
        public string Desc;

        [FoldoutGroup("��������", expanded: true)]
        [ShowInInspector, LabelText("��������")]
        public string Name;

        [FoldoutGroup("��������", expanded: true)]
        [ShowInInspector, LabelText("ʹ��������")]
        public bool UseNewSystem;

        [FoldoutGroup("��������", expanded: true)]
        [ShowInInspector, LabelText("�Ƿ����ö��η���")]
        public bool IsIP;

        [FoldoutGroup("��������", expanded: true)]
        [ShowInInspector, LabelText("���ܱ�ʶID")]
        [PropertyTooltip("ͨ��buff�������ã���ͬ�ȼ�ID��ͬ��")]
        public int SkillUuid;

        [FoldoutGroup("������Ч", expanded: true)]
        [ShowInInspector, LabelText("����ID����")]
        [PropertyTooltip("ÿ���ȼ�һ��ID,������SkillEffectTable,������SkillPassiveTable")]
        [ListDrawerSettings(DraggableItems = false)]
        public int[] EffectIDs;

        [HideInInspector]
        public enum TargetTypeEnum
        {
            [LabelText("����")]
            Passive = -1,     // ����
            [LabelText("����")]
            All = 0,          // ����
            [LabelText("�Լ�")]
            Self = 1,         // �Լ�
            [LabelText("�ѷ�")]
            Friends = 2,      // �ѷ�
            [LabelText("����")]
            Team = 3,         // ����
            [LabelText("����")]
            Enemy = 4,        // ����
            [LabelText("�������ѷ�")]
            DeadFriend = 5,   // �������ѷ�
            [LabelText("�Լ��ĳ���")]
            Pet = 6,          // �Լ��ĳ���
            [LabelText("�Լ��������ٻ���")]
            FinalSummoner = 7 // �Լ��������ٻ���
        }
        [FoldoutGroup("��������", expanded: false)]
        [ShowInInspector, LabelText("����Ŀ������")]
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
            [LabelText("���еļ���")]
            Self = 0,
            [LabelText("����ļ���")]
            Pet = 1
        }
        [FoldoutGroup("��������", expanded: false)]
        [ShowInInspector, LabelText("��������")]
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
            [LabelText("�ǳ��＼��")]
            NonePetSkill = 0,
            [LabelText("������ͨ����")]
            PetNormalAttack = 1,
            [LabelText("������������")]
            PetPositiveSkill = 2,
            [LabelText("�������⼼��")]
            PetSpecialSkill = 3
        }
        [FoldoutGroup("��������", expanded: false)]
        [ShowInInspector, LabelText("���＼������")]
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
            [LabelText("����")]
            Passive = -1,   // ����
            [LabelText("Ŀ��")]
            Target = 0,     // Ŀ��
            [LabelText("����")]
            Direction = 1,  // ����
            [LabelText("����")]
            Coordinate = 2, // ����
            [LabelText("�Լ�")]
            Self = 3        // �Լ�
        }
        [FoldoutGroup("��������", expanded: false)]
        [ShowInInspector, LabelText("����Ŀ�귶Χ����")]
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
            [LabelText("��ս")]
            Melee = 0,   // ��ս
            [LabelText("Զ��")]
            Remote = 1,  // Զ��
            [LabelText("���ͷ�ʱ����Ŀ��ľ������")]
            Distance = 2 // ���ͷ�ʱ����Ŀ��ľ������ ���ڵ���4��Զ��  С��4�׽�ս ����global������
        }

        [FoldoutGroup("��������", expanded: false)]
        [ShowInInspector, LabelText("�����������")]
        [PropertyTooltip("���ͷ�ʱ����Ŀ��ľ������(���ڵ���4��Զ��,С��4�׽�ս(����global������)")]
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
            [LabelText("����")]
            Passive = -1,    // ����
            [LabelText("�˺�")]
            Impair = 1,      // �˺�
            [LabelText("֧Ԯ")]
            Backup = 2,      // ֧Ԯ
            [LabelText("������")]
            Scorned = 3,     // ������
            [LabelText("�˺�+֧Ԯ��")]
            ImpairBackup = 4 // �˺�+֧Ԯ��
        }
        [FoldoutGroup("��������", expanded: false)]
        [ShowInInspector, LabelText("���ܳ������")]
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
            [LabelText("��ͨ����")]
            Common = 0,           // ��ͨ����
            [LabelText("͵���༼��")]
            Steal = 1,            // ͵���༼
            [LabelText("���ﻥ��")]
            MonsterFistfight = 2, // ���ﻥ��
            [LabelText("λ���༼��")]
            Shift = 3,            // λ���༼��
            [LabelText("����༼��")]
            Control = 4,          // ����༼��
            [LabelText("����༼��")]
            Ride = 5              // ����༼��
        }
        [FoldoutGroup("��������", expanded: false)]
        [ShowInInspector, LabelText("��������")]
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
            [LabelText("����")]
            Passive = -1,       // ����
            [LabelText("�չ�")]
            Attack = 0,         // �չ�
            [LabelText("����")]
            PhysicalAttack = 1, // ����
            [LabelText("ħ��")]
            MagicalAttack = 2,  // ħ��
            [LabelText("���")]
            MixedAttack = 3,    // ���
        }
        [FoldoutGroup("��������", expanded: false)]
        [ShowInInspector, LabelText("�����˺�����")]
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
            [LabelText("����")]
            Passive = -1,     // ����
            [LabelText("����Ӱ��")]
            NoInfluenced = 0, // ����Ӱ��
            [LabelText("��Ӱ��")]
            Influenced = 1    // ��Ӱ��
        }
        [FoldoutGroup("���ܼ���״̬", expanded: false)]
        [ShowInInspector, LabelText("�Ƿ�ASPDӰ��")]
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

        [FoldoutGroup("����״̬", expanded: false)]
        [ShowInInspector, LabelText("ASPDӰ��ٷֱ�")]
        public float ASPDPercent;

        [FoldoutGroup("���ܼ���״̬", expanded: false)]
        [ShowInInspector, LabelText("���ܲ����Ĺ�CD�Ƿ��ܵ�ASPD�ӳ�")]
        public bool IsAffectedASPDExtra;

        [FoldoutGroup("��������", expanded: false)]
        [ShowInInspector, LabelText("�Ƿ�AOE����")]
        public bool IsAoe;

        [HideInInspector]
        public enum PushInterruptEnum
        {
            [LabelText("���ɱ�����")]
            CannotRepelled = 0,          // ���ɱ�����
            [LabelText("���Ա����˵������")]
            RepelledCannotInterrupt = 1, // ���Ա����˵������
            [LabelText("���Ա����˲��Ҵ�ϼ���״̬")]
            RepelledInterruptible = 2    // ���Ա����˲��Ҵ�ϼ���״̬
        }
        [FoldoutGroup("����״̬", expanded: false)]
        [ShowInInspector, LabelText("����״̬���������")]
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

        [FoldoutGroup("����״̬", expanded: false)]
        [ShowInInspector, LabelText("��������")]
        [PropertyTooltip("����������-1")]
        public int SkillAttr;

        [FoldoutGroup("����״̬", expanded: false)]
        [ShowInInspector, LabelText("�Ƿ���Ի���")]
        public bool IsKnockBack;

        [FoldoutGroup("�ͷ�����", expanded: true)]
        [ShowInInspector, LabelText("����ʧЧ��Χ")]
        [PropertyTooltip("��0ʱĬ��Ϊ20")]
        public int Length;

        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("�Ƿ����������Ӱ��")]
        public bool IsRangeAttrImpact;

        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("�Ƿ�������Ƚ����")]
        public bool IsRangeWeaponAttrImpact;

        [FoldoutGroup("�ͷ�����", expanded: false)]
        [ShowInInspector, LabelText("��������")]
        [PropertyTooltip("���������ƣ�������EquipWeaponTable�����ֵ���ɶ���")]
        public int[] WeaponTypeLimit;

        [FoldoutGroup("�ͷ�����", expanded: true)]
        [ShowInInspector, LabelText("������������")]
        [PropertyTooltip("���������ƣ�������EquipWeaponTable�����ֵ���ɶ���")]
        public int[] SecondWeaponTypeLimit;

        [FoldoutGroup("�ͷ�����", expanded: true)]
        [ShowInInspector, LabelText("����ͬʱ���ڸ�������")]
        [PropertyTooltip("������")]
        public string Limits;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("�Ƿ�����CD")]
        public bool IsDisableCD;

        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("����ļ���ID����")]
        [PropertyTooltip("ÿ���ȼ�һ��ID")]
        public int[] EffectExIDs;

        [HideInInspector]
        public MSeq<int> SkillPanelPos;
        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("����λ��")]
        public List<int> _SkillPanelPos = new List<int>();

        [FoldoutGroup("�ͷ�����", expanded: false)]
        [ShowInInspector, LabelText("����ǰ������")]
        [PropertyTooltip("0����")]
        public int[][] SpePreSkillRequired;

        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("�ͷź��Ƿ��Զ��չ�")]
        public bool LinkAutoAtack;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("�ͷż����Ƿ�ı�Ŀ����ɫ")]
        public bool IsChangeColor;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("�ͷż����Ƿ�ı�Ŀ����ɫ")]
        [PropertyTooltip("��дģ������ı䣬����д�򲻱�")]
        public string ChangeColor;

        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("��ǿ�ƴ�ϵļ���ID")]
        [PropertyTooltip("�����ö��")]
        public int[] InteruptSkill;

        [HideInInspector]
        public MSeq<int> IsUseArrow;
        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("�Ƿ�ʹ�ü�ʸ����������")]
        public List<int> _IsUseArrow = new List<int>();

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("�����ͼ��")]
        [PropertyTooltip("�����⼼���Ƕ�������ʱ����")]
        public string IconEx;

        [FoldoutGroup("�ͷ�����", expanded: false)]
        [ShowInInspector, LabelText("ָ������ID�����ͷ�")]
        public int[] LimitScene;

        [FoldoutGroup("�ͷ�����", expanded: true)]
        [ShowInInspector, LabelText("ָ���������Ͳ����ͷ�")]
        public int[] LimitSceneType;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("�Ƿ���Ҫ��ʾʩ����Χ")]
        public bool NeedEffectRange;

        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("���⼼���Ƿ�Ϊ�����ļ���")]
        public bool IsEffectExStandalone;

        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("�Ƿ�Ϊ��������")]
        public bool IsPassive;

        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("�Զ�ս���ͷ����ȼ�")]
        [PropertyTooltip("Ĭ��Ϊ0������Խ�����ȼ�Խ��")]
        public int AutoBattlePRI;

        [FoldoutGroup("�ͷ�����", expanded: false)]
        [ShowInInspector, LabelText("��������")]
        [PropertyTooltip("��SkillEffectTable���ж�Ӧ���ܵĲ�ͬ�ȼ������ֵ��û������")]
        public int[] SkillEffectElementEnergy;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("��������ʱ����ʾ")]
        [PropertyTooltip("ֻ�������༼�ܲŻ���ʾ����ӦChineseTable�ֶ�")]
        public string EnergyTips;

        [HideInInspector]
        public enum AnimationTypeEnum
        {
            [LabelText("��������")]
            Passive = -1, // ��������
            [LabelText("ͨ��")]
            Common = 0,   // ͨ��
            [LabelText("ְҵ")]
            Career = 1    // ְҵ
        }
        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("�����ļ���")]
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

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("��������˳��")]
        [PropertyTooltip("����-1��� ����0���� ����0��������˳�����Ų���")]
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
        [FoldoutGroup("�ͷ�����", expanded: false)]
        [ShowInInspector, LabelText("ǰ�ü��ܺ͵ȼ�")]
        [PropertyTooltip("id=lv,id=lv")]
        public List<Seq> _PreSkillRequired = new List<Seq>();


        [HideInInspector]
        public enum SkillTypeIconEnum
        {
            [LabelText("����")]
            Physical = 0, // ����
            [LabelText("����")]
            Magical = 1,  // ����
            [LabelText("֧Ԯ")]
            Backup = 2,   // ֧Ԯ
            [LabelText("����")]
            Passive = 3,  // ����
        }
        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("�������ͼ��")]
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

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("ͼ������")]
        public string Atlas;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("ͼ��")]
        public string Icon;

        [FoldoutGroup("���ü���", expanded: false)]
        [ShowInInspector, LabelText("���ܽű�·��")]
        [PropertyTooltip("�˵���ֵĲ���")]
        public string SkillDataPath;

        [FoldoutGroup("���ü���", expanded: false)]
        [ShowInInspector, LabelText("�����Զ�ս��AI")]
        [PropertyTooltip("�˵���ֵĲ��Ҫ�Զ��ŵ�����Զ��ŵĲ���")]
        public string AITreeName;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("��������")]
        public string SkillDesc;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("���ܶ�������")]
        public string SkillDescAdd;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("�������ü��ܼ���")]
        public string SimpleSkillDesc;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("��Ƭ������������")]
        public string ActiveCardDesc;

        [FoldoutGroup("ǰ����ʾ��", expanded: false)]
        [ShowInInspector, LabelText("�޷��϶���UI�����")]
        [PropertyTooltip("����ʾ�ڼ�����壬�����޷��϶���UI����� 0��ʾ�������� 1��ʾ�޷��϶���UI Ĭ����0")]
        public int AllowUI;

        [FoldoutGroup("����", expanded: false)]
        [ShowInInspector, LabelText("���ػ����򻮷�")]
        [PropertyTooltip("0���� 1�� 2�� 4��� 8�� 16�����������ʱ���ֵ��ӣ������պ����Ͼ���4 + 8 = 12")]
        public string LocalizationArea;

        #endregion

        protected override void OnLoadCSV()
        {
            base.OnLoadCSV();

            #region �����������ʹ���
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
            #region �����������ʹ���
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

        #region ����&����
        [ButtonGroup]
        [Button("����", ButtonSizes.Medium), GUIColor(120 / 255.0f, 151 / 255.0f, 171 / 255.0f)]
        private void SaveCSV()
        {
            OnSaveCSV();
            UnityEditor.EditorWindow.focusedWindow.ShowNotification(new GUIContent("����ɹ�"));
        }

        [ButtonGroup]
        [Button("����", ButtonSizes.Medium), GUIColor(216 / 255.0f, 133 / 255.0f, 163 / 255.0f)]
        private void LoadCSV()
        {
            OnLoadCSV();
            UnityEditor.EditorWindow.focusedWindow.ShowNotification(new GUIContent("���سɹ�"));
        }

        [ButtonGroup]
        [Button("���ٽ�����Ϸ", ButtonSizes.Medium), GUIColor(151 / 255.0f, 219 / 255.0f, 174 / 255.0f)]
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