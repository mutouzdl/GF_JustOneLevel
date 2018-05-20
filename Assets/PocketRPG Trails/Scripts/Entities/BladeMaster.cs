using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This is the PocketRPG Blade Master class stripped down to just a bit of animation... it's just for an example of how to use the PocketRPG weapon trails.
// This code was written by Evan Greenwood (of Free Lives) and used in the game PocketRPG by Tasty Poison Games.
// But you can use this how you please... Make great games... that's what this is about.
// This code is provided as is. Please discuss this on the Unity Forums if you need help.

[AddComponentMenu("PocketRPG/Blade Master")]
public class BladeMaster : MonoBehaviour
{

	//  ---------------------------------------------------------------  
	#region Inspector Assigned	
	//
	public WeaponTrail leftSwipe;
	public WeaponTrail rightSwipe;
	//     
	#endregion
	//  ---------------------------------------------------------------  
	#region Temporary

	protected float t = 0.033f;
	#endregion
	//
	#region Internal
	protected AnimationController animationController;
	#endregion
	//  ---------------------------------------------------------------  
	#region Logic
	//
	protected float thinkTime = 0.35f;
	protected int thinkState = 0;
	//
	#endregion
	//  ---------------------------------------------------------------  
	#region Animation
	//
	private AnimationState animationAttack1Anticipation;
	private AnimationState animationAttack1;
	private AnimationState animationAttack2Anticipation;
	private AnimationState animationAttack2;
	private AnimationState animationAttack3;	
	private AnimationState animationWhirlwind;
	//
	private AnimationState animationIdle;
	private AnimationState animationRespawn;
	//
	protected float timeScale = 1; // This is here for personal time distortion... like freeze spells that slow enemies... (changing this affects the animation rate)
	protected float facingAngle = 0;
	//
	#endregion
	//
	
	#region Initialisation
	//
	protected void Awake ()
	{
		animationController = GetComponent<AnimationController> ();
		transform.position = Vector3.zero;
	}
	protected void Start ()
	{
		animationController.AddTrail (leftSwipe); // Adds the trails to the animationController which will run them
		animationController.AddTrail (rightSwipe);
		//
		Initialise ();
		//
		// This is just making him jump at the start... normally you would just hit PlayAnimation(idle)...		
		//
		thinkTime = 1.5f;
		animationController.PlayAnimation (animationRespawn);
		leftSwipe.SetTime (2.1f, 0, 1);
		rightSwipe.SetTime (2.1f, 0, 1);			
		//
	}

	protected void Initialise ()
	{
		// The Animation Controller feeds on AnimationStates. You've got to assign your animations to variables so that you can call them from the controller
		//    
		GetComponent<Animation>()["Attack1"].speed = 2.0f;
		GetComponent<Animation>()["Attack2"].speed = 2.0f;		
		//
		//
		animationAttack1Anticipation = GetComponent<Animation>()["Attack1Anticipation"];
		animationAttack1 = GetComponent<Animation>()["Attack1"];
		//
		animationAttack2Anticipation = GetComponent<Animation>()["Attack2Anticipation"];
		animationAttack2 = GetComponent<Animation>()["Attack2"];
		animationAttack3 = GetComponent<Animation>()["WhirlwindAttack"];
		//		
		animationWhirlwind = GetComponent<Animation>()["Whirlwind"];		
		animationIdle = GetComponent<Animation>()["Idle"];		
		animationIdle.speed = 0.4f;		
		animationRespawn = GetComponent<Animation>()["Resurection"];
		animationRespawn.speed = 0.8f;
		animationAttack3.speed = 0.8f;
		//		
		leftSwipe.SetTime (0.0f, 0, 1);
		rightSwipe.SetTime (0.0f, 0, 1);
		//		
	}
	//
	#endregion
	// ------------------------------------------------------------------------------------------------------------------- 
	#region Update
	//
	protected void Update ()
	{
		t = Mathf.Clamp (Time.deltaTime * timeScale, 0, 0.066f);
		//
		animationController.SetDeltaTime (t); // Sets the delta time that the animationController uses.
		//
		//
		//  This is just some sample code to show you how you can use the animation controller component along with the trails 
		//
		thinkTime -= t;
		if (thinkTime < 0) {
			switch (thinkState) {
			case 0:
				animationController.CrossfadeAnimation (animationIdle, 0.2f);
				thinkState++;
				thinkTime = 1.5f;
				break;
			case 1:
				// START ATTACK 1
				animationController.CrossfadeAnimation (animationAttack1Anticipation, 0.1f);
				thinkState++;
				thinkTime = 0.3f;
				facingAngle = -180 + Random.value * 360;
				
				break;
			case 2:
				animationController.PlayAnimation (animationAttack1);
				thinkTime = 0.2f;
				thinkState++;
				rightSwipe.StartTrail(0.5f, 0.4f); // Fades the trail in				
				facingAngle += -40 + Random.value * 80;			
				break;
			case 3:				
				thinkState++;
				thinkTime = 0.3f;
				rightSwipe.FadeOut(0.5f); // Fades the trail out
				break;
			case 4:
				// BACK TO IDLE
				animationController.CrossfadeAnimation (animationIdle, 0.2f);
				thinkState++;
				thinkTime = 0.3f;	
				rightSwipe.ClearTrail(); // Forces the trail to clear			
				break;
			case 5:
				// START ATTACK 2
				animationController.CrossfadeAnimation (animationAttack2Anticipation, 0.1f);
				thinkState++;
				thinkTime = 0.3f;
				facingAngle = -180 + Random.value *360;
			
				break;
			case 6:
				animationController.PlayAnimation (animationAttack2);
				thinkState++;
				thinkTime = 0.2f;
				leftSwipe.StartTrail(0.5f, 0.4f);  // Fades the trail in				
				facingAngle += -40 + Random.value * 80;
				
				break;
			case 7:				
				thinkState++;
				thinkTime = 0.3f;
				leftSwipe.FadeOut(0.5f); // Fades the trail out
				break;
			case 8:
				// BACK TO IDLE
				animationController.CrossfadeAnimation (animationIdle, 0.2f);
				thinkState++;
				thinkTime = 0.3f;
				leftSwipe.ClearTrail(); // Forces the trail to clear			
				break;
			case 9:
				// START THE SPIN ATTACK
				animationController.CrossfadeAnimation (animationAttack1Anticipation, 0.1f);
				thinkState++;
				thinkTime = 0.3f;
				facingAngle = -180 + Random.value * 360;
			
				break;
			case 10:
				animationController.CrossfadeAnimation (animationWhirlwind, 0.2f);
				thinkState++;
				thinkTime = 2.8f;
				leftSwipe.StartTrail(0.9f, 1.4f); // Fades both trais in				
				rightSwipe.StartTrail(0.9f, 1.4f);					
				break;
			case 11:
				// Checking for a specific place in the animation from which to start the next animation
				//
				if (Mathf.Repeat(animationWhirlwind.normalizedTime, 1) < 0.93f + t*1f && Mathf.Repeat(animationWhirlwind.normalizedTime, 1) > 0.93f-t*1.2f){				
					animationController.CrossfadeAnimation (animationAttack3, 0.05f* animationWhirlwind.length);
					thinkState++;			
					thinkTime = 0.6f;
				}
				break;
			case 12:				
				thinkState++;
				leftSwipe.FadeOut(0.4f); // Fades both trails out			
				rightSwipe.FadeOut(0.4f);
				thinkTime = 0.6f;
				break;
			case 13:
				// BACK TO IDLE (for a second)
				animationController.CrossfadeAnimation (animationIdle, 0.2f);
				thinkState++;
				thinkTime = 1f;
				leftSwipe.ClearTrail();  // Forces both trails to clear		
				rightSwipe.ClearTrail();
				break;
			default:
				thinkState = 0;
				break;
			}
		}
		//
		// ** THIS IS A REALLY TERRIBLE EXAMPLE OF THE CHARACTER MOVING ... (It moves forward when executing a attack)
		if (thinkState == 3  || thinkState == 7 ){
			transform.position += transform.TransformDirection( Vector3.forward) * t*3;
		} else if (thinkState == 11 || thinkState == 10){
			transform.position += transform.TransformDirection( Vector3.forward) * t*0.5f;
		}
		//
		// This rotates the character a based on "facingAngle". It's an experiment to show that the animationController works
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle( transform.eulerAngles.y, facingAngle, 12 *t), transform.eulerAngles.z);
	}
	#endregion	
}
