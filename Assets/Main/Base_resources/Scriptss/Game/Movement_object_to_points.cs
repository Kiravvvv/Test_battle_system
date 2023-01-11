//Передвигает объект по точкам
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Movement_enum
{
    Last_end,//Дойти до последней точки и остановится
    Loop,//Зациклить движение от первой к последней и заново (перейдёт от последней к первой)
    Ping_pong//Зациклить передвижение от первой к последней и обратно
}

[AddComponentMenu("Sych scripts / Game / Movement object to points")]
[DisallowMultipleComponent]
public class Movement_object_to_points : MonoBehaviour
{
    [Tooltip("Точки движения")]
    [SerializeField]
    Transform[] Points_array = new Transform[0];

    [Tooltip("Скорость")]
    [SerializeField]
    float Speed = 0.01f;

    [Tooltip("Тип логики передвижения между точками")]
    [SerializeField]
    Movement_enum Type_movement = Movement_enum.Last_end;


    [Header("Внутриние параметры (не трогать, если не нужно)")]
    [Space(20)]
    [Tooltip("Меш объекта")]
    [SerializeField]
    Mesh Object_mesh = null;

    [Tooltip("Расположение на линии")]
    [Range(0f, 1f)]
    [SerializeField]
    float Step = 0;


    bool Active_bool = true;

    Transform Start_point = null;

    Transform End_point = null;

    int id_end_point = 1;

    bool Movement_end = true;

    private void Start()
    {
        if (Points_array.Length >= 2)
        {
            Start_point = Points_array[0];
            End_point = Points_array[1];
        }
        else
        {
            Active_bool = false;
            Debug.Log("Недостаточно точек для передвижения!");
        }
    }

    private void FixedUpdate()
    {
        if(Active_bool)
        Movement_object();
    }

    void Motor()
    {
        if (Movement_object())
        {
            Next_way();
        }
    }

    /// <summary>
    /// Назначить новые точки для передвижения
    /// </summary>
    void Next_way()
    {
        if (Points_array.Length - 1 > id_end_point && Movement_end || 0 < id_end_point && !Movement_end)
        {
            if (Movement_end)
            {
                id_end_point++;

                Start_point = Points_array[id_end_point - 1];
                End_point = Points_array[id_end_point];
            }
            else
            {
                id_end_point--;

                Start_point = Points_array[id_end_point + 1];
                End_point = Points_array[id_end_point];
            }

        }
        else
        {
            switch (Type_movement)
            {
                case Movement_enum.Last_end:
                    Active_bool = false;
                    break;

                case Movement_enum.Loop:
                    Start_point = Points_array[id_end_point];
                    End_point = Points_array[0];
                    id_end_point = 0;
                    break;

                case Movement_enum.Ping_pong:
                    if (Movement_end)
                    {
                        Start_point = Points_array[id_end_point];
                        End_point = Points_array[id_end_point - 1];

                        id_end_point = id_end_point - 1;

                        Movement_end = false;
                    }
                    else
                    {
                        Start_point = Points_array[0];
                        End_point = Points_array[id_end_point + 1];

                        id_end_point = id_end_point + 1;

                        Movement_end = true;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Передвижение и спрос
    /// </summary>
    /// <returns>Мы приехали?</returns>
    bool Movement_object()
    {
        bool result = false;

        if (Step < 1 && Movement_end)
        {
            Step += Speed;
        }
        else if (Movement_end)
        {
            Movement_end = false;
        }


        if (Step > 0 && !Movement_end)
        {
            Step -= Speed;
        }
        else if (!Movement_end)
        {
            Movement_end = true;
        }

        transform.localPosition = Vector3.Lerp(Start_point.position, End_point.position, Step);

        return result;
    }

    private void OnDrawGizmos()
    {
        if (Start_point && End_point && Object_mesh)
        {
            int sigment_number = 20;

            Vector3 preveuse_point = Start_point.position;

            for (int x = 0; x < sigment_number; x++)
            {
                float perimeter = (float)x / sigment_number;
                Vector3 point = Vector3.Lerp(Start_point.position, End_point.position, perimeter);
                Gizmos.DrawLine(preveuse_point, point);
                preveuse_point = point;
            }

            Vector3 pos = Vector3.Lerp(Start_point.position, End_point.position, Step);

            Gizmos.DrawMesh(Object_mesh, pos, transform.rotation, transform.localScale - new Vector3(-0.001f, -0.001f, -0.001f));

            Gizmos.DrawMesh(Object_mesh, Start_point.position, transform.rotation, transform.localScale * 0.8f);
            Gizmos.DrawMesh(Object_mesh, End_point.position, transform.rotation, transform.localScale * 0.8f);
        }
    }

}
