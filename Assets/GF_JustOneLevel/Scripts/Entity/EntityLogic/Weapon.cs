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

    protected override void OnInit (object userData)
    {
        base.OnInit (userData);
    }

    protected override void OnShow (object userData)
    {
        base.OnShow (userData);

        weaponData = userData as WeaponData;
        if (weaponData == null) {
            Log.Error ("Weapon data is invalid.");
            return;
        }

        GameEntry.Entity.AttachEntity (Entity, weaponData.OwnerId, AttachPoint);
    }

    protected override void OnAttachTo (EntityLogic parentEntity, Transform parentTransform, object userData)
    {
        base.OnAttachTo (parentEntity, parentTransform, userData);

        Name = string.Format ("Weapon of {0}", parentEntity.Name);
        CachedTransform.localPosition = Vector3.zero;
    }

    public void Attack (int aimEntityID, int ownerAtk) {
        BulletData bulletData = new BulletData (
            EntityExtension.GenerateSerialId (), 
            weaponData.BulletId, 
            aimEntityID,
            weaponData.OwnerCamp, 
            weaponData.Attack + ownerAtk, 
            weaponData.BulletSpeed
        );
        bulletData.Position = CachedTransform.position;
        
        EntityExtension.ShowBullet (typeof(Bullet), "BulletGroup", bulletData);
        
        GameEntry.Sound.PlaySound (weaponData.BulletSoundId);
    }
}