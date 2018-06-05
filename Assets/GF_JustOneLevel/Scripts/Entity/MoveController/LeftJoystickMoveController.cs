using UnityEngine;

public class LeftJoystickMoveController : IMoveController
{
    private LeftJoystick joystick = null;

    public LeftJoystickMoveController(LeftJoystick joystick) {
        this.joystick = joystick;
    }
    
    public Vector3 GetInput()
    {
        return joystick.GetInputDirection();;
    }

    public bool IsIdle()
    {
        return false;
    }
}