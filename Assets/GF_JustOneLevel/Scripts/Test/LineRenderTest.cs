using System.Collections;
using UnityEngine;

public class LineRenderTest : MonoBehaviour {
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    Ray shootRay = new Ray (); // A ray from the gun end forwards.
    RaycastHit shootHit; // A raycast hit to get information about what was hit.
    LineRenderer lineRenderer = null;

    void Start () {
        // 添加线
        lineRenderer = gameObject.AddComponent<LineRenderer> ();
        lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.2f;

        // 线的样式
        float alpha = 1.0f;
        Gradient gradient = new Gradient ();
        gradient.SetKeys (
            new GradientColorKey[] { new GradientColorKey (c1, 0.0f), new GradientColorKey (c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey (alpha, 0.0f), new GradientAlphaKey (alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;

        Refresh ();
    }

    /// <summary>
    /// 刷新，相当于你的泡泡龙的手指操作，到时候你得自己实现
    /// </summary>
    public void Refresh () {
        // 碰撞射线初始坐标为手指所在位置，初始方向为手指方向（这里就是那个白色的球）
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // 发出第一条线（一条线2个坐标）
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition (0, transform.position);
        lineRenderer.SetPosition (1, transform.forward * 100); // 乘以100只是随便设的，让线足够长
    }

    void Update () {
        // shootRay是碰撞射线，用于碰撞检测。1000也是随便设的，让碰撞检测的射线足够长
        if (Physics.Raycast (shootRay, out shootHit, 1000)) {
            Debug.Log ("碰撞！" + shootHit.point);

            // 因为线的长度很长（上面乘以了100），碰撞的时候需要将线最后一个坐标重新设置成碰撞点所在坐标
            int preIndex = lineRenderer.positionCount - 1;
            lineRenderer.SetPosition (preIndex, shootHit.point);

            // 避免转角太多，做个限制，测试用。这个以后你自己处理咯
            if (lineRenderer.positionCount > 3) {
                return;
            }

            // 根据线的方向向量和法线，得到反射向量
            Vector3 reflectVector = Vector3.Reflect(shootRay.direction, shootHit.normal);

            // 增加一个新的点，这样线就会往折射方向延申
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition (preIndex + 1, reflectVector * 100);

            // 重新设置碰撞射线的起点和方向
            shootRay.origin = shootHit.point;
            shootRay.direction = reflectVector;
        }
    }
}