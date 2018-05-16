using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 子弹表。
/// </summary>
public class DRBullet : IDataRow {
    /// <summary>
    /// 编号。
    /// </summary>
    public int Id {
        get;
        private set;
    }

    /// <summary>
    /// 资源名称。
    /// </summary>
    public string AssetName {
        get;
        protected set;
    }

    public void ParseDataRow (string dataRowText) {
        string[] text = DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        AssetName = text[index++];
    }

    private void AvoidJIT () {
        new Dictionary<int, DRBullet> ();
    }
}