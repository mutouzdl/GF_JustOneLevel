using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTest : MonoBehaviour {
	public WeaponTrail myTrail;

	protected void Awake () {
		transform.position = Vector3.zero;
	}

	protected void Start () {
		myTrail.SetTime (2.1f, 0, 1);
	}

	private float t = 0.033f;
	private float animationIncrement = 0.003f;
	private float tempT = 0;
	void LateUpdate () {
		t = Mathf.Clamp (Time.deltaTime, 0, 0.066f);

		if (t > 0) {
			while (tempT < t) {
				tempT += animationIncrement;

				if (myTrail.time > 0) {
					myTrail.Itterate (Time.time - t + tempT);
				} else {
					myTrail.ClearTrail ();
				}
			}

			tempT -= t;

			if (myTrail.time > 0) {
				myTrail.UpdateTrail (Time.time, t);
			}
		}
	}

	public void heroAttack () {
		//设置拖尾时长  
		myTrail.SetTime (2.0f, 0.0f, 1.0f);
		//开始进行拖尾  
		myTrail.StartTrail (0.5f, 0.4f);
	}

	public void heroIdle () {
		//清除拖尾  
		myTrail.ClearTrail ();
	}
}