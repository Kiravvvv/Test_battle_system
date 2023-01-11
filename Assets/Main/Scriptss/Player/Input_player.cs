using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Sych scripts / Game / Input player")]
[DisallowMultipleComponent]
public class Input_player : MonoBehaviour
{
    internal Vector2 Move_vector;

    internal Vector2 Look_rotation;

    internal bool Sprint_bool;

    internal bool Analog_Movement_bool;//Пересмотреть на нужность

    internal bool Jump_bool;

    public void OnMove(InputValue _value)
    {
        Move_vector = _value.Get<Vector2>();
    }


    public void OnSprint(InputValue _value)
    {
        Sprint_bool = _value.isPressed;
    }

    public void OnDash(InputValue _value)
    {
        
    }

    public void OnLook_rotation_camera (InputValue _value)
    {
        Look_rotation = _value.Get<Vector2>();
    }

    public void OnJump(InputValue _value)
    {
        Jump_bool = _value.isPressed;
    }

}
