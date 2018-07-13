using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 子弹类。
/// </summary>
public class Bullet : Entity {
    private BulletData bulletData = null;
    private BulletEffect bulletEffect = null;
    /// <summary>
    /// 剩余可产生效果次数
    /// </summary>
    private int leftEffectTimes = 1;

    /// <summary>
    /// 速度为0的子弹自动销毁时间记录
    /// </summary>
    private float zeroSpeedAutoDestroyTimes = 0;

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

        leftEffectTimes = 1;
        zeroSpeedAutoDestroyTimes = 0;

        CachedTransform.forward = bulletData.Forward;

        // 让子弹保持水平
        // CachedTransform.forward = new Vector3(
        //     CachedTransform.forward.x,
        //     1,
        //     CachedTransform.forward.z);

        if (bulletData.EffectId > 0) {
            BulletEffectData bulletEffectData = new BulletEffectData (EntityExtension.GenerateSerialId (), bulletData.EffectId, Id);
            EntityExtension.ShowBulletEffect (typeof (BulletEffect), "BulletEffectGroup", bulletEffectData);
        }
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);

        CachedTransform.Translate (CachedTransform.forward * bulletData.Speed * elapseSeconds, Space.World);

        zeroSpeedAutoDestroyTimes += elapseSeconds;

        // 将超出边界的子弹隐藏
        if (PositionUtility.IsOutOfMapBoundary(CachedTransform.position)) {
            GameEntry.Entity.HideEntity(this.Id);
        }
        // 对于速度为0的子弹，在0.5秒后自动销毁 
        else if (bulletData.Speed == 0) {
            if (zeroSpeedAutoDestroyTimes >= 0.5f) {
                GameEntry.Entity.HideEntity(this.Id);
            }
        }
    }

    protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData) {
        base.OnAttached (childEntity, parentTransform, userData);

        if (childEntity is BulletEffect) {
            bulletEffect = (BulletEffect) childEntity;
            bulletEffect.SetBulletData(this.bulletData);
            leftEffectTimes = bulletEffect.EffectTimes;
        }
    }

    /// <summary>
    /// 减少可产生效果次数
    /// </summary>
    /// <returns>如果返回true，代表该子弹不能继续产生效果，需要隐藏</returns>
    public bool SubEffectTimes () {
        leftEffectTimes--;

        if (leftEffectTimes <= 0) {
            return true;
        }

        return false;
    }
}