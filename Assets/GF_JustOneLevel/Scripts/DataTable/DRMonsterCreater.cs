using System.Collections.Generic;
using GameFramework.DataTable;

/// <summary>
/// 怪物生成器表
/// </summary>
public class DRMonsterCreater : IDRAssetsRow {
    /// <summary>
    /// 编号。
    /// </summary>
    public int Id {
        get;
        protected set;
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name {
        get;
        protected set;
    }
    
    /// <summary>
    /// 资源名称
    /// </summary>
    public string AssetName {
        get;
        protected set;
    }

    /// <summary>
    /// 开始创建时间（秒）
    /// </summary>
    /// <returns></returns>
    public float StartTime {
        get;
        private set;
    }

    /// <summary>
    /// 创建间隔（秒）
    /// </summary>
    public float Interval {
        get;
        protected set;
    }

    /// <summary>
    /// 每轮创建概率（0-100）
    /// </summary>
    public int Probability {
        get;
        protected set;
    }

    /// <summary>
    /// 每轮创建数量
    /// </summary>
    /// <returns></returns>
    public int PerNum {
        get;
        protected set;
    }

    /// <summary>
    /// 最大创建数量
    /// </summary>
    /// <returns></returns>
    public int MaxNum {
        get;
        protected set;
    }

    /// <summary>
    /// 怪物ID
    /// </summary>
    /// <returns></returns>
    public int MonsterTypeId {
        get;
        protected set;
    }

    /// <summary>
    /// 怪物奖励分值
    /// </summary>
    /// <returns></returns>
    public int MonsterPrize {
        get;
        protected set;
    }

    /// <summary>
    /// 强化百分比（正常为1）
    /// </summary>
    /// <returns></returns>
    public float PowerPercent {
        get;
        private set;
    }

    public virtual void ParseDataRow (string dataRowText) {
        string[] text = DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        AssetName = text[index++];
        Name = text[index++];
        StartTime = float.Parse (text[index++]);
        Interval = float.Parse (text[index++]);
        Probability = int.Parse (text[index++]);
        PerNum = int.Parse (text[index++]);
        MaxNum = int.Parse (text[index++]);
        MonsterTypeId = int.Parse (text[index++]);
        MonsterPrize = int.Parse (text[index++]);
        PowerPercent = float.Parse(text[index++]);
    }

    private void AvoidJIT () {
        new Dictionary<int, DRMonsterCreater> ();
    }
}