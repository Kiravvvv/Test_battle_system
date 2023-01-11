using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Camera_state_abstract
{

    // cinemachine
    protected float _cinemachineTargetYaw;
    protected float _cinemachineTargetPitch;

    protected const float _threshold = 0.01f;


    protected Camera_control Main_script;
    protected State_machine_camera State_Machine_script;

    bool Start_preparation_bool = false;//Подготовка

    protected Camera_state_abstract(Camera_control main, State_machine_camera stateMachine)
    {
        Main_script = main;
        State_Machine_script = stateMachine;
    }

    /// <summary>
    /// Поворот камеры
    /// </summary>
    public abstract void Camera_rotation();


    protected static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    /// <summary>
    /// Вход состояния (начало реализации)
    /// </summary>
    public virtual void Enter_state()
    {
        if (!Start_preparation_bool)
        {
            Start_preparation_bool = true;
            Preparation();
        }
    }

    /// <summary>
    /// Начальная подготовка
    /// </summary>
    protected virtual void Preparation()
    {

    }

    /// <summary>
    /// Выход из состояния
    /// </summary>
    public virtual void Exit_state()
    {

    }

}
