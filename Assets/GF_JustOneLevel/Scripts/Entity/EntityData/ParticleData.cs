using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class ParticleData : EntityData {
    public ParticleData (int entityId, int typeId, int ownerId) : base (entityId, typeId) {
        this.OwnerId = ownerId;
    }

    /// <summary>
    /// 拥有者编号。
    /// </summary>
    public int OwnerId {
        get;
        private set;
    }
}