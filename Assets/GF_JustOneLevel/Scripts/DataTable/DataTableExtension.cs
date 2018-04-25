using System;
using GameFramework;
using UnityGameFramework.Runtime;

/// <summary>
/// 参考来源：https://github.com/EllanJiang/StarForce
/// </summary>
public static class DataTableExtension {
    private const string DataRowClassPrefixName = "DR";
    private static readonly string[] ColumnSplitOld = new string[] { "\t" };
    private static readonly string[] ColumnSplit = new string[] { "," };

    public static void LoadDataTable (this DataTableComponent dataTableComponent, string dataTableName, object userData = null) {
        if (string.IsNullOrEmpty (dataTableName)) {
            Log.Warning ("Data table name is invalid.");
            return;
        }

        string[] splitNames = dataTableName.Split ('_');
        if (splitNames.Length > 2) {
            Log.Warning ("Data table name is invalid.");
            return;
        }

        string dataRowClassName = DataRowClassPrefixName + splitNames[0];

        Type dataRowType = Type.GetType (dataRowClassName);
        if (dataRowType == null) {
            Log.Warning ("Can not get data row type with class name '{0}'.", dataRowClassName);
            return;
        }

        string dataTableNameInType = splitNames.Length > 1 ? splitNames[1] : null;
        dataTableComponent.LoadDataTable (dataRowType, dataTableName, dataTableNameInType, AssetUtility.GetDataTableAsset (dataTableName), userData);
    }

    public static string[] SplitDataRowOld (string dataRowText) {
        return dataRowText.Split (ColumnSplitOld, StringSplitOptions.None);
    }

    public static string[] SplitDataRow (string dataRowText) {
        return dataRowText.Split (ColumnSplit, StringSplitOptions.None);
    }
}