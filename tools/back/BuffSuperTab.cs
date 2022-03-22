
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Sirenix.OdinInspector.Demos.RPGEditor
{

    public class BuffSuperTab : TabBase
    {
        public static string[] SubToolbarContentArr = new string[] { "BuffPetTable", "BuffPVPTable", "BuffTable" };
        public static string[] SubToolbarEffectContentArr = new string[] { "BuffEffectPetTable", "BuffEffectPVPTable", "BuffEffectTable" };
        public TabBase[] SubTabInstances;

        private int m_iSubSelect;
        private TabBase m_currentSubTab;
        public BuffSuperTab()
        {
            SubTabInstances = new TabBase[SubToolbarContentArr.Length];
            for (int i = 0; i < SubToolbarContentArr.Length; i++)
            {
                SubTabInstances[i] = new TabBuff(eMenuType.Buff, SubToolbarContentArr[i], SubToolbarEffectContentArr[i]);
            }
        }

        public override void OnGUI()
        {
            base.OnGUI();
            m_iSubSelect = GUILayout.Toolbar(m_iSubSelect, SubToolbarContentArr, CustomEditorStyle.toolbarButton);
            TabBase selected = SubTabInstances[m_iSubSelect];
            if (m_currentSubTab == null)
            {
                m_currentSubTab = selected;
                m_currentSubTab.Owner = m_owner;
                m_currentSubTab.OnEnable();
            }
            else
            {
                if (m_currentSubTab != selected)
                {
                    m_currentSubTab.OnDisable();
                    m_currentSubTab = selected;
                    m_currentSubTab.Owner = m_owner;
                    m_currentSubTab.OnEnable();
                    m_owner.RefreshMenuTree = true;
                }
            }
        }

        public override void UpdateMenuItems(OdinMenuTree tree)
        {
            m_currentSubTab.UpdateMenuItems(tree);
        }

        public override OdinMenuTree BuildMenuTree(bool isForceRebuild)
        {
            return m_currentSubTab.BuildMenuTree(isForceRebuild);
        }

        public override void DeleteTrigger()
        {
            m_currentSubTab.DeleteTrigger();
        }

        public override CommonMenuWindow GetMenu(OdinMenuItem item)
        {
            return m_currentSubTab.GetMenu(item);
        }

        public override CommonMenuWindow GetRootMenu()
        {
            return m_currentSubTab.GetRootMenu();
        }

        public override TriggerInspector GetTriggerInspector()
        {
            return m_currentSubTab.GetTriggerInspector();
        }

        public override void HandleMouseEvents()
        {
            m_currentSubTab.HandleMouseEvents();
        }

        public override void OnBeginDrawEditors()
        {
            m_currentSubTab.OnBeginDrawEditors();
        }

        public override void OnClickItem(SelectionChangedType type)
        {
            m_currentSubTab.OnClickItem(type);
        }

        public override void OnDoubleClickItem(OdinMenuTreeSelection selection)
        {
            m_currentSubTab.OnDoubleClickItem(selection);
        }

        public override void OnDisable()
        {
            m_currentSubTab.OnDisable();
        }

        public override void OnEnable()
        {
            if (m_currentSubTab == null)
            {
                m_currentSubTab = SubTabInstances[m_iSubSelect];
                m_currentSubTab.Owner = m_owner;
            }
            m_currentSubTab.OnEnable();
        }

        public override void OnEndDrawEditors()
        {
            m_currentSubTab.OnEndDrawEditors();
        }

        public override void OnFocus()
        {
            m_currentSubTab.OnFocus();
        }

        public override void OnLostFocus()
        {
            m_currentSubTab.OnLostFocus();
        }

        public override void RefreshConfigData()
        {
            m_currentSubTab.RefreshConfigData();
        }
    }
}