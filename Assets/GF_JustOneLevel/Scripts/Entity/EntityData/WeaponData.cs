using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class WeaponData : AccessoryObjectData {
    [SerializeField]
    private int m_Attack = 0;


    [SerializeField]
    private int m_BulletId = 0;

    [SerializeField]
    private float m_BulletSpeed = 0f;

    [SerializeField]
    private int m_BulletSoundId = 0;

    public WeaponData (int entityId, int typeId, int ownerId, CampType ownerCamp) : base (entityId, typeId, ownerId, ownerCamp) {
        IDataTable<DRWeapon> dtWeapon = GameEntry.DataTable.GetDataTable<DRWeapon> ();
        DRWeapon drWeapon = dtWeapon.GetDataRow (TypeId);
        if (drWeapon == null) {
            return;
        }

        m_Attack = drWeapon.Attack;
        m_BulletId = drWeapon.BulletId;
        m_BulletSpeed = drWeapon.BulletSpeed;
        m_BulletSoundId = drWeapon.BulletSoundId;
    }

    /// <summary>
    /// 攻击力。
    /// </summary>
    public int Attack {
        get {
            return m_Attack;
        }
    }

    /// <summary>
    /// 子弹编号。
    /// </summary>
    public int BulletId {
        get {
            return m_BulletId;
        }
    }

    /// <summary>
    /// 子弹速度。
    /// </summary>
    public float BulletSpeed {
        get {
            return m_BulletSpeed;
        }
    }

    /// <summary>
    /// 子弹声音编号。
    /// </summary>
    public int BulletSoundId {
        get {
            return m_BulletSoundId;
        }
    }
}