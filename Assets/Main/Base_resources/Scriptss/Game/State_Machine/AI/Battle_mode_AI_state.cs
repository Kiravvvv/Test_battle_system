using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_mode_AI_state : State_abstract
{

    protected bool Move_bool = false;

    protected float Time_pause_attack = 0;

    private AI_abstract Character;

    public Battle_mode_AI_state(AI_abstract character, StateMachine stateMachine) : base(character, stateMachine)
    {
        Character = character;
    }

    public override void Enter_state()
    {
        base.Enter_state();
        Character.New_target_move(Character.Target);
        Move_bool = true;
        Character.Anim.SetBool("Move", true);
    }

    protected override void Preparation()
    {
        base.Preparation();

        Character.End_attack_event.AddListener(End_attack);
    }

    public override void Slow_Update()
    {
        base.Slow_Update();

        if (Move_bool)
        {
            if (Character.Find_out_Remaining_distance(Character.transform.position, Character.Target.position) <= Character.Distance_attack)
            {
                if (Time_pause_attack <= 0)
                    Attack();
                else
                    Change_move(false);
            }
            else
            {
                if (Time_pause_attack > 0)
                    Change_move(true);
            }
        }


    }

    public override void Logic_Update()
    {
        base.Logic_Update();

        if(Time_pause_attack > 0)
        Pause_attack();
    }

    /// <summary>
    /// Отдыхаем и ждём
    /// </summary>
    void Pause_attack()
    {
        if (Time_pause_attack > 0)
        {
            Time_pause_attack -= Time.deltaTime;
        }
    }

    protected void Change_move(bool _bool)
    {
        Character.Anim.SetBool("Move", _bool);

        if (Character.NavMeshAgent_.isStopped == _bool)
        {
            Character.Stop_move_activity(!_bool);
        }

    }

    protected virtual void Attack()
    {
        Move_bool = false;
        Character.Anim.SetBool("Attack", true);
        Character.Stop_move_activity(true);
    }

    /// <summary>
    /// Продолжаем путь
    /// </summary>
    protected virtual void  End_attack()
    {
        Time_pause_attack = Random.Range(Character.Pause_attack_time.x, Character.Pause_attack_time.y);

        Move_bool = true;
        Character.Anim.SetBool("Attack", false);
        Character.Stop_move_activity(false);
    }
}
