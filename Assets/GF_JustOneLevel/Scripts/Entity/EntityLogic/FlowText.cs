using DG.Tweening;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 漂浮文字
/// </summary>
public class FlowText : Entity {
	[SerializeField]
	private FlowTextData flowTextData = null;

	private TextMesh flowText = null;
	private float hideTime = 0;

	protected override void OnInit (object userData) {
		base.OnInit (userData);

		flowText = FindObjectOfType<TextMesh> ();
	}

	protected override void OnShow (object userData) {
		base.OnShow (userData);

		flowTextData = userData as FlowTextData;
		if (flowTextData == null) {
			Log.Error ("FlowHPText data is invalid.");
			return;
		}

		// GameEntry.Entity.AttachEntity (this.Id, flowTextData.OwnerId);

		flowText.text = flowTextData.Text;
		flowText.color = flowTextData.Color.ToColor ();

		hideTime = Time.time + 1;

		CachedTransform.localScale = Vector3.one / 2;

        Sequence seq = DOTween.Sequence();
		
        seq.Append(CachedTransform.DOScale(1, 0.4f))
            .Append(CachedTransform.DOBlendableLocalMoveBy(new Vector3(0.3f, 0.7f, 0), 0.3f))
            .OnComplete(()=> { GameEntry.Entity.HideEntity(this.Id); });
	}

	protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
		base.OnUpdate (elapseSeconds, realElapseSeconds);

		if (Time.time >= hideTime) {
			
		}
	}

	protected override void OnAttachTo (EntityLogic parentEntity, Transform parentTransform, object userData) {
		base.OnAttachTo (parentEntity, parentTransform, userData);

		Name = string.Format ("FlowText of {0}", parentEntity.Name);
		CachedTransform.localPosition = new Vector3(0.3f, 1, 0);
		CachedTransform.localScale = Vector3.one;
		CachedTransform.localRotation = new Quaternion (0, 0, 0, 0);

	}
}