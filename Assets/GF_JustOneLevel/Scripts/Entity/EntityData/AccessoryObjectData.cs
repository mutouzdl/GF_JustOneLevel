using System;
using UnityEngine;

[Serializable]
public abstract class AccessoryObjectData : EntityData {
    private int ownerId = 0;

    private CampType ownerCamp = CampType.Unknown;

    public AccessoryObjectData (int entityId, int typeId, int ownerId, CampType ownerCamp) : base (entityId, typeId) {
        this.ownerId = ownerId;
        this.ownerCamp = ownerCamp;
    }

    /// <summary>
    /// 拥有者编号。
    /// </summary>
    public int OwnerId {
        get {
            return ownerId;
        }
    }

    /// <summary>
    /// 拥有者阵营。
    /// </summary>
    public CampType OwnerCamp {
        get {
            return ownerCamp;
        }
    }
}