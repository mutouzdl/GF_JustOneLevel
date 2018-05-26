using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class BulletData : EntityData {
    private int aimEntityID = 0;

    private CampType ownerCamp = CampType.Unknown;

    private int attack = 0;

    private float speed = 0f;

    public BulletData (int entityId, int typeId, int aimEntityID,
        CampType ownerCamp, int attack, float speed) : base (entityId, typeId) {
        IDataTable<DRBullet> dtBullet = GameEntry.DataTable.GetDataTable<DRBullet> ();
        DRBullet drBullet = dtBullet.GetDataRow (TypeId);
        if (drBullet == null) {
            return;
        }

        EffectId = drBullet.EffectId;

        this.aimEntityID = aimEntityID;
        this.ownerCamp = ownerCamp;
        this.attack = attack;
        this.speed = speed;
    }

    public int EffectId {
        get;
        private set;
    }

    public int AimEntityID {
        get {
            return aimEntityID;
        }
    }

    public CampType OwnerCamp {
        get {
            return ownerCamp;
        }
    }

    public int Attack {
        get {
            return attack;
        }
    }

    public float Speed {
        get {
            return speed;
        }
    }
}