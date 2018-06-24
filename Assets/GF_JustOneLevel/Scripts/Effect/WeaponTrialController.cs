using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrialController {
	private WeaponTrail weaponTrail = null;

	private float t = 0.033f;
	private float animationIncrement = 0.003f;
	private float tempT = 0;

	public WeaponTrialController (WeaponTrail weaponTrail) {
		this.weaponTrail = weaponTrail;
	}

	public void Reset () {
        weaponTrail.SetTime (0.0f, 0, 1);
	}

	public void Update () {
		if (weaponTrail == null) {
			return;
		}
		
		t = Mathf.Clamp (Time.deltaTime, 0, 0.066f);

		if (t > 0) {
			while (tempT < t) {
				tempT += animationIncrement;

				if (weaponTrail.time > 0) {
					weaponTrail.Itterate (Time.time - t + tempT);
				} else {
					weaponTrail.ClearTrail ();
				}
			}

			tempT -= t;

			if (weaponTrail.time > 0) {
				weaponTrail.UpdateTrail (Time.time, t);
			}
		}
	}

    /// <summary>
    /// 播放拖尾效果
    /// </summary>
    public void PlayTrailEffect () {
        if (weaponTrail == null) {
            return;
        }

        //设置拖尾时长  
        weaponTrail.SetTime (2.0f, 0.0f, 1.0f);

        //开始进行拖尾  
        weaponTrail.StartTrail (0.5f, 0.4f);
    }

    /// <summary>
    /// 清除拖尾效果
    /// </summary>
    public void ClearTrialEffect () {
        if (weaponTrail == null) {
            return;
        }

        //清除拖尾  
        weaponTrail.ClearTrail ();
    }

}