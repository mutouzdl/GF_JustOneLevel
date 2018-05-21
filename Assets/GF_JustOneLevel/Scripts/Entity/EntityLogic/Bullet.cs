using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 子弹类。
/// </summary>
public class Bullet : Entity {
    private BulletData m_BulletData = null;
    private ParticleEffect m_ParticleEffect = null;

    public ImpactData GetImpactData () {
        return new ImpactData (m_BulletData.OwnerCamp, 0, m_BulletData.Attack, 0);
    }

    protected override void OnInit (object userData) {
        base.OnInit (userData);

    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        m_BulletData = userData as BulletData;
        if (m_BulletData == null) {
            Log.Error ("Bullet data is invalid.");
            return;
        }

        if (m_BulletData.ParticleId > 0) {
            ParticleData particleData = new ParticleData (EntityExtension.GenerateSerialId (), m_BulletData.ParticleId, Id);
            EntityExtension.ShowParticle (typeof (ParticleEffect), "ParticleGroup", particleData);
        }
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);

        CachedTransform.Translate (m_BulletData.Forward * m_BulletData.Speed * elapseSeconds, Space.World);

        // 将超出边界的子弹隐藏
        if (PositionUtility.IsOutOfMapBoundary(CachedTransform.position)) {
            GameEntry.Entity.HideEntity(this.Id);
        }
    }

    protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData) {
        base.OnAttached (childEntity, parentTransform, userData);

        if (childEntity is ParticleEffect) {
            m_ParticleEffect = (ParticleEffect) childEntity;
        }
    }
}