using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 武器类。
/// </summary>
public class Weapon : Entity {
    private const string AttachPoint = "WeaponPoint";

    [SerializeField]
    private WeaponData weaponData = null;

    private Transform parentTransform = null;

    protected override void OnInit (object userData) {
        base.OnInit (userData);
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        weaponData = userData as WeaponData;
        if (weaponData == null) {
            Log.Error ("Weapon data is invalid.");
            return;
        }

        GameEntry.Entity.AttachEntity (Entity, weaponData.OwnerId, AttachPoint, weaponData);
    }

    protected override void OnAttachTo (EntityLogic parentEntity, Transform parentTransform, object userData) {
        base.OnAttachTo (parentEntity, parentTransform, userData);

        Name = string.Format ("Weapon of {0}", parentEntity.Name);
        CachedTransform.localPosition = Vector3.zero;
        this.parentTransform = parentTransform;
    }

    /// <summary>
    /// 获取数据编号
    /// </summary>
    /// <returns></returns>
    public int GetTypeId () {
        return weaponData.TypeId;
    }

    /// <summary>
    /// 消耗MP
    /// </summary>
    /// <returns></returns>
    public int CostMP {
        get {
            return weaponData.CostMP;
        }
    }

    /// <summary>
    /// 锁定目标攻击
    /// </summary>
    /// <param name="ownerAtk"></param>
    public void Attack (int ownerAtk) {
        switch (weaponData.AttackType)
        {
            case WeaponAttackType.手动触发:
                AttackWithAim(ownerAtk);
            break;
            case WeaponAttackType.自动触发:
                AttackWithAim(ownerAtk);
            break;
            case WeaponAttackType.技能触发:
                AttackWithAim(ownerAtk);
            break;
        }
    }

    /// <summary>
    /// 锁定目标攻击
    /// </summary>
    /// <param name="aimEntityID"></param>
    /// <param name="ownerAtk"></param>
    private void AttackWithAim (int ownerAtk) {
        CachedTransform.forward = parentTransform.forward;

        BulletData bulletData = new BulletData (
            EntityExtension.GenerateSerialId (),
            weaponData.BulletId,
            CachedTransform.forward,
            weaponData.OwnerCamp,
            weaponData.Attack + ownerAtk,
            weaponData.BulletSpeed
        );
        bulletData.Position = CachedTransform.position;

        EntityExtension.ShowBullet (typeof (Bullet), "BulletGroup", bulletData);

        GameEntry.Sound.PlaySound (weaponData.BulletSoundId);
    }
}