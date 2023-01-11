//Обычный объект
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public abstract class Game_object_abstract : Item
{
    [Space(20)]
    [Header("Настройки объекта")]

    [Tooltip("Имя объекта (если есть)")]
    public string Name  = "Имя объекта";

    [Tooltip("Аниматор (если есть)")]
    [field: SerializeField]
    public Animator Anim { get; private set; } = null;

    [Tooltip("Скрипт для управления звуками")]
    [field: SerializeField]
    protected Sound_control Sound_control_ { get; private set; } = null;

    protected Transform My_transform { get; private set; } = null;//Трансформ объекта 

    protected virtual void Start()
    {
        My_transform = transform;
    }

    /// <summary>
    /// Узнать имя объекта
    /// </summary>
    public string Find_out_Name
    {
        get
        {
            return Name;
        }
    }

}
