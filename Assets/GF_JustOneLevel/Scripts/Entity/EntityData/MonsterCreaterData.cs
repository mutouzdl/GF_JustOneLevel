using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class MonsterCreaterData : EntityData {
    public MonsterCreaterData (int entityId, int typeId) : base (entityId, typeId) {
        IDataTable<DRMonsterCreater> dtMonsterCreater = GameEntry.DataTable.GetDataTable<DRMonsterCreater> ();
        DRMonsterCreater drMonsterCreater = dtMonsterCreater.GetDataRow (typeId);
        if (drMonsterCreater == null) {
            return;
        }

        AssetName = drMonsterCreater.AssetName;
        Name = drMonsterCreater.Name;
        StartTime = drMonsterCreater.StartTime;
        Interval = drMonsterCreater.Interval;
        Probability = drMonsterCreater.Probability;
        PerNum = drMonsterCreater.PerNum;
        MaxNum = drMonsterCreater.MaxNum;
        MonsterTypeId = drMonsterCreater.MonsterTypeId;
        MonsterPrize = drMonsterCreater.MonsterPrize;
        PowerPercent = drMonsterCreater.PowerPercent;
    }

    /// <summary>
    /// 资源名称
    /// </summary>
    public string AssetName {
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
    /// 创建间隔（秒）
    /// </summary>
    public float Interval {
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
}