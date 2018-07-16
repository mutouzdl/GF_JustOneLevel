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

        autoWeaponsFireTimeCounter = 0;
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
    /// 自动武器发射间隔计时器
    /// </summary>
    private float autoWeaponsFireTimeCounter = 0;
    /// <summary>
    /// 尝试进行自动攻击，只有自动类型的武器才能生效
    /// </summary>
    /// <param name="elapseSeconds"></param>
    /// <param name="ownerAtk"></param>
    public void TryAutoAttack (float elapseSeconds, int ownerAtk) {
        if (weaponData.AttackType == WeaponAttackType.自动触发) {

            autoWeaponsFireTimeCounter += elapseSeconds;
            if (autoWeaponsFireTimeCounter >= weaponData.AtkSpeed) {
                autoWeaponsFireTimeCounter = 0;

                DoAttack (ownerAtk);
            }
        }
    }

    /// <summary>
    /// 攻击
    /// </summary>
    /// <param name="ownerAtk"></param>
    public void Attack (int ownerAtk) {
        switch (weaponData.AttackType) {
            case WeaponAttackType.手动触发:
                DoAttack (ownerAtk);
                break;
            case WeaponAttackType.自动触发:
                DoAttack (ownerAtk);
                break;
            case WeaponAttackType.技能触发:
                DoAttack (ownerAtk);
                break;
        }
    }

    /// <summary>
    /// 执行攻击
    /// </summary>
    /// <param name="aimEntityID"></param>
    /// <param name="ownerAtk"></param>
    private void DoAttack (int ownerAtk) {
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