using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class First_person_movement_Player_state : State_abstract
{

    float _jumpTimeoutDelta = 0;//����� ���������� � ������
    float _fallTimeoutDelta = 0;//����� ���������� � ��������

    float _terminalVelocity = 53.0f;//������������ �������� �������

    protected bool No_jump_bool = false;


    private Player_script Character;

    public First_person_movement_Player_state(Player_script character, StateMachine stateMachine) : base(character, stateMachine)
    {
        Character = character;
    }


    public override void Logic_Update()
    {
        base.Logic_Update();

        if (Character.Control_bool && !Character.Dash_script.Play_bool)
            Move();

        Jump_and_gravity();
    }

    void Move()
    {
        float speed = Character.Input.Sprint_bool ? Character.Speed_sprint : Character.Speed_movement;

        Vector3 moveDirectionForward = Character.transform.forward * Character.Input.Move_vector.y;
        Vector3 moveDirectionSide = Character.transform.right * Character.Input.Move_vector.x;

        Vector3 direction = (moveDirectionForward + moveDirectionSide).normalized;

        Vector3 inputDirection = new Vector3(Character.Input.Move_vector.x, 0.0f, Character.Input.Move_vector.y).normalized;

        Character.CharacterController_script.Move(direction * (speed * Time.deltaTime) +
                 new Vector3(0.0f, Character._verticalVelocity, 0.0f) * Time.deltaTime);
    }


    /// <summary>
    /// ������ � �������
    /// </summary>
    protected void Jump_and_gravity()
    {

        if (Character.Grounded_bool)
        {
            // �������� ������ ����-���� �������
            _fallTimeoutDelta = Character.Fall_Timeout;

            if (Character.Anim)
            {
                Character.Anim.SetBool("Jump", false);
                Character.Anim.SetBool("Free_fall", false);
            }


            // ���������� ����������� ������� ����� �������� ��� ����������
            if (Character._verticalVelocity < 0.0f)
            {
                Character._verticalVelocity = -2f;
            }

            // ������
            if (Character.Input.Jump_bool && Character.Jump_bool && _jumpTimeoutDelta <= 0.0f && !No_jump_bool)
            {
                // ���������� ������ �� H * -2 * G = ��������, ����������� ��� ���������� �������� ������
                Character._verticalVelocity = Mathf.Sqrt(Character.JumpHeight * -2f * Character.Gravity);

                if(Character.Anim)
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
                if (Character.Anim)
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

}
