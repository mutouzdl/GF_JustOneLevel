using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 粒子特效实体类
/// </summary>
public class EParticle : Entity {
    private const string AttachPoint = "ParticlePoint";

    private ParticleData particleData = null;
    protected override void OnInit (object userData) {
        base.OnInit (userData);
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        particleData = userData as ParticleData;
        if (particleData == null) {
            Log.Error ("EParticle data is invalid.");
            return;
        }

        GameEntry.Entity.AttachEntity (Entity, particleData.OwnerId, AttachPoint);
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }

    protected override void OnAttachTo (EntityLogic parentEntity, Transform parentTransform, object userData) {
        base.OnAttachTo (parentEntity, parentTransform, userData);

        Name = string.Format ("{0}", parentEntity.Name);
        CachedTransform.localPosition = Vector3.zero;
    }

    protected override void OnHide (object userData) {
        base.OnHide (userData);
    }
}