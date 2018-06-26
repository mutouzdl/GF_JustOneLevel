using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class BulletData : EntityData {
    public BulletData (int entityId, int typeId, Vector3 forward,
        CampType ownerCamp, int attack, float speed) : base (entityId, typeId) {
        IDataTable<DRBullet> dtBullet = GameEntry.DataTable.GetDataTable<DRBullet> ();
        DRBullet drBullet = dtBullet.GetDataRow (TypeId);
        if (drBullet == null) {
            return;
        }

        EffectId = drBullet.EffectId;

        this.Forward = forward;
        this.OwnerCamp = ownerCamp;
        this.Attack = attack;
        this.Speed = speed;
    }

    public int EffectId {
        get;
        private set;
    } = 0;

    public Vector3 Forward {
        get;
        private set;
    }

    public CampType OwnerCamp {
        get;
        private set;
    } = CampType.Unknown;

    public int Attack {
        get;
        private set;
    } = 0;

    public float Speed {
        get;
        private set;
    } = 0;
}