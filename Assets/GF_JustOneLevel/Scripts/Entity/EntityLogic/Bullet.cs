using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 子弹类。
/// </summary>
public class Bullet : Entity {
    private BulletData bulletData = null;
    private BulletEffect bulletEffect = null;

    public ImpactData GetImpactData () {
        return new ImpactData (bulletData.OwnerCamp, 0, bulletData.Attack, 0);
    }

    protected override void OnInit (object userData) {
        base.OnInit (userData);

    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        bulletData = userData as BulletData;
        if (bulletData == null) {
            Log.Error ("Bullet data is invalid.");
            return;
        }

        CachedTransform.LookAt(GameEntry.Entity.GetEntity(bulletData.AimEntityID).transform);

        // 让子弹保持水平
        CachedTransform.forward = new Vector3(
            CachedTransform.forward.x,
            0,
            CachedTransform.forward.z);

        if (bulletData.EffectId > 0) {
            BulletEffectData bulletEffectData = new BulletEffectData (EntityExtension.GenerateSerialId (), bulletData.EffectId, Id);
            EntityExtension.ShowBulletEffect (typeof (BulletEffect), "BulletEffectGroup", bulletEffectData);
        }
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);

        CachedTransform.Translate (CachedTransform.forward * bulletData.Speed * elapseSeconds, Space.World);

        // 将超出边界的子弹隐藏
        if (PositionUtility.IsOutOfMapBoundary(CachedTransform.position)) {
            GameEntry.Entity.HideEntity(this.Id);
        }
    }

    protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData) {
        base.OnAttached (childEntity, parentTransform, userData);

        if (childEntity is BulletEffect) {
            bulletEffect = (BulletEffect) childEntity;
            bulletEffect.SetBulletData(this.bulletData);
        }
    }
}