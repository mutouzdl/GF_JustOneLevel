using System.Collections.Generic;
using GameFramework.DataTable;

public interface IDRAssetsRow : IDataRow {
    /// <summary>
    /// 资源名称。
    /// </summary>
    string AssetName {
        get;
    }
}