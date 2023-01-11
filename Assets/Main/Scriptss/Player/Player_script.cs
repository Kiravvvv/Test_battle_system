using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Player_initialization_administrator))]
//[RequireComponent(typeof(Item_in_hand_handler))]
[RequireComponent(typeof(Input_player))]
//[RequireComponent(typeof(Grounded_handler))]
//[RequireComponent(typeof(Camera_control))]
public class Player_script : Game_character_abstract
{
    public First_person_movement_Player_state Movement_state;

    #region Переменные

    [Tooltip("Управление камерой")]
    [field: SerializeField]
    public Camera_control Camera_control_sc { get; private set; } = null;


    [Tooltip("Для управления")]
    [field: SerializeField]
    public CharacterController CharacterController_script { get; private set; } = null;

    [Tooltip("Оружие")]
    [SerializeField]
    Weapon Weapon_script = null;

    [field: Tooltip("Скрипт дэша")]
    [field: SerializeField]
    public Dash_skill Dash_script { get; private set; } = null;

    bool Dash_time_bool = false;
    Coroutine Dash_coroutine = null;

    [Tooltip("Ускорение и замедление (выяснить зачем)")]
    [field: SerializeField]
    public float SpeedChangeRate { get; private set; } = 10.0f;

    /*
    [Tooltip("Как быстро персонаж поворачивается лицом в направлении движения")]
    [Range(0.0f, 0.3f)]
    [SerializeField]
    float RotationSmoothTime = 0.12f;
    */

    [Tooltip("Скрипт отвечающий за принятие нажатия кнопок игрока")]
    [field: SerializeField]
    public Input_player Input { get; private set; } = null;


    [Space(20)]
    [Header("Гравитация")]

    [Tooltip("Персонаж использует собственное значение гравитации. Гравитация в самом Unity по умолчанию -9.81f")]
    public float Gravity = -15.0f;

    [Tooltip("Время, необходимое для перехода в состояние падения. Полезно для спуска по лестнице (а то каждая ступенька заставит падать)")]
    public float Fall_Timeout = 0.15f;


    [field: Space(20)]
    [field: Header("Прыжок")]
    [field: Tooltip("Разрешение на прыжок")]
    [field: SerializeField]
    public bool Jump_bool { get; private set; } = true;

    [ShowIf(nameof(Jump_bool))]
    [Tooltip("Высота, на которую может прыгнуть игрок")]
    public float JumpHeight = 1.2f;

    [ShowIf(nameof(Jump_bool))]
    [Tooltip("Время, необходимое для того, чтобы снова прыгнуть. Установите 0f, чтобы снова мгновенно прыгнуть")]
    public float Jump_Timeout = 0.2f;



    internal float _verticalVelocity  = 0;//Вертикальная скорость персонажа

    #endregion

    #region MonoBehaviour Callbacks

    protected override void Start()
    {
        base.Start();
        Game_Player.Cursor_player(false);
    }

    protected override void Update()
    {
        base.Update();
    }

    #endregion

    #region Методы


    protected override void Initialized_State_machine()
    {
        State_Machine = new StateMachine(); 

        Movement_state = new First_person_movement_Player_state(this, State_Machine);

        State_Machine.Initialize(Movement_state);
    }

    #endregion

    #region Управляющие методы

    public void OnAttack()
    {
        Weapon_script.Attack();
    }

    public void OnDash()
    {
        if (Dash_time_bool)
        {
            Dash_script.Dash(Input.Move_vector);
        }
        else if(Dash_coroutine == null)
            Dash_coroutine = StartCoroutine(Coroutine_Dash_time());
    }

    IEnumerator Coroutine_Dash_time()
    {
        Dash_time_bool = true;
        yield return new WaitForSeconds(0.3f);
        Dash_time_bool = false;
        Dash_coroutine = null;
    }

    #endregion

    #region Публичные Методы

    #endregion

    #region Дополнительно

    #endregion
}
