//Набор параметров для персонажей
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Health))]//Атрибут добавляющий здоровье
[DisallowMultipleComponent]
public abstract class Game_character_abstract : Game_object_abstract
{
    #region Variables

    public StateMachine State_Machine;
    public Test_state Test_Standing;


    [Tooltip("Время обновления для медленного Update")]
    [SerializeField]
    float Time_slow_update = 0.4f;

    [Tooltip("Время обновления для медленного Update_2 (второй)")]
    [SerializeField]
    float Time_slow_update_2 = 0.5f;


    Coroutine Slow_update_coroutine = null;
    Coroutine Slow_update_coroutine_2 = null;

    internal UnityEvent Start_attack_event = new UnityEvent();//Евент когда началась атака (нужно для stateMachine)
    internal UnityEvent End_attack_event = new UnityEvent();//Евент когда закончилась атака (нужно для stateMachine)
    internal UnityEvent Reset_attack_event = new UnityEvent();//Евент когда можно продолжить атаку (продолжить комбо) (нужно для stateMachine)
    internal UnityEvent Start_harm_event = new UnityEvent();//Евент когда началась реакция получения урона (нужно для stateMachine)
    internal UnityEvent End_harm_event = new UnityEvent();//Евент когда закончилось реакция получения урона (нужно для stateMachine)
    internal UnityEvent<bool> Grounded_event = new UnityEvent<bool>();//Евент когда меняется заземлённость персонажа (он стоит или падает) (нужно для stateMachine)
    internal UnityEvent<bool> Control_bool_event = new UnityEvent<bool>();
    internal UnityEvent Dead_event = new UnityEvent();

#pragma warning disable 0649

    [field: Space(20)]
    [field: Header("Настройки персонажа")]

    [field: Tooltip("Скорость обычного передвижения")]
    [field: SerializeField]
    [field: Min(0)]
    //[FormerlySerializedAs(oldName: "Health_active")] // При переименование поля, можно создать ссылку на старое имя, что бы не переделывать (нужно не забыть using UnityEngine.Serialization;)
    public float Speed_movement { get; private set; } = 1.1f;

    [field: Tooltip("Скорость бега (спринта)")]
    [field: SerializeField]
    [field: Min(0)]
    public float Speed_sprint { get; private set; } = 2.2f;//Параметр скорости с которым работаем

    [field: Tooltip("Скорость поворота")]
    [field: SerializeField]
    [field: Min(0)]
    public float Speed_rotation { get; private set; } = 1f;

    [Tooltip("Голова (если есть)")]
    [field: SerializeField]
    public Transform Head { get; private set; } = null;

    protected Rigidbody Body = null;//Физика

    public bool Control_bool { get; private set; } = true;//Контролирует ли игрок персонажа




    internal bool Grounded_bool = true;//Стоит ли на замле

    protected bool Attack_bool = false;

    protected bool No_stop_attack_bool = false;

    protected bool Alive_bool = true; 

    #endregion

    #region MonoBehaviour Callbacks
    protected virtual void Awake()
    {
        Body = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();

        Initialized_State_machine();

        Slow_update_coroutine = StartCoroutine(Coroutine_Slow_update());
        Slow_update_coroutine_2 = StartCoroutine(Coroutine_Slow_update_2());

    }

    IEnumerator Coroutine_Slow_update()
    {
        while (true)
        {
            if (Alive_bool)
            {
                State_Machine.Current_State.Slow_Update();
            }

            yield return new WaitForSeconds(Time_slow_update);
        }

    }

    IEnumerator Coroutine_Slow_update_2()
    {
        while (true)
        {
            if (Alive_bool)
            {
                State_Machine.Current_State.Slow_Update_2();
            }

            yield return new WaitForSeconds(Time_slow_update_2);
        }

    }

    protected virtual void Update()
    {
        if (Alive_bool)
        {
            State_Machine.Current_State.Handle_Input();
            State_Machine.Current_State.Logic_Update();
        }

    }

    protected virtual void FixedUpdate()
    {
        if (Alive_bool)
        {
            State_Machine.Current_State.Physics_Update();
        }
    }

    #endregion


    #region Methods


    /// <summary>
    /// Инициализация машины состояния
    /// </summary>
    protected virtual void Initialized_State_machine()
    {
        State_Machine = new StateMachine();

        Test_Standing = new Test_state(this, State_Machine);

        State_Machine.Initialize(Test_Standing);
    }

    /// <summary>
    /// Получает урон
    /// </summary>
    protected virtual void Harm()
    {
        if (Alive_bool)
        {
            Active_control(false);

        }

    }

    #endregion


    #region Управляющие методы

    /// <summary>
    /// Начало получения урона
    /// </summary>
    public virtual void Harm_start()
    {
        if (Alive_bool)
        {
            Active_control(true);
            Start_harm_event.Invoke();
        }

    }


    /// <summary>
    /// Конец получения урона
    /// </summary>
    public virtual void Harm_end()
    {
        if (Alive_bool)
        {
            Active_control(true);
            End_harm_event.Invoke();
        }
            
    }


    /// <summary>
    /// Начало атаки
    /// </summary>
    protected virtual void Start_attack()
    {
        if (Alive_bool)
        {
            Active_control(false);
            Start_attack_event.Invoke();
        }

    }


    /// <summary>
    /// Атака закончена
    /// </summary>
    public virtual void End_attack()
    {
        Reset_combo_attack();
        Attack_bool = false;
        Speed_movement = 0;
        Active_control(true);
        End_attack_event.Invoke();
    }


    /// <summary>
    /// Ресетнуть триггер атаки
    /// </summary>
    public virtual void Reset_combo_attack()
    {
        if (Alive_bool)
        {
            No_stop_attack_bool = false;
            Reset_attack_event.Invoke();
        }
        
    }


    public virtual void Grounded(bool _bool)
    {
        Grounded_bool = _bool;
        Grounded_event.Invoke(_bool);
    }

    /// <summary>
    /// Включить/отключить контроль персонажем
    /// </summary>
    /// <param name="_active">Активность</param>
    public virtual void Active_control(bool _active)
    {
        Control_bool = _active;
        Control_bool_event.Invoke(_active);
    }

    /// <summary>
    /// Жизни закончились
    /// </summary>
    public virtual void Dead()
    {
        Active_control(false);

        Dead_event.Invoke();

        StopAllCoroutines();

        if (Anim)
        {
            Anim.Play("Death", 0, 0);
        }

        Alive_bool = false;

        print("Я всё.");

    }

    #endregion

    #region Публичные методы

    /// <summary>
    /// Включить в аниматоре анимацию через Id с булевой переменной
    /// </summary>
    /// <param name="param">Id</param>
    /// <param name="value">включение и отключение</param>
    public void SetAnimationBool(int param, bool value)
    {
        Anim.SetBool(param, value);
    }

    /// <summary>
    /// Включить в аниматоре анимацию через Id с тригерной переменной
    /// </summary>
    /// <param name="param">Id</param>
    public void TriggerAnimation(int param)
    {
        Anim.SetTrigger(param);
    }

    #endregion


    #region Дополнительно

    #endregion

}
