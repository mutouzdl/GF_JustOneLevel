#pragma strict
public var lighting:float = 1;
public var lightPower:Light;
public var flashFlg:boolean = false;
public var flashTimer:float = 0.3;

function Start () {
	lightPower = this.GetComponent.<Light>();
	
	if( flashFlg ){
		lightPower.enabled = false;
		yield WaitForSeconds( flashTimer );
		lightPower.enabled = true;
	}
}

function Update () {
	
	if( lightPower.intensity > 0 && lightPower.enabled)lightPower.intensity -= lighting * Time.deltaTime;
	
}