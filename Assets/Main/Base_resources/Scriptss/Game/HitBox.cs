//Триггер нанесения урона
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Sych scripts / Game / HitBox")]
[DisallowMultipleComponent]
public class HitBox : MonoBehaviour
{

    [Tooltip("Владелец (тот кто наносит урон)")]
    [SerializeField]
    Game_character_abstract My_Character = null;

    [Tooltip("Здоровье владельца (что бы не нанести урон самому себе)")]
    [SerializeField]
    Health My_Health = null;

    [Tooltip("Нанесения урона")]
    [SerializeField]
	[Min(1)]
    int Damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        Health target_health = null;

        if (other.GetComponent<Health>())
            target_health = other.GetComponent<Health>();

        if (My_Character != null)
        {
            if (target_health != null && target_health != My_Health)
            {
                target_health.Damage(Damage, My_Character);
            }
        }
        else
        {
            if (target_health != null)
            {
                target_health.Damage(Damage, My_Character);
            }
        }
        
    }
}
