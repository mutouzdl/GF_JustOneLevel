using UnityEngine;
using System.Collections;
using GameFramework;

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


	private float nextMagicTime = 0f;
	private bool isMagicable = false;
	
	void Update () {
		if (nextMagicTime == 0) {
			nextMagicTime= Time.time + delay;
		}

		if (Time.time >= nextMagicTime) {
			isMagicable = true;
			nextMagicTime= Time.time + delay;
		} else {
			isMagicable = false;
		}
	}	

	void OnTriggerEnter(Collider other) {
	}
	
	void OnTriggerStay(Collider other) {
		if (isMagicable == true) {
			Log.Info("碰撞！");
			Hero hero = other.gameObject.GetComponent<Hero>();	
			if(hero != null) {
				if (addHPPercent != 0) {
					hero.OnDamage(-addHPPercent, true);
				}

				if(addAtk != 0) {
					hero.HeroData.PowerUp(addAtk, 0, 0);
				}
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
	}
}
