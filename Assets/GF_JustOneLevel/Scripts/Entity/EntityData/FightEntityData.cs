using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class FightEntityData : TargetableObjectData {
    [SerializeField]
    protected int m_MaxHP = 0;

    [SerializeField]
    protected int m_Defense = 0;
    [SerializeField]
    protected float m_MoveSpeed = 0;
    [SerializeField]
    protected float m_RotateSpeed = 0;

    [SerializeField]
    protected int m_DeadEffectId = 0;

    [SerializeField]
    protected int m_DeadSoundId = 0;

    public FightEntityData (int entityId, int typeId, CampType camp) : base (entityId, typeId, camp) {
    }

    /// <summary>
    /// 最大生命。
    /// </summary>
    public override int MaxHP {
        get {
            return m_MaxHP;
        }
    }

    /// <summary>
    /// 防御。
    /// </summary>
    public int Defense {
        get {
            return m_Defense;
        }
    }

    /// <summary>
    /// 移动速度。
    /// </summary>
    public float MoveSpeed {
        get {
            return m_MoveSpeed;
        }
    }

    /// <summary>
    /// 旋转速度。
    /// </summary>
    public float RotateSpeed {
        get {
            return m_RotateSpeed;
        }
    }

    public int DeadEffectId {
        get {
            return m_DeadEffectId;
        }
    }

    public int DeadSoundId {
        get {
            return m_DeadSoundId;
        }
    }

    private void RefreshData () {
        // m_MaxHP = 0;
        // m_Defense = 0;
        // for (int i = 0; i < m_ArmorDatas.Count; i++)
        // {
        //     m_MaxHP += m_ArmorDatas[i].MaxHP;
        //     m_Defense += m_ArmorDatas[i].Defense;
        // }

        if (HP > m_MaxHP) {
            HP = m_MaxHP;
        }
    }
}