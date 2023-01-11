//Здоровье 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Sych scripts / Game / Health")]
[DisallowMultipleComponent]
public class Health : MonoBehaviour, I_damage
{

    /// <summary>
    /// Событие относящаяся кто атакует (даёт данные атакующего этого персонажа)
    /// </summary>
    public UnityEvent<Game_character_abstract> Killer_event = new UnityEvent<Game_character_abstract>();

    /// <summary>
    /// Событие относящаяся кто лечит (даёт данные лечащего этого персонажа)
    /// </summary>
    public UnityEvent<Game_character_abstract> Healer_event = new UnityEvent<Game_character_abstract>();

    /// <summary>
    /// Событие когда здоровье закончилось
    /// </summary>
    public UnityEvent Dead_event = new UnityEvent();

    /// <summary>
    /// Событие когда наносится вред
    /// </summary>
    public UnityEvent Harm_event = new UnityEvent();

    /// <summary>
    /// Событие когда добавляется исцеление
    /// </summary>
    public UnityEvent Heal_event = new UnityEvent();

    [field: Space(20)]
    [field: Header("Основное")]

    [field: Tooltip("Количество жизней")]
    [field: SerializeField]
    [field: Min(1)]
    public int Health_active { get; private set; } = 10;

    internal int Health_default { get; private set; } = 0;//Параметр для манипуляции с жизнями

    [Tooltip("Пауза отключающая нанесение урона")]
    [SerializeField]
    float Time_pause_damage = 0f;

    bool Damage_add_bool = true;//Можно ли получать урон

    [Tooltip("Не умирает")]
    [SerializeField]
    private bool No_death_bool = false;

    [Tooltip("Уничтожается сразу когда заканчивается здоровье")]
    [SerializeField]
    private bool Death_destroy_bool = false;


    [Space(20)]
    [Header("Дополнительно")]

    [Tooltip("Скрипт тряпичной куклы (не обязательно)")]
    [SerializeField]
    Ragdoll_activity Ragdoll_script = null;

    [Tooltip("Скрипт мигания при получение урона (не обязательно)")]
    [SerializeField]
    Blinking_effect Blinking_effect_script = null;



    public bool Alive_bool { get; private set; } = true;//Является ли живым

    protected Transform My_transform = null;//Трансформ объекта 

    protected virtual void Start()
    {
        My_transform = transform;

        Health_default = Health_active;
    }

    #region
    /// <summary>
    /// Изменение здоровья
    /// </summary>
    /// <param name="_change">На какое значение изменить</param>
    protected virtual void Change_health(int _change)
    {

        Health_active += _change;

        Health_active = Mathf.Clamp(Health_active, 0, Health_default);

        if (!No_death_bool && Health_active <= 0)
            {

                Death();
            }

    }

    /// <summary>
    /// Получение урона с указанием кто атаковал
    /// </summary>
    /// <param name="_damage">Значение урона</param>
    /// <param name="_killer">Кто атаковал</param>
    protected virtual void Damage_add(int _damage, Game_character_abstract _killer)
    {

        if (Alive_bool && Damage_add_bool)
        {
            Change_health(-_damage);

            if (Time_pause_damage > 0)
            {
                Damage_add_bool = false;
                StartCoroutine(Coroutine_Time_pause_damage());
            }

            Killer_event?.Invoke(_killer);

            Harm_event.Invoke();

            if (Blinking_effect_script)
            Blinking_effect_script.Activation();

        }
    }

    /// <summary>
    /// Таймер при окончание которого можно снова наносить урон
    /// </summary>
    /// <returns></returns>
    IEnumerator Coroutine_Time_pause_damage()
    {
        yield return new WaitForSeconds(Time_pause_damage);

        Damage_add_bool = true;
    }

    /// <summary>
    /// Получение лечения с указанием кто лечил
    /// </summary>
    /// <param name="_heal">Значение лечения</param>
    /// <param name="_healer">Кто лечил</param>
    protected virtual void Heal_add(int _heal, Game_character_abstract _healer)
    {
        if (Alive_bool)
        {
            Change_health(_heal);

            Healer_event.Invoke(_healer);

            Heal_event.Invoke();
        }
    }
    #endregion




    #region Публичные методы
    /// <summary>
    /// Нанести 1 урон
    /// </summary>
    [ContextMenu("Add Damage")]
    public void Damage()
    {
        Damage_add(1, null);

    }

    /// <summary>
    /// Нанести определённый урон
    /// </summary>
    /// <param name="_damage">Количество урона</param>
    public void Damage(int _damage)
    {
        Damage_add(_damage, null);
    }

    /// <summary>
    /// Нанести определённый урон и указать, кто это сделал
    /// </summary>
    /// <param name="_damage">Количество урона</param>
    /// <param name="_killer">Кто атаковал</param>
    public void Damage(int _damage, Game_character_abstract _killer)
    {
        Damage_add(_damage, _killer);
    }



    /// <summary>
    /// Исцелить 1 урон
    /// </summary>
    [ContextMenu("Add Heal")]
    public void Heal()
    {
        Heal_add(1, null);

    }

    /// <summary>
    /// Исцелить определённое количество здоровья
    /// </summary>
    /// <param name="_damage">Количество исцеления</param>
    public void Heal(int _heal)
    {
        Heal_add(_heal, null);
    }

    /// <summary>
    /// Исцелить определённое количество здоровья и указать кто это сделал
    /// </summary>
    /// <param name="_damage">Количество исцеления</param>
    /// <param name="_killer">Кто исцелял</param>
    public void Heal(int _heal, Game_character_abstract _healer)
    {
        Heal_add(_heal, _healer);
    }


    /// <summary>
    /// Смерть/разрушение объекта
    /// </summary>
    [ContextMenu(nameof(Death))]
    public virtual void Death()
    {
        Alive_bool = false;

        Health_active = 0;

        Dead_event.Invoke();

        if (Ragdoll_script)
        {
            Ragdoll_script.Active_change(true);
        }
        else if (Death_destroy_bool)
        {
            Destroy(gameObject);
        }

    }
    #endregion
}
