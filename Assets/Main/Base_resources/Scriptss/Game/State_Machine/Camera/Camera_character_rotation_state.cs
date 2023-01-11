using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_character_rotation_state : Camera_state_abstract
{
    public Camera_character_rotation_state(Camera_control main_script, State_machine_camera stateMachine) : base(main_script, stateMachine)
    {
    }

    public override void Camera_rotation()
    {
        // if there is an input and camera position is not fixed
        if (Main_script.Input.Look_rotation.sqrMagnitude >= _threshold && !Main_script.LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = 1.0f;//IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            Main_script.Character_rotation.Rotate(new Vector3(0, Main_script.Input.Look_rotation.x * deltaTimeMultiplier * Main_script.Speed_rotation, 0));

            _cinemachineTargetPitch += Main_script.Input.Look_rotation.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, Main_script.BottomClamp, Main_script.TopClamp);

        // Cinemachine will follow this target
        Main_script.CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + Main_script.CameraAngleOverride,
            Main_script.Character_rotation.eulerAngles.y, 0.0f);

    }
}
