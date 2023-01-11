using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using NaughtyAttributes;

public abstract class AI_abstract : Game_character_abstract
{
    public Peaceful_AI_mode_state Peaceful_state;
    public Battle_mode_AI_state Battle_state;

    #region Переменные
    [field: Space(20)]
    [field: Header("Настройки ИИ")]

    [field: Foldout("Общее ИИ")]
    [field: Tooltip("Тег противника (кого будет атаковать AI с таким тегом")]
    [field: SerializeField]
    public string Tag_enemy { get; private set; } = "Player";

    NavMeshPath NavMeshPath_;// путь до цели на невмеше

    [field: Tooltip("Дистанция атаки")]
    [field: SerializeField]
    public float Distance_attack { get; private set; } = 2f;

    [field: Tooltip("Время паузы между атаками")]
    [field: SerializeField]
    public Vector2 Pause_attack_time { get; private set; } = new Vector2(0.5f, 2f);

    [field: Tooltip("Время паузы во время передвежения между точками")]
    [field: SerializeField]
    public Vector2 Pause_walk_time { get; private set; } = new Vector2(2, 8);

    [field:  Tooltip("Радиус обнуружения игрока")]
    [field: SerializeField]
    public float Radius_detect { get; private set; } = 1f;

    [Tooltip("ИИ агент")]
    [field: SerializeField]
    public NavMeshAgent NavMeshAgent_ { get; private set; } = null;

    internal Transform Target = null;//Цель

    internal UnityEvent Distance_target_attack_event = new UnityEvent();//Евент когда дошёл до расстояния атаки цели (нужно для stateMachine)

    Coroutine Coroutine_Update_target = null;//Коррутина для обновления слежения за целью (если она передвигается)
    
    #endregion


    #region Callback Методы

    protected override void Awake()
    {
        NavMeshPath_ = new NavMeshPath();
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        NavMeshAgent_.speed = Speed_movement;
        NavMeshAgent_.angularSpeed = Speed_rotation;
        NavMeshAgent_.avoidancePriority = Random.Range(4, 50);

    }

    private void OnEnable()
    {
        Game_administrator.Instance.Add_enemy_list(transform);
    }

    #endregion


    #region Методы

    protected override void Initialized_State_machine()
    {
        State_Machine = new StateMachine();

        Peaceful_state = new Peaceful_AI_mode_state(this, State_Machine);
        Battle_state = new Battle_mode_AI_state(this, State_Machine);

        State_Machine.Initialize(Peaceful_state);
    }

    #endregion

    #region Управляющие методы

    /// <summary>
    /// Назначить новую цель
    /// </summary>
    /// <param name="_target">Цель</param>
    public virtual void New_target_move(Transform _target)
    {
        if (Alive_bool && Control_bool)
        {
            Target = _target;
                if (Coroutine_Update_target != null)
                    StopCoroutine(Update_target_coroutine());

                Coroutine_Update_target = StartCoroutine(Update_target_coroutine());

        }
    }

    public virtual void New_target_move(Vector3 _target)
    {
        if (Alive_bool && Control_bool)
        {
            if(!Attack_bool)
           NavMeshAgent_.SetDestination(_target);

            if (Coroutine_Update_target != null)
                StopCoroutine(Update_target_coroutine());

        }
    }


    IEnumerator Update_target_coroutine()//Обновление реакции ИИ
    {

        while (Target && Control_bool)
        {
            if (Vector3.Distance(Target.position, My_transform.position) > Distance_attack)
            {
                New_target_move(Target.position);
            }
            //else if (Check_visual() && Check_look_rotation())
            else if (Check_look_rotation(Target))
            {
                Distance_target_attack_event.Invoke();
            }
            else
            {
                New_target_move(Target.position);
            }

            yield return new WaitForSeconds(0.4f);
        }

    }


    /// <summary>
    /// Создать случайную точку для передвижения к ней
    /// </summary>
    public Vector3 Nav_random_point_target(float _radius)
    {
        bool get_correct_point_bool = false;//Сгенерировалась ли корректная точка (до которой можно добраться)
        Vector3 test_new_point = Vector3.zero;

        bool fatal_error = false;
        int step_alert = 0;//Сколько раз понадобилось для решения

        while (!get_correct_point_bool)
        {
            NavMeshHit nav_hit;

            NavMesh.SamplePosition(Random.insideUnitSphere * _radius + transform.position, out nav_hit, _radius, NavMesh.AllAreas);
            test_new_point = nav_hit.position;

            if (Check_path_comlete(test_new_point))
                get_correct_point_bool = true;
            else if(step_alert > 100)
            {
                get_correct_point_bool = true;
                fatal_error = true;
                Debug.Log("Фатальная ошибка, не смог найти путь!((");
            }

            step_alert++;
        }

        if (!fatal_error)
        {
            New_target_move(test_new_point);
        }

        return test_new_point;
    }


    public override void End_attack()
    {
        base.End_attack();

        if (Alive_bool)
        {
            End_attack_event.Invoke();
        }
    }


    /// <summary>
    /// Остановить движение или продолжить
    /// </summary>
    /// <param name="_activity">Остановить?</param>
    public void Stop_move_activity(bool _activity)
    {
        NavMeshAgent_.isStopped = _activity;
        //NavMeshAgent_.speed = _activity ? 0 : Speed_movement_default;

        if (_activity)
        {
            if (Coroutine_Update_target != null)
                StopCoroutine(Update_target_coroutine());

            NavMeshAgent_.velocity = new Vector3(NavMeshAgent_.velocity.x * 0.2f, NavMeshAgent_.velocity.y, NavMeshAgent_.velocity.z * 0.2f); //new Vector3(0, NavMeshAgent_.velocity.y, 0);
        }
        
    }

    /// <summary>
    /// Погиб
    /// </summary>
    public override void Dead()
    {

        Stop_move_activity(true);
        NavMeshAgent_.enabled = false;
        Attack_bool = false;

        if (Game_administrator.Instance != null)
            Game_administrator.Instance.Remove_enemy_list(transform);

        base.Dead();

    }



    #endregion



    #region Дополнительные методы
    /// <summary>
    /// Дополнительный доворот в сторону цели
    /// </summary>
    public void Additional_rotation_look()
    {
        var direction = (Target.position - transform.position).normalized;
        direction.y = 0f;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 0.8f);
    }

    /// <summary>
    /// Дополнительное движение назад, что бы не стоять вплотную к цели
    /// </summary>
    public void Additional_move_back()
    {
        if ((Vector3.Distance(Target.position, My_transform.position)) < Distance_attack * 0.6f)
        {
            transform.position -= transform.forward * (Speed_movement * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
            Gizmos.color = new Color(1,0,1,0.2f);
            Gizmos.DrawSphere(transform.position, Radius_detect);
    }
    #endregion


    #region Проверяющие методы
    /// <summary>
    /// Проверить визуально на наличие препятсвий от целей
    /// </summary>
    /// <param name="_target">Цель</param>
    /// <returns>Ответ</returns>
    public bool Check_visual(Transform _target)
    {
        bool result = false;

        Ray ray = new Ray(Head.position, My_transform.forward);

        RaycastHit hit;

        if (Physics.Linecast(Head.position, _target.position, out hit))
        {
            if (hit.transform.tag != "Player")
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Проверить, повёрнут ли в сторону цели
    /// </summary>
    /// <param name="_target">Цель</param>
    /// <returns>Ответ</returns>
    public bool Check_look_rotation(Transform _target)
    {
        bool result = false;

        Vector3 direction = _target.position - My_transform.position;
        Quaternion qua = Quaternion.LookRotation(direction);

        if (Quaternion.Angle(My_transform.rotation, qua) <= 10f)
        {
            result = true;
        }

        return result;
    }


    /// <summary>
    /// Проверить, можно ли дойти до этой точки
    /// </summary>
    /// <param name="_target">Конечная точка</param>
    /// <returns>Результат</returns>
    protected bool Check_path_comlete(Vector3 _target)
    {
        bool result_bool = false;

        //if(NavMeshAgent_.isOnNavMesh)
        NavMeshAgent_.CalculatePath(_target, NavMeshPath_);

        if (NavMeshPath_.status == NavMeshPathStatus.PathComplete)
        {
            result_bool = true;
        }

        return result_bool;
    }


    /// <summary>
    /// Проверяет есть ли препятствие между 2-мя точками
    /// </summary>
    /// <param name="_target_1">1 Точка</param>
    /// <param name="_target_2">2 Точка</param>
    /// <returns>Результат</returns>
    public bool Check_no_obstacle_in_way(Vector3 _target_1, Vector3 _target_2)
    {
        bool result_bool = true;

        NavMeshHit NavMeshHit_;//Для проверки препятствий на невмеше

        if (NavMesh.Raycast(_target_1, _target_2, out NavMeshHit_, NavMesh.AllAreas))
        {
            result_bool = false;
        }

        return result_bool;
    }


    /// <summary>
    /// Узнать расстояние между двумя точками
    /// </summary>
    /// <param name="_target_1"></param>
    /// <param name="_target_2"></param>
    /// <returns></returns>
    public float Find_out_Remaining_distance(Vector3 _target_1, Vector3 _target_2)
    {
        float result_distance = 0;

        Vector3[] corners = NavMeshAgent_.path.corners;

        if(corners.Length > 2)
        {
            for(int x = 1; x < corners.Length; x++)
            {
                Vector2 previous = new Vector2(corners[x - 1].x, corners[x - 1].z);
                Vector2 current = new Vector2(corners[x].x, corners[x].z);

                result_distance += Vector2.Distance(previous, current);
            }
        }
        else
        {
            result_distance = NavMeshAgent_.remainingDistance;

            if(result_distance == 0 && Vector3.Distance(_target_1, _target_2) > NavMeshAgent_.stoppingDistance + 1)
            {
                result_distance = Vector3.Distance(_target_1, _target_2);
                Debug.Log("Не смог сразу определить растояние, нужно исправление!");
            }
            
        }

        return result_distance;
    }

    #endregion

}
