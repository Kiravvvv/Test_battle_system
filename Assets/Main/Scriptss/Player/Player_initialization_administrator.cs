//Инициализация и коннект нужных скриптов игрока (например для Game_administrator и для интерфейса)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_initialization_administrator : MonoBehaviour
{

    private void Awake()
    {
        Game_administrator.Instance.Add_player_administrator(this);
    }

    [Tooltip("Скрипт игрока")]
    [field: SerializeField]
    public Game_character_abstract Player_sc { get; private set; } = null;

    [Tooltip("Камера")]
    [field: SerializeField]
    public Camera Cam { get; private set; } = null;

    [Tooltip("Здоровье")]
    [field: SerializeField]
    public Health Player_health { get; private set; } = null;


    #region Системные методы

    private void Start()
    {
        if (Player_health)
        {
            Player_health.Harm_event.AddListener(Change_health);
            Player_health.Heal_event.AddListener(Change_health);
            Player_health.Harm_event.AddListener(Damage_add);
            Player_health.Dead_event.AddListener(Death);
        }
    }

    #endregion

    #region Methods
    void Change_health()
    {
        Player_interface_UI.Instance.Player_Health_info((float)Player_health.Health_active / (float)Player_health.Health_default);
    }


    void Damage_add()
    {
        if (Player_health.Alive_bool)
        {
            Player_interface_UI.Instance.Damage_anim_effect();
        }
    }

    void Death()
    {
        Game_administrator.Instance.End_game(false);
    }
    #endregion
}
