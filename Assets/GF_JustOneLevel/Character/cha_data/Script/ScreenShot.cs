using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour {
	string format;
	
	// Use this for initialization
	void Start () {
		format = "yyyy-MM-dd-HH-mm-ss"; 
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			ScreenCapture.CaptureScreenshot(Application.dataPath + "/" + System.DateTime.Now.ToString(format) +".png");
		}
	}
}