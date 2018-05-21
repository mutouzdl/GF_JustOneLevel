using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class BulletData : EntityData {
    private int m_OwnerId = 0;
    private int m_AimEntityId = 0;

    private CampType m_OwnerCamp = CampType.Unknown;

    private int m_Attack = 0;

    private float m_Speed = 0f;
    private Vector3 m_Forward = Vector3.zero;

    public BulletData (int entityId, int typeId, int ownerId, int aimEntityID,
        CampType ownerCamp, int attack, float speed) : base (entityId, typeId) {
        IDataTable<DRBullet> dtBullet = GameEntry.DataTable.GetDataTable<DRBullet> ();
        DRBullet drBullet = dtBullet.GetDataRow (TypeId);
        if (drBullet == null) {
            return;
        }

        ParticleId = drBullet.ParticleId;

        m_OwnerId = ownerId;
        m_OwnerCamp = ownerCamp;
        m_Attack = attack;
        m_Speed = speed;

        m_Forward = GameEntry.Entity.GetEntity(ownerId).transform.forward;
    }

    public int ParticleId {
        get;
        private set;
    }

    public int OwnerId {
        get {
            return m_OwnerId;
        }
    }

    public int AimEntityID {
        get {
            return m_AimEntityId;
        }
    }

    public CampType OwnerCamp {
        get {
            return m_OwnerCamp;
        }
    }

    public int Attack {
        get {
            return m_Attack;
        }
    }

    public float Speed {
        get {
            return m_Speed;
        }
    }

    public Vector3 Forward {
        get {
            return m_Forward;
        }
    }
}