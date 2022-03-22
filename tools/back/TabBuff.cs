namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CSVEditor;
    using Sirenix.OdinInspector.Editor;
    using UnityEngine;

    public class TabBuff : TabTriggerBase
    {
        private OdinMenuItem _lastOdinMenuItem = null;

        private CommonTool.WorkSheetInfo m_buffWorkSheetInfo;
        private CommonTool.WorkSheetInfo m_buffEffectWorkSheetInfo;

        private StringBuilder m_stringBuilder = new StringBuilder();

        private string m_buffCsvName = "BuffTable";
        private string m_buffEffectCsvName = "BuffEffectTable";

        public TabBuff(eMenuType menuType)
        {
            m_menuType = menuType;
            m_itemToolBars[0] = "效果";
            m_refreshConfigString = "刷新效果配置数据";
            m_buffWorkSheetInfo = CommonTool.GetCSVSheetInfo("BuffTable");
            m_buffEffectWorkSheetInfo = CommonTool.GetCSVSheetInfo("BuffEffectTable");
        }

        public TabBuff(eMenuType menuType, string buffTableName, string buffEffectTableName)
        {
            m_menuType = menuType;
            m_itemToolBars[0] = "效果";
            m_refreshConfigString = "刷新效果配置数据";
            m_buffCsvName = $"BuffTable/{buffTableName}";
            m_buffEffectCsvName = $"BuffEffectTable/{buffEffectTableName}";
            m_buffWorkSheetInfo = CommonTool.GetCSVSheetInfo(m_buffCsvName);
            m_buffEffectWorkSheetInfo = CommonTool.GetCSVSheetInfo(m_buffEffectCsvName);
        }

        public override void UpdateMenuItems(OdinMenuTree menutree)
        {
            var parent = menutree.RootMenuItem;

            var buffWorkSheet = m_buffWorkSheetInfo.TableSheet;
            var buffEffectWorkSheet = m_buffEffectWorkSheetInfo.TableSheet;

            int buffId;
            string strBuffEffectId;
            int buffEffectId;
            int buffEffectRowIdx;
            for (int i = 2; i < buffWorkSheet.Cells.MaxDataRow; ++i)
            {
                buffId = buffWorkSheet.Cells[i, 0].IntValue;
                string[] effectIDs = (buffWorkSheet.Cells[i, 13].StringValue).Split('|');

                string skillName = buffId + " - " + buffWorkSheet.Cells[i, 1].StringValue;
                MenuItemBuff parentItem = new MenuItemBuff(menutree, skillName, null, buffId);
                parent.ChildMenuItems.Add(parentItem);

                for (int j = 0; j < effectIDs.Length; j++)
                {
                    strBuffEffectId = effectIDs[j];
                    buffEffectId = 0;
                    if (!string.IsNullOrEmpty(effectIDs[j]))
                    {
                        int.TryParse(effectIDs[j], out buffEffectId);
                    }

                    m_stringBuilder.Clear();
                    m_stringBuilder.Append(buffEffectId);
                    m_stringBuilder.Append(" - ");
                    if (m_buffEffectWorkSheetInfo.DicID2RowIndex.TryGetValue(strBuffEffectId, out buffEffectRowIdx))
                    {
                        m_stringBuilder.Append(buffEffectWorkSheet.Cells[buffEffectRowIdx, 1].StringValue);
                        MenuItemBuffEffect item = new MenuItemBuffEffect(menutree, m_stringBuilder.ToString(), null, buffEffectId);
                        parentItem.ChildMenuItems.Add(item);
                    }
                    else
                    {
                        //Debug.LogError($"请策划查看配置:【SkillTable】表【effectIDs】中填写的特效不存在。【SkillId】= {skillId}, 【SkillEffectId】= {skillEffectId}。");
                    }
                }
            }
        }

        public override void OnClickItem(SelectionChangedType type)
        {
            if (type == SelectionChangedType.ItemAdded)
            {
                //if (m_menuTree.Selection.Count == 1 && _lastOdinMenuItem != m_menuTree.Selection[0])
                if (m_menuTree.Selection.Count == 1)
                {
                    _lastOdinMenuItem = m_menuTree.Selection[0];
                    var item = m_menuTree.Selection[0].Value;
                    if (_lastOdinMenuItem is MenuItemBuff)
                    {
                        if (item == null)
                        {
                            var menu = m_menuTree.Selection[0] as MenuItemBuff;
                            var data = ScriptableObject.CreateInstance<BuffShowData>();
                            data.OnInit(menu.Id, m_buffCsvName);
                            m_menuTree.Selection[0].Value = data;
                        }
                    }
                    else if (_lastOdinMenuItem is MenuItemBuffEffect)
                    {
                        if (item == null)
                        {
                            var menu = m_menuTree.Selection[0] as MenuItemBuffEffect;
                            var data = ScriptableObject.CreateInstance<BuffEffectShowData>();
                            data.OnInit(menu.Id, m_buffEffectCsvName);
                            m_menuTree.Selection[0].Value = data;
                        }
                    }
                }
            }
            base.OnClickItem(type);
        }
    }
}