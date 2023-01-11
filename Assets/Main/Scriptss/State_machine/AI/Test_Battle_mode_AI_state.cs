using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Battle_mode_AI_state : Battle_mode_AI_state
{

    float Time_pause_dash = 0;

    float Time_pause_attack_dash = 0;

    float Time_attack_dash_training = 0;

    bool Attack_bool = false;

    bool Harm_bool = false;

    bool Dash_attack_bool = false;

    private AI_test Character;

    public Test_Battle_mode_AI_state(AI_test character, StateMachine stateMachine) : base(character, stateMachine)
    {
        Character = character;
    }

    public override void Enter_state()
    {
        base.Enter_state();
        Time_pause_attack_dash = Random.Range(Character.Pause_attack_dash_time.x, Character.Pause_attack_dash_time.y);
    }

    protected override void Preparation()
    {
        base.Preparation();
        Character.Start_harm_event.AddListener(Start_Harm);
        Character.End_harm_event.AddListener(End_Harm);
    }

    public override void Slow_Update()
    {
        if (Move_bool && !Harm_bool)
        {
            if (Character.Find_out_Remaining_distance(Character.transform.position, Character.Target.position) <= Character.Distance_attack)
            {
                if (Time_pause_attack <= 0)
                    Attack();
                else
                {
                    Change_move(false);
                }
                    
            }
            else
            {
                
                if (!Dash_attack_bool)
                {
                    Change_move(true);

                    if (Time_pause_attack_dash <= 0 && Vector3.Distance(Character.transform.position, Character.Target.position) > Character.Distance_attack && Vector3.Distance(Character.transform.position, Character.Target.position) < Character.Dash_script.Distance * 2)
                    {
                        Dash_attack_bool = true;
                        Character.Stop_move_activity(true);
                        Character.Anim.Play("Dash_attack_training");
                        Time_attack_dash_training = Character.Time_attack_dash_training;
                    }

                    if (Time_pause_attack > 0)
                    {
                        if (Time_pause_dash <= 0 && Vector3.Distance(Character.transform.position, Character.Target.position) < Character.Distance_attack * 2)
                        {
                            Time_pause_dash = Random.Range(Character.Pause_dash_time.x, Character.Pause_dash_time.y);
                            Character.Dash_script.Dash(new Vector2(Random.Range(-1f, 1f), -1));
                        }
                    }
                    else
                    {
                        if (Time_pause_dash <= 0 && Vector3.Distance(Character.transform.position, Character.Target.position) > Character.Dash_script.Distance)
                        {
                            Time_pause_dash = Random.Range(Character.Pause_dash_time.x, Character.Pause_dash_time.y);
                            Character.Dash_script.Dash(new Vector2(0, 1));
                        }
                    }
                }

                    
            }
        }
    }

    public override void Logic_Update()
    {
        if (!Harm_bool)
        {
            base.Logic_Update();

            if (!Dash_attack_bool)
            {
                if (Time_pause_dash > 0)
                    Pause_dash();

                if (Time_pause_attack_dash > 0)
                    Pause_attack_dash();
            }


            if (Time_attack_dash_training > 0)
                Timer_training_attack_dash();
        }

    }

    void Pause_dash()
    {
        if (Time_pause_dash > 0)
        {
            Time_pause_dash -= Time.deltaTime;
        }
    }

    void Start_attack_dash()
    {
        Character.Dash_script.Dash(new Vector2(0, 2));
    }

    void Pause_attack_dash()
    {
        if (Time_pause_attack_dash > 0)
        {
            Time_pause_attack_dash -= Time.deltaTime;
        }
    }

    void Timer_training_attack_dash()
    {
        if (Time_attack_dash_training > 0)
        {
            Time_attack_dash_training -= Time.deltaTime;

            Character.transform.LookAt(Character.Target);
            Character.transform.eulerAngles = new Vector3(0, Character.transform.eulerAngles.y, 0);

            if (Time_attack_dash_training <= 0)
            {
                Character.Anim.SetTrigger("Dash_attack");
                Character.Dash_script.Dash(new Vector2(0, 2));
            }
                
        }
    }

    void Start_Harm()
    {
        if (!Attack_bool)
        {
            Harm_bool = true;
            Character.Anim.Play("Harm");
            Character.Stop_move_activity(true);
        }

    }

    void End_Harm()
    {
        Character.Stop_move_activity(false);
        Harm_bool = false;
        
    }

    protected override void Attack()
    {
        base.Attack();
        Attack_bool = true;
    }

    protected override void End_attack()
    {
        base.End_attack();
        Attack_bool = false;

        if (Dash_attack_bool)
        {
            Dash_attack_bool = false;
            Time_pause_attack_dash = Random.Range(Character.Pause_attack_dash_time.x, Character.Pause_attack_dash_time.y);
        }
            
    }

}
