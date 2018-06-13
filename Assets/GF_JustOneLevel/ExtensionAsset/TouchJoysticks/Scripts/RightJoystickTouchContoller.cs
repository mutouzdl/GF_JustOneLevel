/*
about this script: 

if a joystick is not set to stay in a fixed position
 this script makes it appear and  disappear within the right-side half of the screen where the screen was touched 

if a joystick is set to stay in a fixed position
 this script makes it appear and disappear if the user touches within the area of its background image (even if it is not currently visible)
 
this script also optionaly ykeeps the joystick always visible
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RightJoystickTouchContoller : MonoBehaviour
{
    public Image rightJoystickBackgroundImage; // background image of the right joystick (the joystick's handle (knob) is a child of this image and moves along with it)
    public bool rightJoyStickAlwaysVisible = false; // value from right joystick that determines if the right joystick should be always visible or not

    private Image rightJoystickHandleImage; // handle (knob) image of the right joystick
    private RightJoystick rightJoystick; // script component attached to the right joystick's background image
    private int rightSideFingerID = 0; // unique finger id for touches on the right-side half of the screen

    void Start()
    {
        if (rightJoystickBackgroundImage.GetComponent<RightJoystick>() == null)
        {
            Debug.LogError("There is no RightJoystick script attached to Right Joystick game object.");
        }
        else
        {
            rightJoystick = rightJoystickBackgroundImage.GetComponent<RightJoystick>(); // gets the right joystick script
            rightJoystickBackgroundImage.enabled = rightJoyStickAlwaysVisible; // sets right joystick background image to be always visible or not
        }

        if (rightJoystick.transform.GetChild(0).GetComponent<Image>() == null)
        {
            Debug.LogError("There is no right joystick handle attached to this script.");
        }
        else
        {
            rightJoystickHandleImage = rightJoystick.transform.GetChild(0).GetComponent<Image>(); // gets the handle (knob) image of the right joystick
            rightJoystickHandleImage.enabled = rightJoyStickAlwaysVisible; // sets right joystick handle (knob) image to be always visible or not
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
                    // if this touch is on the right-side half of screen
                    if (myTouches[i].position.x > Screen.width / 2)
                    {
                        rightSideFingerID = myTouches[i].fingerId; // stores the unique id for this touch that happened on the right-side half of the screen

                        // if the right joystick will drag with any touch
                        if (rightJoystick.joystickStaysInFixedPosition == false)
                        {
                            var currentPosition = rightJoystickBackgroundImage.rectTransform.position; // gets the current position of the right joystick
                            currentPosition.x = myTouches[i].position.x + rightJoystickBackgroundImage.rectTransform.sizeDelta.x / 2; // calculates the x position of the right joystick to where the screen was touched
                            currentPosition.y = myTouches[i].position.y - rightJoystickBackgroundImage.rectTransform.sizeDelta.y / 2; // calculates the y position of the right joystick to where the screen was touched

                            // keep the right joystick on the right-side half of the screen
                            currentPosition.x = Mathf.Clamp(currentPosition.x, Screen.width / 2 + rightJoystickBackgroundImage.rectTransform.sizeDelta.x, Screen.width);
                            currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - rightJoystickBackgroundImage.rectTransform.sizeDelta.y);

                            rightJoystickBackgroundImage.rectTransform.position = currentPosition; // sets the position of the right joystick to where the screen was touched (limited to the right half of the screen)

                            // enables right joystick on touch
                            rightJoystickBackgroundImage.enabled = true;
                            rightJoystickBackgroundImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                        }
                        else
                        {
                            // right joystick stays fixed, does not set position of right joystick on touch

                            // if the touch happens within the fixed area of the right joystick's background image within the x coordinate
                            if ((myTouches[i].position.x <= rightJoystickBackgroundImage.rectTransform.position.x) && (myTouches[i].position.x >= (rightJoystickBackgroundImage.rectTransform.position.x - rightJoystickBackgroundImage.rectTransform.sizeDelta.x)))
                            {
                                // and the touch also happens within the right joystick's background image y coordinate
                                if ((myTouches[i].position.y >= rightJoystickBackgroundImage.rectTransform.position.y) && (myTouches[i].position.y <= (rightJoystickBackgroundImage.rectTransform.position.y + rightJoystickBackgroundImage.rectTransform.sizeDelta.y)))
                                {
                                    // makes the right joystick appear
                                    rightJoystickBackgroundImage.enabled = true;
                                    rightJoystickBackgroundImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                                }
                            }
                        }
                    }
                }

                // if this touch has ended (finger is up and now off of the screen), for this particular touch 
                if (myTouches[i].phase == TouchPhase.Ended)
                {
                    // if this touch is the touch that began on the right half of the screen
                    if (myTouches[i].fingerId == rightSideFingerID)
                    {
                        // makes the right joystick disappear or stay visible
                        rightJoystickBackgroundImage.enabled = rightJoyStickAlwaysVisible;
                        rightJoystickHandleImage.enabled = rightJoyStickAlwaysVisible;
                    }
                }
            }
        }
    }
}
