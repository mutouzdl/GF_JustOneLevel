using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 子弹特效表
/// </summary>
public class DRBulletEffect : IDRAssetsRow {
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
        private set;
    }

    /// <summary>
    /// 子弹特效类型
    /// </summary>
    /// <returns></returns>
    public int Type {
        get;
        private set;
    }

    /// <summary>
    /// 产生效果次数
    /// </summary>
    /// <returns></returns>
    public int EffectTimes {
        get;
        private set;
    }

    public void ParseDataRow (string dataRowText) {
        string[] text = DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        index++; // 备注列
        AssetName = text[index++];
        Type = int.Parse (text[index++]);
        EffectTimes = int.Parse (text[index++]);
    }

    private void AvoidJIT () {
        new Dictionary<int, DRBulletEffect> ();
    }
}