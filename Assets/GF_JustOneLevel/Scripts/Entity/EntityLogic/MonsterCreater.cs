using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 怪物生成器
/// </summary>
public class MonsterCreater : Entity {
	[SerializeField]
	private MonsterCreaterData m_MonsterCreaterData = null;

	/// <summary>
	/// 已创建的怪物数量
	/// </summary>
	private int m_CreateNum = 0;

	private float m_TimeCounter = 0;

	protected override void OnInit (object userData) {
		base.OnInit (userData);
	}

	protected override void OnShow (object userData) {
		base.OnShow (userData);

		m_MonsterCreaterData = userData as MonsterCreaterData;
		if (m_MonsterCreaterData == null) {
			Log.Error ("MonsterCreater data is invalid.");
			return;
		}
	}

	protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
		base.OnUpdate (elapseSeconds, realElapseSeconds);

		m_TimeCounter += elapseSeconds;

		if (m_TimeCounter < m_MonsterCreaterData.Interval) {
			return;
		}

		m_TimeCounter = 0;

		// 创建怪物
		if (Utility.Random.GetRandom (100) < m_MonsterCreaterData.Probability) {
			for (int i = 0; i < m_MonsterCreaterData.PerNum; i++) {
				MonsterData monsterData = new MonsterData (
					EntityExtension.GenerateSerialId (), m_MonsterCreaterData.TypeId, CampType.Enemy, m_MonsterCreaterData.MonsterPrize);
				monsterData.Position = new Vector3 (Utility.Random.GetRandom (5, 25), 0, Utility.Random.GetRandom (5, 25));
				EntityExtension.ShowMonster (typeof (Monster), "MonsterGroup", monsterData);

				m_CreateNum++;
			}

			// 达到最大创建数量，销毁生成器
			if (m_CreateNum >= m_MonsterCreaterData.MaxNum) {
				Log.Info ("m_CreateNum:" + m_CreateNum);
				Log.Info ("m_MonsterCreaterData.MaxNum:" + m_MonsterCreaterData.MaxNum);
				GameEntry.Entity.HideEntity (this.Id);
			}
		}
	}
}