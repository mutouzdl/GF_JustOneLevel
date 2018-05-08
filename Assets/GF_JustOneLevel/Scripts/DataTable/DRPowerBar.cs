using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 能量条表。
/// </summary>
public class DRPowerBar : IDataRow {
    /// <summary>
    /// 编号。
    /// </summary>
    public int Id {
        get;
        private set;
    }

    /// <summary>
    /// 颜色
    /// </summary>
    public string Color {
        get;
        private set;
    }

    public void ParseDataRow (string dataRowText) {
        string[] text = DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        index++;
        Color = text[index++];
    }

    private void AvoidJIT () {
        new Dictionary<int, DRPowerBar> ();
    }
}