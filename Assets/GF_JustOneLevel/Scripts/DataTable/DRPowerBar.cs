using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 能量条表。
/// </summary>
public class DRPowerBar : IDRAssetsRow {
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
        AssetName = text[index++];
        Color = text[index++];
    }

    private void AvoidJIT () {
        new Dictionary<int, DRPowerBar> ();
    }
}