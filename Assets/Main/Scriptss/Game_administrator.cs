//Скрипт общего управления во время игры
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Game_administrator : Singleton<Game_administrator>
{
    /// <summary>
    /// Событие относящихся к контролю игрока 
    /// </summary>
    /// <param name="_bool">Параметр активации</param>
    public static UnityEvent<bool> Player_control_event = new UnityEvent<bool>();

    /// <summary>
    /// Событие относящиеся к началу игры
    /// </summary>
    public static UnityEvent Start_game_event = new UnityEvent();

    /// <summary>
    /// Событие относящиеся к паузе игры
    /// </summary>
    public static UnityEvent<bool> Pause_game_event = new UnityEvent<bool>();

    /// <summary>
    /// Событие относящиеся к концу игры
    /// </summary>
    /// <param name="_active">Параметр активации</param>
    public static UnityEvent<bool> End_game_event = new UnityEvent<bool>();


    internal Player_initialization_administrator Player_administrator { get; private set; } = null;//Скрипт игрока

    internal List<Transform> Enemy_list { get; private set; } = new List<Transform>();//Лист противников


    /// <summary>
    ///  Сменить активность контроля игрока над персонажем
    /// </summary>
    /// <param name="_active">Включение или выключение</param>
    public void Player_control_activity(bool _active)
    {
        Player_control_event.Invoke(_active);
    }


    /// <summary>
    /// Начать игру
    /// </summary>
    public void Start_game()
    {
        Player_control_event.Invoke(true);
        Start_game_event.Invoke();

    }

    /// <summary>
    /// Поставить на паузу игру
    /// </summary>
    public void Pause_game(bool _bool)
    {
        Player_control_event.Invoke(!_bool);
        Pause_game_event.Invoke(_bool);

    }

    /// <summary>
    /// Закончить игру
    /// </summary>
    /// <param name="_win">Победа?</param>
    public void End_game(bool _win)
    {
        Player_control_event.Invoke(false);
        End_game_event.Invoke(_win);

    }

    /// <summary>
    /// Добавить администратора игрока
    /// </summary>
    /// <param name="_player_script"></param>
    public void Add_player_administrator(Player_initialization_administrator _player_script)
    {
        Player_administrator = _player_script;
    }


    /// <summary>
    /// Добавить противника в лист
    /// </summary>
    /// <param name="_enemy">Противник</param>
    public void Add_enemy_list(Transform _enemy)
    {
        Enemy_list.Add(_enemy);
    }

    /// <summary>
    /// Убрать противника из листа
    /// </summary>
    /// <param name="_enemy">Противник</param>
    public void Remove_enemy_list(Transform _enemy)
    {
        Enemy_list.Remove(_enemy);
    }

}
