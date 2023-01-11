using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AI_test : AI_abstract
{

    [field: Tooltip("Способность дэша")]
    [field: SerializeField]
    public Dash_skill Dash_script {get; private set;}  = null;

    [field: Tooltip("Пауза между рывками")]
    [field: SerializeField]
    public Vector2 Pause_dash_time { get; private set; } = new Vector2 (2, 4);

    [field: Tooltip("Пауза между особой атакой рывком")]
    [field: SerializeField]
    public Vector2 Pause_attack_dash_time { get; private set; } = new Vector2(10, 20);

    [field: Tooltip("Длительность подготовки перед рывком атакой")]
    [field: SerializeField]
    public float Time_attack_dash_training { get; private set; } = 3f;

    protected override void Initialized_State_machine()
    {
        State_Machine = new StateMachine();

        Peaceful_state = new Peaceful_AI_mode_state(this, State_Machine);
        Battle_state = new Test_Battle_mode_AI_state(this, State_Machine);

        State_Machine.Initialize(Peaceful_state);
    }

    public override void Dead()
    {
        base.Dead();

        Anim.Play("Dead");
    }


}
