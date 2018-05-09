using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 能量条
/// </summary>
public class PowerBar : Entity {
	[SerializeField]
	private PowerBarData m_PowerBarData = null;

	protected override void OnInit (object userData) {
		base.OnInit (userData);
	}

	protected override void OnShow (object userData) {
		base.OnShow (userData);

		m_PowerBarData = userData as PowerBarData;
		if (m_PowerBarData == null) {
			Log.Error ("PowerBar data is invalid.");
			return;
		}

		GameEntry.Entity.AttachEntity (this.Id, m_PowerBarData.OwnerId, "HPBarPosition");
	}

	protected override void OnAttachTo (EntityLogic parentEntity, Transform parentTransform, object userData) {
		base.OnAttachTo (parentEntity, parentTransform, userData);

		Name = string.Format ("PowerBar of {0}", parentEntity.Name);
		CachedTransform.localPosition = Vector3.zero;
		CachedTransform.localScale = Vector3.one;
		CachedTransform.localRotation = new Quaternion (0, 0, 0, 0);
	}

	/// <summary>
	/// 更新能量数据
	/// </summary>
	/// <param name="curPower"></param>
	/// <param name="maxPower"></param>
	public void UpdatePower (int curPower, int maxPower) {
		float value = curPower / (float)maxPower;

		CachedTransform.localScale = new Vector3 (
			CachedTransform.localScale.x,
			value >= 0 ? value : 0f,
			CachedTransform.localScale.z);
	}
}