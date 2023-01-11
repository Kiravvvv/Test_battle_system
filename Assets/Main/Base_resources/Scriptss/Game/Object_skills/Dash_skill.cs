//����� � ������� (�����������)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Sych scripts / Game / Skills / Dash")]
[DisallowMultipleComponent]
public class Dash_skill : MonoBehaviour
{

    [field: Tooltip("��������� �����")]
    [field: SerializeField]
    public float Distance { get; private set; } = 5f;

    [Tooltip("�������� �����")]
    [SerializeField]
    float Speed = 5f;

    [Tooltip("��������� ����������� ������� �� ��������� (���� �� ������� �����) � �������� ����������� ������ �� �����������")]
    [SerializeField]
    bool Only_horizontal = false;

    [Tooltip("������� ������ ���� ����������� ��� ����� (��� ����������� ����������� ����� + �����, � ������ ����� ��� ������ �����)")]
    [SerializeField]
    bool No_diagonal = false;

    [Tooltip("���������� �� ����� �����")]
    [SerializeField]
    bool Forward_bool = true;

    [Tooltip("���������� �� ����� �����")]
    [SerializeField]
    bool Back_bool = true;

    [Tooltip("���������� �� ����� �����")]
    [SerializeField]
    bool Left_bool = true;

    [Tooltip("���������� �� ����� ������")]
    [SerializeField]
    bool Right_bool = true;

    [Tooltip("����� ��������� ����� (����� � �����)")]
    [SerializeField]
    UnityEvent<bool> Dash_event = new UnityEvent<bool>();



    [Tooltip("������ ������, ��� �����")]
    [SerializeField]
    bool Debug_mode = false;

    Vector3 End_point = Vector3.zero;//�������� ����� �����

    Coroutine Dash_coroutine = null;

    internal bool Play_bool = false;

    [ContextMenu("����������� ��� �����")]
    void Test()
    {
        Dash(Vector2.up);
    }


    /// <summary>
    /// ��������� �����
    /// </summary>
    /// <param name="_direction">�����������</param>
    public void Dash(Vector2 _direction)
    {
        Vector3 direction_move = Vector3.zero;

        if (!Forward_bool && _direction.y > 0) _direction.y = 0;
        if (!Back_bool && _direction.y < 0) _direction.y = 0;
        if (!Right_bool && _direction.x > 0) _direction.x = 0;
        if (!Left_bool && _direction.x < 0) _direction.x = 0;

        if (No_diagonal && (_direction.y != 0 && _direction.x == 0 || _direction.y == 0 && _direction.x != 0) || !No_diagonal)
            direction_move = transform.forward * _direction.y + transform.right * _direction.x;

        if (Only_horizontal)
        {
            Vector3 point_test = transform.position + direction_move * 10;
            point_test.y = transform.position.y;

            Vector3 direction = point_test - transform.position;

            End_point = transform.position + (Distance * direction.normalized);
        }
        else
            End_point = transform.position + (Distance * direction_move);

        if(Dash_coroutine == null)
        Dash_coroutine = StartCoroutine(Coroutine_dash_update());
    }



    /// <summary>
    /// �����������
    /// </summary>
    /// <returns></returns>
    IEnumerator Coroutine_dash_update()
    {
        Play_bool = true;
        Dash_event.Invoke(true);

        float step = 0;

        while (step < 1)
        {
            step += Speed * Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, End_point, step);

            yield return null;
        }

        Dash_coroutine = null;

        Dash_event.Invoke(false);
        Play_bool = false;
    }


    private void OnDrawGizmosSelected()
    {
        if (Debug_mode)
        {
            Gizmos.color = Color.blue;

            Vector3 end_point_gizmos = Vector3.zero;

            Vector3 dir_object = Vector3.zero;

                for(int x = 0; x < 4; x++)
                {
                    if (x == 0) dir_object = transform.forward;
                    else if (x == 1) dir_object = -transform.forward;
                    else if (x == 2) dir_object = transform.right;
                    else if (x == 3) dir_object = -transform.right;

                    if (Only_horizontal)
                    {
                        Vector3 point_test = transform.position + dir_object * 10;
                        point_test.y = transform.position.y;

                        Vector3 direction = point_test - transform.position;

                        end_point_gizmos = transform.position + (Distance * direction.normalized);
                    }
                    else
                    {
                        end_point_gizmos = transform.position + (dir_object * Distance);
                    }

                    if(x == 0 && Forward_bool || x == 1 && Back_bool || x == 2 && Right_bool || x == 3 && Left_bool)
                    Gizmos.DrawCube(end_point_gizmos, Vector3.one);
                }

            
        }

    }

}
