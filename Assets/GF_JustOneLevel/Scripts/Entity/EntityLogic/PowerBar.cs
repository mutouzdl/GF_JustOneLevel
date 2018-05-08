using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 能量条
/// </summary>
public class PowerBar : Entity {
	[SerializeField]
	private PowerBarData m_PowerBarData = null;

	protected override void OnInit (object userData)
	{
		base.OnInit (userData);
	}

	protected override void OnShow (object userData)
	{
		base.OnShow (userData);

		m_PowerBarData = userData as PowerBarData;
		if (m_PowerBarData == null) {
			Log.Error ("PowerBar data is invalid.");
			return;
		}

		GameEntry.Entity.AttachEntity (this, m_PowerBarData.OwnerId);
	}

	protected override void OnAttachTo (EntityLogic parentEntity, Transform parentTransform, object userData)
	{
		base.OnAttachTo (parentEntity, parentTransform, userData);

		Name = string.Format ("PowerBar of {0}", parentEntity.Name);
		CachedTransform.localPosition = Vector3.zero;
	}
}