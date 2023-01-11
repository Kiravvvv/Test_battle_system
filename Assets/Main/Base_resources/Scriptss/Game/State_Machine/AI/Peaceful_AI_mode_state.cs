using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peaceful_AI_mode_state : State_abstract
{
    Vector3 End_point;

    bool Move_bool = false;

    float Time_pause = 0;

    private AI_abstract Character;

    public Peaceful_AI_mode_state(AI_abstract character, StateMachine stateMachine) : base(character, stateMachine)
    {
        Character = character;
    }

    public override void Enter_state()
    {
        base.Enter_state();
    }

    protected override void Preparation()
    {
        base.Preparation();
        End_point = Character.Nav_random_point_target(50);
        Move_bool = true;
        Character.Anim.SetBool("Move", true);
    }

    public override void Exit_state()
    {
        base.Exit_state();
    }

    public override void Slow_Update()
    {
        base.Slow_Update();

        if (Move_bool)
        {

            if (Character.Find_out_Remaining_distance(Character.transform.position, End_point) <= 0.5f)
            {
                Move_bool = false;

                Time_pause = Random.Range(Character.Pause_walk_time.x, Character.Pause_walk_time.y);

                Character.Anim.SetBool("Move", false);
            }
        }


        Detect_enemy();
    }



    public override void Logic_Update()
    {
        base.Logic_Update();

        if (!Move_bool)
        {
            Pause();
        }
    }


    /// <summary>
    /// „уем игрока
    /// </summary>
    void Detect_enemy()
    {
        Collider[] check_array = Physics.OverlapSphere(Character.transform.position, Character.Radius_detect, 1 << 0);

        for (int x = 0; x < check_array.Length; x++)
        {
            if (check_array[x].tag == Character.Tag_enemy)
            {
                Character.Target = check_array[x].transform;
                Character.State_Machine.Change_State(Character.Battle_state);
                break;
            }

        }
    }


    /// <summary>
    /// ќтдыхаем и ждЄм
    /// </summary>
    void Pause()
    {
        if (Time_pause > 0)
        {
            Time_pause -= Time.deltaTime;
        }
        else
        {
            Move_bool = true;
            Character.Stop_move_activity(false);
            End_point = Character.Nav_random_point_target(50);

            Character.Anim.SetBool("Move", true);
        }
    }

}
