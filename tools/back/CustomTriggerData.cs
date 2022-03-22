using CSVEditor;
using LitJson;
using MoonCommonLib;
using Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ToolLib.CSV;
using ToolLib.Excel;
using UnityEngine;

namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    public class CustomTriggerData : CustomItemTrigger
    {
        [HideInInspector]
        protected Dictionary<string, Parser> _cacheParsers = new Dictionary<string, Parser>(); // 缓存所有解析器

        [HideInInspector]
        protected TableInfo _fields; // 表格的表头信息

        [HideInInspector]
        protected int UID;

        [HideInInspector]
        protected string tableNamePrefix = "";

        [HideInInspector]
        protected string tableName;

        [HideInInspector]
        protected string csvName; // 用于加载和保存csv

        [HideInInspector]
        protected Dictionary<string, string> _cacheRow = new Dictionary<string, string>(); // 缓存当前一行的数据

        public virtual void OnInit(int UID, string _csvName = "")
        {
            this.UID = UID;
            this.ID = UID.ToString();
            csvName = !string.IsNullOrEmpty(_csvName) ? _csvName : tableName;
            var json = FileEx.ReadText(Path.Combine(ExcelUtil.ExcelConfigPath, tableName + ".json"));
            _fields = JsonMapper.ToObject<TableInfo>(json);

            OnLoadCSV();
        }

        protected virtual void OnLoadCSV()
        {
            if (_fields == null)
            {
                return;
            }
            var workSheetInfo = CommonTool.GetCSVSheetInfo(csvName, true);
            foreach (var field in _fields.Fields)
            {
                if (!_cacheParsers.ContainsKey(field.FieldTypeName))
                {
                    _cacheParsers.Add(field.FieldTypeName, ParserUtil.GetParser(field.FieldTypeName));
                }

                var fieldObj = this.GetType().GetField(field.FieldName);
                var propertyObj = this.GetType().GetProperty(field.FieldName);
                if (fieldObj != null)
                {
                    var element = CommonTool.GetCellValue(csvName, UID.ToString(), field.FieldName, workSheetInfo);
                    //MDebug.singleton.AddGreenLog($"{field.FieldName}, {field.FieldTypeName}");
                    if (!string.IsNullOrEmpty(element))
                    {
                        _cacheParsers[field.FieldTypeName].Parse(element, out object val);
                        fieldObj.SetValue(this, val);
                    }
                    
                }
                else if (propertyObj != null)
                {
                    var element = CommonTool.GetCellValue(csvName, UID.ToString(), field.FieldName, workSheetInfo);
                    //MDebug.singleton.AddGreenLog($"{field.FieldName}, {field.FieldTypeName}");
                    if (!string.IsNullOrEmpty(element))
                    {
                        _cacheParsers[field.FieldTypeName].Parse(element, out object val);
                        propertyObj.SetValue(this, val);
                    }
                }
            }
        }

        protected virtual void OnSaveCSV()
        {
            if (_fields == null)
            {
                return;
            }
            foreach (var field in _fields.Fields)
            {
                if (!_cacheParsers.ContainsKey(field.FieldTypeName))
                {
                    _cacheParsers.Add(field.FieldTypeName, ParserUtil.GetParser(field.FieldTypeName));
                }

                var fieldObj = this.GetType().GetField(field.FieldName);
                var propertyObj = this.GetType().GetProperty(field.FieldName);
                if (fieldObj != null && fieldObj.GetValue(this) != null)
                {
                    //MDebug.singleton.AddGreenLog($"{field.FieldName}, {field.FieldTypeName}");
                    try
                    {
                        var res = _cacheParsers[field.FieldTypeName].SerializeExcel(fieldObj.GetValue(this));
                        if (_cacheRow.ContainsKey(field.FieldName))
                        {
                            _cacheRow[field.FieldName] = res;
                        }
                        else
                        {
                            _cacheRow.Add(field.FieldName, res);
                        }
                    }
                    catch (Exception ex)
                    {
                        MDebug.singleton.AddErrorLog($"Table Name: {tableName}\nFiled Name: {field.FieldName}\nField Type Name: {field.FieldTypeName}:\n {ex.ToString()}");
                    }
                }
                else if (propertyObj != null && propertyObj.GetValue(this) != null)
                {
                    try
                    {
                        var res = _cacheParsers[field.FieldTypeName].SerializeExcel(propertyObj.GetValue(this));
                        if (_cacheRow.ContainsKey(field.FieldName))
                        {
                            _cacheRow[field.FieldName] = res;
                        }
                        else
                        {
                            _cacheRow.Add(field.FieldName, res);
                        }
                    }
                    catch (Exception ex)
                    {
                        MDebug.singleton.AddErrorLog($"Table Name: {tableName}\nFiled Name: {field.FieldName}\nField Type Name: {field.FieldTypeName}:\n {ex.ToString()}");
                    }
                }
            }
            CommonTool.ExportToCSV(UID, _cacheRow, Path.Combine(CSVUtil.CSVPath, $"{csvName}.csv"));
            GenerateBytes();
        }

        private void GenerateBytes()
        {
            bool ret = ToolLib.Excel.ExcelExporter.GenBytes(_fields, true, true, false);
            if (!ret)
            {
                MDebug.singleton.AddErrorLogF("表 {0} 生成bytes失败", _fields.MainTableName);
            }
        }
    }
}

