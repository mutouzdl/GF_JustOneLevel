using System.Collections;
using GameFramework;
using UnityEngine;

/// <summary>
/// 带有特殊效果的泉水
/// </summary>
public class MagicWater : MonoBehaviour {
	[SerializeField]
	private float addHPPercent = 1f;
	[SerializeField]
	private float delay = 3f;
	[SerializeField]
	private int addAtk = 0;
	[SerializeField]
	private int addGold = 0;
	[SerializeField]
	private int createCloneHeroTypeID = 0;

	private float nextMagicTime = 0f;
	private bool isMagicable = false;

	void Update () {
		if (nextMagicTime == 0) {
			nextMagicTime = Time.time + delay;
		}

		if (Time.time >= nextMagicTime) {
			isMagicable = true;
			nextMagicTime = Time.time + delay;
		} else {
			isMagicable = false;
		}
	}

	void OnTriggerEnter (Collider other) { }

	void OnTriggerStay (Collider other) {
		if (isMagicable == true) {
			Hero hero = other.gameObject.GetComponent<Hero> ();
			if (hero != null) {
				// 改变金币
				if (addGold != 0) {
					int gold = GameEntry.Setting.GetInt (Constant.Player.Gold);
					if (addGold < 0 && gold < Mathf.Abs (addGold)) {
						return;
					}

					GameEntry.Setting.SetInt (Constant.Player.Gold, gold + addGold);
					GameEntry.Event.Fire (this, ReferencePool.Acquire<RefreshGoldEventArgs> ());
				}

				// 改变血量
				if (addHPPercent != 0) {
					hero.OnDamage (-addHPPercent, true);
				}

				// 改变攻击
				if (addAtk != 0) {
					hero.HeroData.PowerUp (addAtk, 0, 0);
				}

				// 生成克隆英雄
				if (createCloneHeroTypeID != 0) {
					MonsterData monsterData = new MonsterData (
						EntityExtension.GenerateSerialId (), createCloneHeroTypeID, CampType.CloneHero, 0);
					monsterData.Position = transform.position;

					// 避免克隆英雄太弱，根据玩家英雄实时最大血量调整克隆英雄属性
					float powerPercent = hero.HeroData.MaxHP / monsterData.MaxHP / 100f;
					monsterData.AjustPower(powerPercent);

					EntityExtension.ShowMonster (typeof (Monster), "MonsterGroup", monsterData);

				}
			}
		}
	}

	void OnTriggerExit (Collider other) { }
}