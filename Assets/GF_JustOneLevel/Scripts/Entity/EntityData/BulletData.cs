using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class BulletData : EntityData {
    private int ownerId = 0;
    private int aimEntityId = 0;

    private CampType ownerCamp = CampType.Unknown;

    private int attack = 0;

    private float speed = 0f;
    private Vector3 forward = Vector3.zero;

    public BulletData (int entityId, int typeId, int ownerId, int aimEntityID,
        CampType ownerCamp, int attack, float speed) : base (entityId, typeId) {
        IDataTable<DRBullet> dtBullet = GameEntry.DataTable.GetDataTable<DRBullet> ();
        DRBullet drBullet = dtBullet.GetDataRow (TypeId);
        if (drBullet == null) {
            return;
        }

        ParticleId = drBullet.ParticleId;

        this.ownerId = ownerId;
        this.ownerCamp = ownerCamp;
        this.attack = attack;
        this.speed = speed;

        forward = GameEntry.Entity.GetEntity(ownerId).transform.forward;
    }

    public int ParticleId {
        get;
        private set;
    }

    public int OwnerId {
        get {
            return ownerId;
        }
    }

    public int AimEntityID {
        get {
            return aimEntityId;
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

    public Vector3 Forward {
        get {
            return forward;
        }
    }
}