using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class MagicWaterData : EntityData {
    public MagicWaterData (int entityId, int typeId) : base (entityId, typeId) {
        IDataTable<DRMagicWater> dtMagicWater = GameEntry.DataTable.GetDataTable<DRMagicWater> ();
        DRMagicWater drMagicWater = dtMagicWater.GetDataRow (TypeId);
        if (drMagicWater == null) {
            return;
        }

        Delay = drMagicWater.Delay;
        AddHPPercent = drMagicWater.AddHPPercent;
        AddHP = drMagicWater.AddHP;
        AddAtk = drMagicWater.AddAtk;
        AddGold = drMagicWater.AddGold;
        CreateCloneHeroTypeID = drMagicWater.CreateCloneHeroTypeID;
        Position = drMagicWater.Position;
        ParticleTypeID = drMagicWater.ParticleTypeID;
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
}