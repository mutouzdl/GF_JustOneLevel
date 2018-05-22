using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class WeaponData : AccessoryObjectData {
    [SerializeField]
    private int attack = 0;


    [SerializeField]
    private int bulletId = 0;

    [SerializeField]
    private float bulletSpeed = 0f;

    [SerializeField]
    private int bulletSoundId = 0;

    public WeaponData (int entityId, int typeId, int ownerId, CampType ownerCamp) : base (entityId, typeId, ownerId, ownerCamp) {
        IDataTable<DRWeapon> dtWeapon = GameEntry.DataTable.GetDataTable<DRWeapon> ();
        DRWeapon drWeapon = dtWeapon.GetDataRow (TypeId);
        if (drWeapon == null) {
            return;
        }

        attack = drWeapon.Attack;
        bulletId = drWeapon.BulletId;
        bulletSpeed = drWeapon.BulletSpeed;
        bulletSoundId = drWeapon.BulletSoundId;
    }

    /// <summary>
    /// 攻击力。
    /// </summary>
    public int Attack {
        get {
            return attack;
        }
    }

    /// <summary>
    /// 子弹编号。
    /// </summary>
    public int BulletId {
        get {
            return bulletId;
        }
    }

    /// <summary>
    /// 子弹速度。
    /// </summary>
    public float BulletSpeed {
        get {
            return bulletSpeed;
        }
    }

    /// <summary>
    /// 子弹声音编号。
    /// </summary>
    public int BulletSoundId {
        get {
            return bulletSoundId;
        }
    }
}