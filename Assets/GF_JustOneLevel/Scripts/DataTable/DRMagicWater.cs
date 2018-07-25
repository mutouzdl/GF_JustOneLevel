using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

/// <summary>
/// 魔力泉表。
/// </summary>
public class DRMagicWater : IDRAssetsRow {
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
    /// 产生效果的间隔
    /// </summary>
    public float Delay {
        get;
        private set;
    }

    /// <summary>
    /// 增加血量百分比
    /// </summary>
    public float AddHPPercent {
        get;
        private set;
    }

    /// <summary>
    /// 增加血量绝对值
    /// </summary>
    public int AddHP {
        get;
        private set;
    }

    /// <summary>
    /// 增加攻击
    /// </summary>
    public int AddAtk {
        get;
        private set;
    }
    
    /// <summary>
    /// 增加金币
    /// </summary>
    public int AddGold {
        get;
        private set;
    }

    /// <summary>
    /// 创建克隆英雄的ID
    /// </summary>
    /// <returns></returns>
    public int CreateCloneHeroTypeID {
        get;
        private set;
    }

    /// <summary>
    /// 粒子特效ID
    /// </summary>
    /// <returns></returns>
    public int ParticleTypeID {
        get;
        private set;
    }

    /// <summary>
    /// 坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 Position {
        get;
        private set;
    }

    public void ParseDataRow (string dataRowText) {
        string[] text = DataTableExtension.SplitDataRow (dataRowText);
        int index = 0;
        index++;
        Id = int.Parse (text[index++]);
        AssetName = text[index++];
        Delay = float.Parse(text[index++]);
        AddHPPercent = float.Parse(text[index++]);
        AddHP = int.Parse(text[index++]);
        AddAtk = int.Parse(text[index++]);
        AddGold = int.Parse(text[index++]);
        CreateCloneHeroTypeID = int.Parse(text[index++]);
        ParticleTypeID = int.Parse(text[index++]);

        // 读取坐标
        string[] pos = text[index++].Split('|');
        Position = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
    }

    private void AvoidJIT () {
        new Dictionary<int, DRMagicWater> ();
    }
}