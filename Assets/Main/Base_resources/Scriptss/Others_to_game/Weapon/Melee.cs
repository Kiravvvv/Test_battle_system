using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{

    bool No_stop_attack_bool = false;

    //[Tooltip("Ёффект от удара")]
    //[SerializeField]
    //MeleeWeaponTrail MeleeWeaponTrail_script = null;

    public override void Attack()
    {
        if (!No_stop_attack_bool)
        {
            Anim.SetBool("Attack", true);

            Anim.SetTrigger("Next_attack");

            No_stop_attack_bool = true;
        }
        
    }

    public override void End_attack()
    {
        Anim.SetBool("Attack", false);

        Reset_combo();
    }

    public virtual void Reset_combo()
    {
        No_stop_attack_bool = false;

        Anim.ResetTrigger("Next_attack");
    }

    public override void Activity(bool _activity)
    {
        //MeleeWeaponTrail_script.Activity_emit(_activity);
    }
}
