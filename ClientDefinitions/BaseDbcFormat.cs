﻿using DBFilesClient.NET;
using System.Reflection;
using System.Windows.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace FileStructures
{
    public class ClientFieldInfo
    {
        public string Name;
        public Type FieldType;
        public int ArraySize; // Only if that's an array

        public ClientFieldInfo(FieldInfo info)
        {
            Name = info.Name;
            FieldType = info.FieldType;
            ArraySize = 0;
        }
    }

    public class BaseDbcFormat
    {
        public static ClientFieldInfo[] GetStructure(Type typeInfo)
        {
            var propsList = typeInfo.GetFields(BindingFlags.Instance | BindingFlags.Public);
            int propCount = propsList.Length;

            ClientFieldInfo[] resultSet = new ClientFieldInfo[propCount];
            for (int i = 0; i < propCount; ++i)
            {
                resultSet[i] = new ClientFieldInfo(propsList[i]);
                foreach (object attr in propsList[i].GetCustomAttributes(true))
                {
                    if (attr is StoragePresenceAttribute)
                    {
                        resultSet[i].ArraySize = (attr as StoragePresenceAttribute).ArraySize;
                        break;
                    }
                }
            }

            return resultSet;
        }

        public static ListViewItem CreateTableRow(dynamic rowInfo, Type structInfo, Type rowStruct)
        {
            ClientFieldInfo[] structCustomInfo = GetStructure(structInfo);
            List<string> rowData = new List<string>();

            foreach (var columnInfo in structCustomInfo)
            {
                var cellValue = rowStruct.GetField(columnInfo.Name).GetValue(rowInfo);
                if (cellValue.GetType().IsArray)
                {
                    foreach (object item in (cellValue as IEnumerable))
                        rowData.Add(item.ToString());
                }
                else
                    rowData.Add(cellValue.ToString());
            }

            return new ListViewItem(rowData.ToArray());
        }
    }
}
