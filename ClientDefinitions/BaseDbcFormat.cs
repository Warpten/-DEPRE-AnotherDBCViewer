using DBFilesClient.NET;
using System.Reflection;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Collections;

namespace FileStructures.DBC.Cataclysm
{
    public struct ClientFieldInfo
    {
        public string name;
        public Type type;
        public int arraySize; // Only if that's an array
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
                resultSet[i] = new ClientFieldInfo();
                resultSet[i].name = propsList[i].Name;
                resultSet[i].type = propsList[i].FieldType;
                object[] customAttrs = propsList[i].GetCustomAttributes(true);
                foreach (object attr in customAttrs)
                {
                    if (!(attr is StoragePresenceAttribute))
                        break;
                    resultSet[i].arraySize = (attr as StoragePresenceAttribute).ArraySize;
                    break;
                }
            }

            return resultSet;
        }

        public static ListViewItem CreateTableRow(dynamic rowInfo, Type structInfo, Type rowStruct)
        {
            // Get FieldInfo for the record, which is just the class's text name
            ClientFieldInfo[] structCustomInfo = GetStructure(structInfo);
            int colCount = 0;
            foreach (var columnInfo in structCustomInfo)
                colCount += columnInfo.arraySize == 0 ? 1 : columnInfo.arraySize;
            string[] rowData = new string[colCount];

            int i = 0;
            foreach (var columnInfo in structCustomInfo)
            {
                var cellValue = rowStruct.GetField(columnInfo.name).GetValue(rowInfo);
                if (cellValue.GetType().IsArray)
                {
                    foreach (object item in (cellValue as IEnumerable))
                    {
                        rowData[i] = item.ToString();
                        ++i;
                    }
                }
                else
                {
                    rowData[i] = cellValue.ToString();
                    ++i;
                }
            }

            return new ListViewItem(rowData);
        }
    }
}
