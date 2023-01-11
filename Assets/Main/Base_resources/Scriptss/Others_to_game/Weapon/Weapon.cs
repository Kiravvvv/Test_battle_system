using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Tooltip("��������")]
    [SerializeField]
    protected Animator Anim = null;

    private void OnEnable()
    {
        Activity(false);
    }

    /// <summary>
    /// �����
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// ����� �����
    /// </summary>
    public abstract void End_attack();

    /// <summary>
    /// ���������� ������
    /// </summary>
    /// <param name="_activity">����������� (����������� � �����) ?</param>
    public abstract void Activity(bool _activity);

}
