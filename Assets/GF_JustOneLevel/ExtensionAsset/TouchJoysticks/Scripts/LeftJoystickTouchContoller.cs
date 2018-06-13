/*
about this script: 

this script is for a single joystick that is limited to the left-side half of the screen

if this joystick is not set to stay in a fixed position
 this script makes it appear and disappear within the left-side half of the screen where the screen was touched 

if a joystick is set to stay in a fixed position
 this script makes it appear and disappear if the user touches within the area of its background image (even if it is not currently visible)
 
this script also optionally keeps the joystick always visible
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeftJoystickTouchContoller : MonoBehaviour
{
    public Image leftJoystickBackgroundImage; // background image of the left joystick (the joystick's handle (knob) is a child of this image and moves along with it)
    public bool leftJoyStickAlwaysVisible = false; // value from left joystick that determines if the left joystick should be always visible or not

    private Image leftJoystickHandleImage; // handle (knob) image of the left joystick
    private LeftJoystick leftJoystick; // script component attached to the left joystick's background image
    private int leftSideFingerID = 0; // unique finger id for touches on the left-side half of the screen

    void Start()
    {

        if (leftJoystickBackgroundImage.GetComponent<LeftJoystick>() == null)
        {
            Debug.LogError("There is no LeftJoystick script attached to the Left Joystick game object.");
        }
        else
        {
            leftJoystick = leftJoystickBackgroundImage.GetComponent<LeftJoystick>(); // gets the left joystick script
            leftJoystickBackgroundImage.enabled = leftJoyStickAlwaysVisible; // sets left joystick background image to be always visible or not
        }

        if (leftJoystick.transform.GetChild(0).GetComponent<Image>() == null)
        {
            Debug.LogError("There is no left joystick handle image attached to this script.");
        }
        else
        {
            leftJoystickHandleImage = leftJoystick.transform.GetChild(0).GetComponent<Image>(); // gets the handle (knob) image of the left joystick
            leftJoystickHandleImage.enabled = leftJoyStickAlwaysVisible; // sets left joystick handle (knob) image to be always visible or not
        }
    }

    void Update()
    {
        // can move code from FixedUpdate() to Update() if your controlled object does not use physics
        // can move code from Update() to FixedUpdate() if your controlled object does use physics
        // can see which one works best for your project
    }

    void FixedUpdate()
    {
        // if the screen has been touched
        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches; // gets all the touches and stores them in an array

            // loops through all the current touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                // if this touch just started (finger is down for the first time), for this particular touch 
                if (myTouches[i].phase == TouchPhase.Began)
                {
                        // if this touch is on the left-side half of screen
                        if (myTouches[i].position.x < Screen.width / 2)
                        {
                            leftSideFingerID = myTouches[i].fingerId; // stores the unique id for this touch that happened on the left-side half of the screen

                            // if the left joystick will drag with any touch (left joystick is not set to stay in a fixed position)
                            if (leftJoystick.joystickStaysInFixedPosition == false)
                            {
                                var currentPosition = leftJoystickBackgroundImage.rectTransform.position; // gets the current position of the left joystick
                                currentPosition.x = myTouches[i].position.x + leftJoystickBackgroundImage.rectTransform.sizeDelta.x / 2; // calculates the x position of the left joystick to where the screen was touched
                                currentPosition.y = myTouches[i].position.y - leftJoystickBackgroundImage.rectTransform.sizeDelta.y / 2; // calculates the y position of the left joystick to where the screen was touched

                                // keeps the left joystick on the left-side half of the screen
                                currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + leftJoystickBackgroundImage.rectTransform.sizeDelta.x, Screen.width / 2);
                                currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - leftJoystickBackgroundImage.rectTransform.sizeDelta.y);

                                leftJoystickBackgroundImage.rectTransform.position = currentPosition; // sets the position of the left joystick to where the screen was touched (limited to the left half of the screen)

                                // enables left joystick on touch
                                leftJoystickBackgroundImage.enabled = true;
                                leftJoystickBackgroundImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                            }
                            else
                            {
                                // left joystick stays fixed, does not set position of left joystick on touch

                                // if the touch happens within the fixed area of the left joystick's background image within the x coordinate
                                if ((myTouches[i].position.x <= leftJoystickBackgroundImage.rectTransform.position.x) && (myTouches[i].position.x >= (leftJoystickBackgroundImage.rectTransform.position.x - leftJoystickBackgroundImage.rectTransform.sizeDelta.x)))
                                {
                                    // and the touch also happens within the left joystick's background image y coordinate
                                    if ((myTouches[i].position.y >= leftJoystickBackgroundImage.rectTransform.position.y) && (myTouches[i].position.y <= (leftJoystickBackgroundImage.rectTransform.position.y + leftJoystickBackgroundImage.rectTransform.sizeDelta.y)))
                                    {
                                        // makes the left joystick appear 
                                        leftJoystickBackgroundImage.enabled = true;
                                        leftJoystickBackgroundImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                                    }
                                }
                            }
                        }
                    }

                // if this touch has ended (finger is up and now off of the screen), for this particular touch 
                if (myTouches[i].phase == TouchPhase.Ended)
                {
                    // if this touch is the touch that began on the left half of the screen
                    if (myTouches[i].fingerId == leftSideFingerID)
                    {
                        // makes the left joystick disappear or stay visible
                        leftJoystickBackgroundImage.enabled = leftJoyStickAlwaysVisible;
                        leftJoystickHandleImage.enabled = leftJoyStickAlwaysVisible;
                    }
                }
            }
        }
    }
}
