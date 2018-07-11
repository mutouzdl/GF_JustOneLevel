using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 怪物生成器
/// </summary>
public class MonsterCreater : Entity {
	[SerializeField]
	private MonsterCreaterData monsterCreaterData = null;

	/// <summary>
	/// 已创建的怪物数量
	/// </summary>
	private int createNum = 0;

	private float timeCounter = 0;
	/// <summary>
	/// 过去的时间
	/// </summary>
	private float pastTime = 0;

	protected override void OnInit (object userData) {
		base.OnInit (userData);
	}

	protected override void OnShow (object userData) {
		base.OnShow (userData);

		monsterCreaterData = userData as MonsterCreaterData;
		if (monsterCreaterData == null) {
			Log.Error ("MonsterCreater data is invalid.");
			return;
		}

		createNum = 0;
		timeCounter = 0;
		pastTime = 0;
	}

	protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
		base.OnUpdate (elapseSeconds, realElapseSeconds);

		timeCounter += elapseSeconds;
		pastTime += elapseSeconds;

		if (pastTime < monsterCreaterData.StartTime) {
			return;
		}

		// 第一个创建的怪物跳过创建间隔
		if (createNum > 0 && timeCounter < monsterCreaterData.Interval) {
			return;
		}

		timeCounter = 0;

		// 创建怪物
		if (Utility.Random.GetRandom (100) < monsterCreaterData.Probability) {
			for (int i = 0; i < monsterCreaterData.PerNum; i++) {
				CampType camp = CampType.Enemy;

				MonsterData monsterData = new MonsterData (
					EntityExtension.GenerateSerialId (), monsterCreaterData.MonsterTypeId, camp, monsterCreaterData.MonsterPrize);
				monsterData.Position = new Vector3 (Utility.Random.GetRandom (5, 25), 0, Utility.Random.GetRandom (5, 25));

				// 调整怪物属性
				monsterData.AjustPower(monsterCreaterData.PowerPercent);
				monsterData.ChangeName(monsterCreaterData.Name);
				EntityExtension.ShowMonster (typeof (Monster), "MonsterGroup", monsterData);

				createNum++;
			}

			// 达到最大创建数量，销毁生成器
			if (createNum >= monsterCreaterData.MaxNum) {
				GameEntry.Entity.HideEntity (this.Id);
			}
		}
	}
}