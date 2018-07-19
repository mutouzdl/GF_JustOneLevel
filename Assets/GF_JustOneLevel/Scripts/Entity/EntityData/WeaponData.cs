using System;
using GameFramework.DataTable;
using UnityEngine;

[Serializable]
public class WeaponData : AccessoryObjectData {
    public WeaponData (int entityId, int typeId, int ownerId, CampType ownerCamp) : base (entityId, typeId, ownerId, ownerCamp) {
        IDataTable<DRWeapon> dtWeapon = GameEntry.DataTable.GetDataTable<DRWeapon> ();
        DRWeapon drWeapon = dtWeapon.GetDataRow (TypeId);
        if (drWeapon == null) {
            return;
        }

        Name = drWeapon.Name;
        Attack = drWeapon.Attack;
        BulletId = drWeapon.BulletId;
        BulletSpeed = drWeapon.BulletSpeed;
        AtkSpeed = drWeapon.AtkSpeed;
        BulletSoundId = drWeapon.BulletSoundId;
        AttackType = (WeaponAttackType)drWeapon.AttackType;
        CostMP = drWeapon.CostMP;
    }

    /// <summary>
    /// 攻击力。
    /// </summary>
    public int Attack {
        get;
        private set;
    } = 0;

    /// <summary>
    /// 子弹编号。
    /// </summary>
    public int BulletId {
        get;
        private set;
    } = 0;

    /// <summary>
    /// 子弹速度。
    /// </summary>
    public float BulletSpeed {
        get;
        private set;
    } = 0;

    /// <summary>
    /// 子弹攻击速度。
    /// </summary>
    public float AtkSpeed {
        get;
        private set;
    } = 0;

    /// <summary>
    /// 子弹声音编号。
    /// </summary>
    public int BulletSoundId {
        get;
        private set;
    } = 0;
    
    /// <summary>
    /// 攻击类型
    /// </summary>
    public WeaponAttackType AttackType {
        get;
        private set;
    } = WeaponAttackType.手动触发;

    /// <summary>
    /// 消耗MP
    /// </summary>
    /// <returns></returns>
    public int CostMP {
        get;
        private set;
    } = 0;

    /// <summary>
    /// 名称
    /// </summary>
    /// <returns></returns>
    public string Name {
        get;
        private set;
    } = "";
}