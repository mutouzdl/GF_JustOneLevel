using System.Collections;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 带有特殊效果的泉水
/// </summary>
public class MagicWater : Entity {
	private EParticle particle = null;
	private float nextMagicTime = 0f;
	private bool isMagicable = false;

	private MagicWaterData magicWaterData = null;

	protected override void OnInit (object userData) {
		base.OnInit (userData);

	}

	protected override void OnShow (object userData) {
		base.OnShow (userData);

		magicWaterData = userData as MagicWaterData;
		if (magicWaterData == null) {
			Log.Error ("MagicWaterData is invalid.");
			return;
		}
	}

	protected override void OnUpdate (float elapseSeconds, float realElapseSeconds) {
		base.OnUpdate (elapseSeconds, realElapseSeconds);

		if (nextMagicTime == 0) {
			nextMagicTime = Time.time + magicWaterData.Delay;
		}

		if (Time.time >= nextMagicTime) {
			isMagicable = true;
			nextMagicTime = Time.time + magicWaterData.Delay;
		} else {
			isMagicable = false;
		}
	}

	protected override void OnAttached (EntityLogic childEntity, Transform parentTransform, object userData) {
		base.OnAttached (childEntity, parentTransform, userData);

		if (childEntity is EParticle) {
			particle = (EParticle) childEntity;
		}
	}

	void OnTriggerEnter (Collider other) {
		Hero hero = other.gameObject.GetComponent<Hero> ();
		if (hero != null) {
			ParticleData data = new ParticleData (EntityExtension.GenerateSerialId (), magicWaterData.ParticleTypeID, this.Id);
			EntityExtension.ShowParticle (typeof (EParticle), "ParticleGroup", data);
		}
	}

	void OnTriggerStay (Collider other) {
		if (isMagicable == true) {
			isMagicable = false;

			Hero hero = other.gameObject.GetComponent<Hero> ();
			if (hero != null) {
				// 改变金币
				if (magicWaterData.AddGold != 0) {
					int gold = PlayerData.Gold;
					if (magicWaterData.AddGold < 0 && gold < Mathf.Abs (magicWaterData.AddGold)) {
						return;
					}

					GameEntry.Setting.SetInt (Constant.Player.Gold, gold + magicWaterData.AddGold);
					GameEntry.Event.Fire (this, ReferencePool.Acquire<RefreshGoldEventArgs> ());
				}

				// 改变血量
				if (magicWaterData.AddHPPercent != 0 &&
					Mathf.Abs ((int) (hero.HeroData.MaxHP * magicWaterData.AddHPPercent)) > Mathf.Abs (magicWaterData.AddHP)) {
					hero.OnDamage (-magicWaterData.AddHPPercent, true);
				} else if (magicWaterData.AddHP != 0) {
					hero.OnDamage (-magicWaterData.AddHP, true);
				}

				// 改变攻击
				if (magicWaterData.AddAtk != 0) {
					hero.PowerUpByAbsValue (0, 0, magicWaterData.AddAtk, 0);
				}

				// 生成克隆英雄
				if (magicWaterData.CreateCloneHeroTypeID != 0) {
					MonsterData monsterData = new MonsterData (
						EntityExtension.GenerateSerialId (), magicWaterData.CreateCloneHeroTypeID, CampType.CloneHero, 0);
					monsterData.Position = new Vector3 (transform.position.x, 0, transform.position.z);

					// 避免克隆英雄太弱，根据玩家英雄实时最大血量调整克隆英雄属性
					float powerPercent = hero.HeroData.MaxHP / monsterData.MaxHP / 100f + 1;
					monsterData.AjustPower (powerPercent);

					EntityExtension.ShowMonster (typeof (Monster), "MonsterGroup", monsterData);

				}
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (particle != null) {
			Hero hero = other.gameObject.GetComponent<Hero> ();

			if (hero != null) {
				GameEntry.Entity.HideEntity (particle.Id);
				particle = null;
			}
		}
	}
}