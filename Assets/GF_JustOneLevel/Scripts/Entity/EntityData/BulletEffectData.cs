using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class BulletEffectData : EntityData {
    public BulletEffectData (int entityId, int typeId, int ownerId) : base (entityId, typeId) {
        this.OwnerId = ownerId;

        DRBulletEffect drBulletEffect = GameEntry.DataTable.GetDataRow<DRBulletEffect> (TypeId);

        if (drBulletEffect == null) {
            return;
        }

        this.Type = drBulletEffect.Type;
        this.EffectTimes = drBulletEffect.EffectTimes;
    }

    /// <summary>
    /// 拥有者编号。
    /// </summary>
    public int OwnerId {
        get;
        private set;
    }

    /// <summary>
    /// 特效类型
    /// </summary>
    /// <returns></returns>
    public int Type {
        get;

    }   
    
    /// <summary>
    /// 产生效果次数
    /// </summary>
    /// <returns></returns>
    public int EffectTimes {
        get;

    }
}