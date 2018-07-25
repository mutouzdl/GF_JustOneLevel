using System;
using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 场景配置表。
/// </summary>
public class DRScene : IDRAssetsRow {
    /// <summary>
    /// 场景编号。
    /// </summary>
    public int Id {
        get;
        protected set;
    }

    /// <summary>
    /// 资源名称。
    /// </summary>
    public string AssetName {
        get;
        private set;
    }

    /// <summary>
    /// 背景音乐编号。
    /// </summary>
    public int BackgroundMusicId {
        get;
        private set;
    }

    public void ParseDataRow (string dataRowText) {
        string[] text = dataRowText.Split (new string[] { "," }, StringSplitOptions.None);//DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        AssetName = text[index++];
    }

    private void AvoidJIT () {
        new Dictionary<int, DRScene> ();
    }
}