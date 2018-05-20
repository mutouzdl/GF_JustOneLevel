using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This is the PocketRPG Animation Controller... It is necessary to run PocketRPG trails
// THIS REPLACES METHODS LIKE ANIMATION.PLAY AND ANIMATION.CROSSFADE... YOU CANNOT USE THOSE IN CONJUNCTION WITH THESE TRAILS UNLESS YOU ARE HAPPY WITH FRAMERATE DEPENDANT ANIMATION
// PocketRPG trails run faster than the framerate... (default is 300 frames a second)... that is how they are so smooth (30fps trails are rather jerky)
// This code was written by Evan Greenwood (of Free Lives) and used in the game PocketRPG by Tasty Poison Games.
// But you can use this how you please... Make great games... that's what this is about.
// This code is provided as is. Please discuss this on the Unity Forums if you need help.

[RequireComponent(typeof(Animation))]
[AddComponentMenu("PocketRPG/Animation Controller")]
public class AnimationController : MonoBehaviour
{

	private AnimationState currentState;
	private float currentStateTime = 0;
	private List<AnimationState> fadingStates;	
	private float animationFadeTime = 0.15f;	
	protected List<WeaponTrail> trails;
	protected float t = 0.033f;
	protected float m = 0;
	protected Vector3 lastEulerAngles = Vector3.zero;
	protected Vector3 lastPosition = Vector3.zero;
	protected Vector3 eulerAngles = Vector3.zero;
	protected Vector3 position = Vector3.zero;
	private float tempT = 0;
	public bool gatherDeltaTimeAutomatically = true; // ** You may want to set the deltaTime yourself for personal slow motion effects
	//
	protected float animationIncrement = 0.003f; // ** This sets the number of time the controller samples the animation for the weapon trails
	//
	//
	void Awake ()
	{
		trails = new List<WeaponTrail> ();
		fadingStates = new List<AnimationState> ();
		currentState = null;
		GetComponent<Animation>().Stop ();
		lastPosition = transform.position;
		lastEulerAngles = transform.eulerAngles;
		//
		GetComponent<Animation>().playAutomatically = false; // ** The Animation component must not be conflicting with the AnimationController
		GetComponent<Animation>().Stop(); 
	}
	void Start(){
		GetComponent<Animation>().Stop(); // ** The Animation component must not be conflicting with the AnimationController
	}
	//
	public void SetDeltaTime (float deltaTime)
	{
		t = deltaTime; // ** This is for forcing the deltaTime of the Animation Controller for personal slow motion effects
	}
	public void SetAnimationSampleRate(int samplesPerSecond){
		animationIncrement = 1f / samplesPerSecond;
	}
	//
	protected virtual void LateUpdate ()
	{
		if (gatherDeltaTimeAutomatically){
			t = Mathf.Clamp (Time.deltaTime, 0, 0.066f);
		}
		// 
		RunAnimations ();
		// 		
	}	
	//
	public void AddTrail (WeaponTrail trail)
	{
		trails.Add (trail);
	}
	//
	public void PlayAnimation (AnimationState state)
	{
		// ** This function is exactly like the Unity Animation.Play() method
		//
		for (int i = 0; i < fadingStates.Count; i++) {
			fadingStates[i].weight = 0;
			fadingStates[i].enabled = false;
		}
		fadingStates.Clear ();
		//
		if (currentState != null) {
			currentState.enabled = false;
			currentState.weight = 0;
		}
		currentState = state;
		currentState.weight = 1;
		currentState.time = currentStateTime = 0;
		currentState.enabled = true;		
		//
	}
	//	
	public void CrossfadeAnimation (AnimationState state, float fadeTime)
	{
		// ** This function is exactly like the Unity Animation.Crossfade() method
		//
		if (currentState == state)
			return;
		animationFadeTime = fadeTime;
		
		for (int i = 0; i < fadingStates.Count; i++) {
			if (state.name == fadingStates[i].name) {
				fadingStates.RemoveAt (i);
				if (currentState != null)
					fadingStates.Add (currentState);
				currentState = state;
				return;
			}
		}
		//
		if (currentState != null)
			fadingStates.Add (currentState);
		//
		currentState = state;
		currentState.weight = 0;
		currentState.time = currentStateTime = 0;
		currentState.enabled = true;
		//
	}
	bool FadeOutAnimation (AnimationState state, float aI)
	{
		// ** This is called every sample to ease out a fading animation 
		//
		state.weight -= aI / animationFadeTime;
		state.time += aI * state.speed;
		// 		
		if (state.weight <= 0) {
			state.enabled = false;
			return true;
		}
		return false;
	}
	//
	void FadeInCurrentState (float aI)
	{
		// ** This is called every sample to ease in the current animation 
		//
		currentState.weight = Mathf.Clamp (currentState.weight + aI / animationFadeTime, 0, 1);
		currentStateTime += aI * currentState.speed;
		currentState.time = currentStateTime;
	}
	//
	void RunAnimations ()
	{
		//
		if (t > 0) {
			
			// ** The Animation Controller also turns the character
			// ** This is very important for smooth trails... Otherwise each frame the trail will jump to wherever the new rotation causes it to be
			// ** This could be taken further and make the animation controller move the character as well... You might want to do this if you have a character that moves very quickly
			//    
			eulerAngles = transform.eulerAngles;
			position = transform.position;
			//
			//
			while (tempT < t) {
				
				//
				// ** This loop runs slowly through the animation at very small increments
				// ** a bit expensive, but necessary to achieve smoother than framerate weapon trails
				//
				tempT += animationIncrement;
				// ** steps forward by a small increment
				//
				// ** Manually adjusts weights of all the animations that are applied 
				//
				for (int i = 0; i < fadingStates.Count; i++) {
					if (FadeOutAnimation (fadingStates[i], animationIncrement)) {
						fadingStates.RemoveAt (i);
						i--;
					}
				}
				if (currentState != null)
					FadeInCurrentState (animationIncrement);
				//
				m = tempT / t;
				transform.eulerAngles = new Vector3(Mathf.LerpAngle(lastEulerAngles.x, eulerAngles.x, m),Mathf.LerpAngle(lastEulerAngles.y, eulerAngles.y, m),Mathf.LerpAngle(lastEulerAngles.z, eulerAngles.z, m));
				transform.position = Vector3.Lerp(lastPosition, position, m);
				//
				// ** Samples the animation at that moment
				//
				GetComponent<Animation>().Sample ();
				//
				// ** Adds the information to the WeaponTrail
				//
				for (int j = 0; j < trails.Count; j++) {
					if (trails[j].time > 0) {
						trails[j].Itterate (Time.time - t + tempT);
					} else {
						trails[j].ClearTrail ();
					}
				}
			}
			//
			// ** End of loop
			//
			tempT -= t;
			//
			// ** Sets the position and rotation to what they were originally
			transform.position = position;
			transform.eulerAngles = eulerAngles;
			lastPosition = position;
			lastEulerAngles = eulerAngles;
			//
			// ** Finally creates the meshes for the WeaponTrails (one per frame)
			//
			for (int j = 0; j < trails.Count; j++) {
				if (trails[j].time > 0) {
					trails[j].UpdateTrail (Time.time, t);
				}
			}
		}
	}
	
}
