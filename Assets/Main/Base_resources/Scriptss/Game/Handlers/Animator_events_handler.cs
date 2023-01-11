//������ �������������� ������ � ���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Sych scripts / Game / Handlers / Animator events handler")]
[DisallowMultipleComponent]
public class Animator_events_handler : MonoBehaviour
{
    [Tooltip("����� ����� ������������ �����")]
    public UnityEvent End_attack_event = new UnityEvent();

    [Tooltip("����� ����������� ����� ����� (����� ������ ��������� �����)")]
    public UnityEvent Reset_combo_attack_event = new UnityEvent();

    [Tooltip("����� ����� ������������ ��������� �����")]
    public UnityEvent Harm_end_event = new UnityEvent();

    void End_attack()
    {
        End_attack_event.Invoke();
    }

    void Reset_combo_attack()
    {
        Reset_combo_attack_event.Invoke();
    }

    void Harm_end()
    {
        Harm_end_event.Invoke();
    }

}
