using System;
using UnityEngine;

[Serializable]
public class BulletData : EntityData {
    [SerializeField]
    private int m_OwnerId = 0;

    [SerializeField]
    private CampType m_OwnerCamp = CampType.Unknown;

    [SerializeField]
    private int m_Attack = 0;

    [SerializeField]
    private float m_Speed = 0f;
    [SerializeField]
    private Vector3 m_Forward = Vector3.zero;

    public BulletData (int entityId, int typeId, int ownerId, CampType ownerCamp, int attack, float speed, Vector3 forward) : base (entityId, typeId) {
        m_OwnerId = ownerId;
        m_OwnerCamp = ownerCamp;
        m_Attack = attack;
        m_Speed = speed;

        m_Forward = forward;
    }

    public int OwnerId {
        get {
            return m_OwnerId;
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