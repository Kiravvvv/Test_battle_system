using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Third_person_movement_Player_state : State_abstract
{
    float _animationBlend = 0;//Отвечает за параметр скорости передвижения в аниматоре

    float Speed_movement;

    Camera Cam = null;

    float _targetRotation = 0.0f;
    float _rotationVelocity = 0;


    float _jumpTimeoutDelta = 0;//Время проведённое в прыжке
    float _fallTimeoutDelta = 0;//Время проведённое в падаение

    float _terminalVelocity = 53.0f;//Максимальная скорость падения

    protected bool No_jump_bool = false;

    private Player_script Character;

    public Third_person_movement_Player_state(Player_script character, StateMachine stateMachine) : base(character, stateMachine)
    {
        Character = character;
    }

    protected override void Preparation()
    {
        base.Preparation();

        Cam = Game_administrator.Instance.Player_administrator.Cam;
    }

    public override void Logic_Update()
    {
        base.Logic_Update();

        if (Character.Control_bool)
            Move();

        Jump_and_gravity();
    }


    #region Управляющие методы

    #endregion


    #region Методы

    /// <summary>
    /// Передвижение
    /// </summary>
    protected void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = Character.Input.Sprint_bool ? Character.Speed_sprint : Character.Speed_movement;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (Character.Input.Move_vector == Vector2.zero)

            targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(Character.CharacterController_script.velocity.x, 0.0f, Character.CharacterController_script.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = Character.Input.Analog_Movement_bool ? Character.Input.Move_vector.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            Speed_movement = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * Character.SpeedChangeRate);

            // round speed to 3 decimal places
            Speed_movement = Mathf.Round(Speed_movement * 1000f) / 1000f;
        }
        else
        {
            Speed_movement = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * Character.SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(Character.Input.Move_vector.x, 0.0f, Character.Input.Move_vector.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (Character.Input.Move_vector != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              Cam.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(Character.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                Character.Speed_rotation);

            // rotate to face input direction relative to camera position
            Character.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        Character.CharacterController_script.Move(targetDirection.normalized * (Speed_movement * Time.deltaTime) +
                         new Vector3(0.0f, Character._verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        if(Character.Anim)
        Character.Anim.SetFloat("Speed", _animationBlend);
        //Anim.SetFloat(_animIDMotionSpeed, inputMagnitude);
    }



    /// <summary>
    /// Прыжок и падение
    /// </summary>
    protected void Jump_and_gravity()
    {

        if (Character.Grounded_bool)
        {
            // сбросить таймер тайм-аута падения
            _fallTimeoutDelta = Character.Fall_Timeout;

            if (Character.Anim)
            {
                Character.Anim.SetBool("Jump", false);
                Character.Anim.SetBool("Free_fall", false);
            }


            // остановить бесконечное падение нашей скорости при заземлении
            if (Character._verticalVelocity < 0.0f)
            {
                Character._verticalVelocity = -2f;
            }

            // Прыжок
            if (Character.Input.Jump_bool && Character.Jump_bool &&  _jumpTimeoutDelta <= 0.0f && !No_jump_bool)
            {
                // квадратный корень из H * -2 * G = скорость, необходимая для достижения желаемой высоты
                Character._verticalVelocity = Mathf.Sqrt(Character.JumpHeight * -2f * Character.Gravity);

                Character.Anim.SetBool("Jump", true);
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = Character.Jump_Timeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                Character.Anim.SetBool("Free_fall", true);
            }

            // if we are not grounded, do not jump
            Character.Input.Jump_bool = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (Character._verticalVelocity < _terminalVelocity)
        {
            Character._verticalVelocity += Character.Gravity * Time.deltaTime;
        }
    }

    #endregion



    #region Дополнительно


    #endregion
}
