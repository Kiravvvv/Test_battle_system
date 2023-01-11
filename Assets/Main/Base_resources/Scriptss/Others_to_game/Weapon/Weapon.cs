using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Tooltip("Аниматор")]
    [SerializeField]
    protected Animator Anim = null;

    private void OnEnable()
    {
        Activity(false);
    }

    /// <summary>
    /// Атака
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// Конец атаки
    /// </summary>
    public abstract void End_attack();

    /// <summary>
    /// Активность оружия
    /// </summary>
    /// <param name="_activity">Активирован (исользуется в атаке) ?</param>
    public abstract void Activity(bool _activity);

}
