using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 英雄商店表。
/// </summary>
public class DRHeroShop : IDRAssetsRow {
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
    /// 英雄名称。
    /// </summary>
    public string Name {
        get;
        protected set;
    }

    /// <summary>
    /// 描述
    /// </summary>
    public string Des {
        get;
        private set;
    }

    /// <summary>
    /// 价格
    /// </summary>
    /// <returns></returns>
    public int Price {
        get;
        private set;
    }

    public void ParseDataRow (string dataRowText) {
        string[] text = DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        AssetName = text[index++];
        Name = text[index++];
        Des = text[index++];
        Price = int.Parse (text[index++]);
    }

    private void AvoidJIT () {
        new Dictionary<int, DRHeroShop> ();
    }
}