//������� ������� �������� ��������� ���� � ������� ��������� ������� ��������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Sych scripts / Game / HurtBox")]
[DisallowMultipleComponent]
public class HurtBox : MonoBehaviour, I_damage
{

    [Tooltip("�������� ������ ��������")]
    [SerializeField]
    Health Main_health = null;

    public void Add_Main_health(Health _health_script)
    {
        Main_health = _health_script;
    }

    public void Damage()
    {
        Main_health.Damage();
    }

    public void Damage(int _damage)
    {
        Main_health.Damage(_damage, null);
    }

    public void Damage(int _damage, Game_character_abstract _killer)
    {
        Main_health.Damage(_damage, _killer);
    }
}
