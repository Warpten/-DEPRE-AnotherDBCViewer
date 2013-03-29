using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DBFilesClient.NET;

// ReSharper disable CheckNamespace
namespace FileStructures

// ReSharper restore CheckNamespace
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

            var resultSet = new ClientFieldInfo[propCount];
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

        public static ListViewItem CreateTableRow(dynamic rowInfo, Type rowStruct)
        {
            var propsList = rowStruct.GetFields(BindingFlags.Instance | BindingFlags.Public);
            var rowData = new List<string>();

            foreach (var propInfo in propsList)
            {
                var cellValue = rowStruct.GetField(propInfo.Name).GetValue(rowInfo);
                if (cellValue.GetType().IsArray)
                    rowData.AddRange(from object item in (cellValue as IEnumerable) select item.ToString());
                else
                    rowData.Add(cellValue.ToString());
            }

            return new ListViewItem(rowData.ToArray());
        }

        public static bool IsFieldString(Type rowStruct, string fieldName)
        {
            return rowStruct.GetField(fieldName).FieldType == typeof(string);
        }
    }
}
