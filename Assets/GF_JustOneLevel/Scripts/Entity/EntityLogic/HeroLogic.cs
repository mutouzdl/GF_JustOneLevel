using GameFramework;
using UnityEngine;

public class HeroLogic : TargetableObject {
    [SerializeField]
    private HeroData m_heroData = null;

    private Vector3 m_TargetPosition = Vector3.zero;

    protected override void OnInit (object userData) {
        base.OnInit (userData);
    }

    protected override void OnShow (object userData) {
        base.OnShow (userData);

        m_heroData = userData as HeroData;
        if (m_heroData == null) {
            Log.Error ("My aircraft data is invalid.");
            return;
        }

    }

    protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
        base.OnUpdate (elapseSeconds, realElapseSeconds);
        if(Input.GetAxis("Horizontal") != 0) {
            /* 旋转镜头 */
			transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * 0.8f * m_heroData.RotateSpeed, 0));
		}
		if (Input.GetAxis ("Vertical") != 0) {
			/* 移动 */
			CachedTransform.position 
                += CachedTransform.forward * Input.GetAxis ("Vertical") * m_heroData.MoveSpeed * Time.deltaTime;

		} 
    }

    public override ImpactData GetImpactData () {
        return new ImpactData (m_heroData.Camp, m_heroData.HP, 0, m_heroData.Defense);
    }
}