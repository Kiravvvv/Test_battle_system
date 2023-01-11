using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[AddComponentMenu("Sych scripts / Game / Camera control")]
[DisallowMultipleComponent]
public class Camera_control : MonoBehaviour
{

    public State_machine_camera State_Machine;
    public Camera_free_rotation_state Free_rotation_st;
    public Camera_character_rotation_state Character_rotation_st;

    [field: Tooltip("�������� ��������")]
    [field: SerializeField]
    public float Speed_rotation { get; private set; } = 1.2f;

    [Tooltip("���� ��������, ������������� � ����������� ������ Cinemachine, �� ������� ����� ��������� ������.")]
    [field: SerializeField]
    public GameObject CinemachineCameraTarget { get; private set; } = null;

    [field: Tooltip("������ ����������")]
    [field: SerializeField]
    public Input_player Input { get; private set; } = null;

    
    [Tooltip("������������ �� Y �� ������, � ������ ���������")]
    [SerializeField]
    bool Y_character_rotation_bool = false;

    [field: ShowIf(nameof(Y_character_rotation_bool))]
    [field: SerializeField]
    public Transform Character_rotation { get; private set; } = null;

    [Tooltip("�� ������� �������� ����� ������� ������")]
    public float TopClamp = 70.0f;

    [Tooltip("�� ������� �������� ����� �������� ������")]
    public float BottomClamp = -30.0f;

    [Tooltip("�������������� ������� ��� ��������������� ������. ������� ��� ������ ��������� ��������� ������ � ��������������� ���������.")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("��� �������� ��������� ������ �� ���� ����")]
    public bool LockCameraPosition = false;

    Camera Cam = null;

    private void Awake()
    {
        Initialized_State_machine();
    }

    private void Start()
    {
        Cam = Game_administrator.Instance.Player_administrator.Cam;


    }

    private void LateUpdate()
    {
        if (!Y_character_rotation_bool)
        {
            if (State_Machine.Current_State != Free_rotation_st)
                State_Machine.Change_State(Free_rotation_st);
        }
        else
        {
            if (State_Machine.Current_State != Character_rotation_st)
                State_Machine.Change_State(Character_rotation_st);
        }

        State_Machine.Current_State.Camera_rotation();
    }


    /// <summary>
    /// ������������� ������ ���������
    /// </summary>
    protected virtual void Initialized_State_machine()
    {
        State_Machine = new State_machine_camera();

        Free_rotation_st = new Camera_free_rotation_state(this, State_Machine);
        Character_rotation_st = new Camera_character_rotation_state(this, State_Machine);

        State_Machine.Initialize(Free_rotation_st);
    }


}
