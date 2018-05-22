using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 子弹类。
/// </summary>
public class Bullet : Entity {
    private BulletData bulletData = null;
    private ParticleEffect particleEffect = null;

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

        if (bulletData.ParticleId > 0) {
            ParticleData particleData = new ParticleData (EntityExtension.GenerateSerialId (), bulletData.ParticleId, Id);
            EntityExtension.ShowParticle (typeof (ParticleEffect), "ParticleGroup", particleData);
        }
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);

        CachedTransform.Translate (bulletData.Forward * bulletData.Speed * elapseSeconds, Space.World);

        // 将超出边界的子弹隐藏
        if (PositionUtility.IsOutOfMapBoundary(CachedTransform.position)) {
            GameEntry.Entity.HideEntity(this.Id);
        }
    }

    protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData) {
        base.OnAttached (childEntity, parentTransform, userData);

        if (childEntity is ParticleEffect) {
            particleEffect = (ParticleEffect) childEntity;
        }
    }
}