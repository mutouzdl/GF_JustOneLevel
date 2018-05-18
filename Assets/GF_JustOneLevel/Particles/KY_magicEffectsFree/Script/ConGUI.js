#pragma strict
public var cameraTrs:Transform;
public var rotSpeed:int = 20;
public var effectObj:GameObject[];
private var arrayNo:int = 0;

private var nowEffectObj:GameObject;
private var cameraState:String[] = ["Camera move" ,"Camera stop"];
private var cameraRotCon:int = 1;

function Start () {

}

function Update () {
	if( cameraRotCon == 1)cameraTrs.Rotate(0 ,rotSpeed * Time.deltaTime ,0);
}

function  OnGUI(){
		
	if (GUI.Button (Rect(20, 0, 30, 30), "←")) {//return
		arrayNo --;
		if(arrayNo < 0)arrayNo = effectObj.Length -1;
		effectOn();
	}
	
	if (GUI.Button (Rect(50, 0, 200, 30), effectObj[ arrayNo ].name )) {
		effectOn();
	}
	
	if (GUI.Button (Rect(250, 0, 30, 30), "→")) {//next
		arrayNo ++;
		if(arrayNo >= effectObj.Length)arrayNo = 0;
		effectOn();
	}
	
	if (GUI.Button (Rect(300, 0, 200, 30), cameraState[ cameraRotCon ] )) {
		if( cameraRotCon == 1){
			cameraRotCon = 0;
		}else{
			cameraRotCon = 1;
		}
	}
}

function effectOn(){
	if(nowEffectObj != null)Destroy( nowEffectObj );
	nowEffectObj = Instantiate(effectObj[ arrayNo ] );
}