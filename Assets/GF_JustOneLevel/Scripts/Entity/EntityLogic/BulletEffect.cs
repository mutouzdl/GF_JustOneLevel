using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 子弹效果类
/// </summary>
public class BulletEffect : Entity {
    private const string AttachPoint = "BulletEffectPoint";

    private BulletData bulletData = null;
    private BulletEffectData bulletEffectData = null;
    private Transform parentTransform = null;
    private int parentEntityId = 0;
    private bool isStop = false;

    /// <summary>
    /// 用于碰撞检测的射线
    /// </summary>
    /// <returns></returns>
    private Ray shootRay = new Ray ();
    /// <summary>
    /// 射线碰撞信息
    /// </summary>
    private RaycastHit shootHit;
    /// <summary>
    /// 射线
    /// </summary>
    private LineRenderer lineRenderer = null;

    protected override void OnInit (object userData) {
        base.OnInit (userData);
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        bulletEffectData = userData as BulletEffectData;
        if (bulletEffectData == null) {
            Log.Error ("BulletEffect data is invalid.");
            return;
        }
        
        isStop = false;
        GameEntry.Entity.AttachEntity (Entity, bulletEffectData.OwnerId, AttachPoint, bulletEffectData);
    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (bulletEffectData.Type == (int) BulletEffectType.射线) {
            RayEffectUpdate(elapseSeconds, realElapseSeconds);
        }
    }

    protected override void OnAttachTo (EntityLogic parentEntity, Transform parentTransform, object userData) {
        base.OnAttachTo (parentEntity, parentTransform, userData);

        Name = string.Format ("BulletEffect of {0}", parentEntity.Name);
        CachedTransform.localPosition = Vector3.zero;
        this.parentTransform = parentTransform;
        this.parentEntityId = parentEntity.Entity.Id;
        
        if (bulletEffectData.Type == (int) BulletEffectType.射线) {
            InitRayEffect ();

            RefreshRayEffect ();
        }
    }

    protected override void OnHide (object userData) {
        base.OnHide (userData);
    }

    /// <summary>
    /// 初始化射线特效
    /// </summary>
    private void InitRayEffect () {
        // 添加线
        lineRenderer = gameObject.GetOrAddComponent<LineRenderer> ();
        // lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.useWorldSpace = true;

        // 线的样式
        // Color c1 = Color.yellow;
        // Color c2 = Color.red;
        // float alpha = 1.0f;

        // Gradient gradient = new Gradient ();
        // gradient.SetKeys (
        //     new GradientColorKey[] { new GradientColorKey (c1, 0.0f), new GradientColorKey (c2, 1.0f) },
        //     new GradientAlphaKey[] { new GradientAlphaKey (alpha, 0.0f), new GradientAlphaKey (alpha, 1.0f) }
        // );

        // lineRenderer.colorGradient = gradient;
    }

    /// <summary>
    /// 刷新射线
    /// </summary>
    private void RefreshRayEffect () {
        shootRay.origin = CachedTransform.position;
        shootRay.direction = parentTransform.forward;

        // 发出第一条线（一条线2个坐标）
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition (0, CachedTransform.position);
        lineRenderer.SetPosition (1, parentTransform.forward * 100);
    }

    private float hideTime = 0;
    /// <summary>
    /// 射线逻辑
    /// </summary>
    /// <param name="elapseSeconds"></param>
    /// <param name="realElapseSeconds"></param>
    private void RayEffectUpdate (float elapseSeconds, float realElapseSeconds) {
        if (isStop) {
            if (Time.time >= hideTime) {
                GameEntry.Entity.HideEntity(parentEntityId);
            }
            return;
        }
        // shootRay是碰撞射线，用于碰撞检测。1000是随便设的，让碰撞检测的射线足够长
        if (Physics.Raycast (shootRay, out shootHit, 1000)) {
            FightEntity entity = shootHit.collider.GetComponent<FightEntity>();
            if (entity != null) {
                entity.ApplyDamage(bulletData.Attack);
            }
            else {
                isStop = true;
                hideTime = Time.time + 0.1f;
                return;
            }

            // 因为线的长度很长（上面乘以了100），碰撞的时候需要将线最后一个坐标重新设置成碰撞点所在坐标
            int preIndex = lineRenderer.positionCount - 1;
            lineRenderer.SetPosition (preIndex, shootHit.point);

            // 达到最大效果次数，停止折射
            if (lineRenderer.positionCount > bulletEffectData.EffectTimes) {
                isStop = true;
                hideTime = Time.time + 0.1f;
                return;
            }

            // 根据线的方向向量和法线，得到反射向量
            Vector3 reflectVector = Vector3.Reflect (shootRay.direction, shootHit.normal);

            // 增加一个新的点，这样线就会往折射方向延申
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition (preIndex + 1, reflectVector * 100);

            // 重新设置碰撞射线的起点和方向
            shootRay.origin = shootHit.point;
            shootRay.direction = reflectVector;
        }
    }

    public void SetBulletData(BulletData bulletData) {
        this.bulletData = bulletData;
    }

    /// <summary>
    /// 可产生效果次数
    /// </summary>
    /// <returns></returns>
    public int EffectTimes {
        get {
            return bulletEffectData.EffectTimes;
        }
    }
}