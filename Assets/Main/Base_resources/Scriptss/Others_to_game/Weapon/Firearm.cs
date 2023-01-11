//���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[AddComponentMenu("Sych scripts / Game / Firearm")]
[DisallowMultipleComponent]
public class Firearm : Weapon
{

    [Tooltip("����� ������ ����")]
    [SerializeField]
    protected Transform Fire_point = null;

    [Tooltip("������ ����")]
    [SerializeField]
    protected Bullet Bullet_prefab = null;

    [Tooltip("������� � ����������� �� ������� UI �������� ������� ")]
    [SerializeField]
    bool Aim_image_spread_bool = true;

    [HideIf(nameof(Aim_image_spread_bool))]
    [Tooltip("������� �������� (���������������� ��������� ����������� �������� ���� ��������� �������� Fire_point)")]
    [SerializeField]
    float Spread = 0;

    float Spread_active = 0;//��� ������ � ���������

    protected int Damage = 0;

    Quaternion Default_rotation_Fire_point = Quaternion.identity;//����������� �������� ����� ������ ���� ��� ������

    protected int Attack_mode_id = 0;

    Transform Target = null;//���� �����

    Camera Cam = null;

    protected Vector3 Finale_point = Vector3.zero;//����� ���� �������� �������

    private void Start()
    {
        Cam = Game_administrator.Instance.Player_administrator.Cam;
        Spread_active = Spread;
        Default_rotation_Fire_point = Fire_point.localRotation;
    }

    protected void Fire_normal()
    {

        if (Damage > 0)
            Instantiate(Bullet_prefab, Fire_point.position, Fire_point.rotation).Specify_settings(Damage);
        else
            Instantiate(Bullet_prefab, Fire_point.position, Fire_point.rotation);
    }


    /// <summary>
    /// ����������
    /// </summary>
	[ContextMenu(nameof(Fire))]
    public void Fire()
    {
        if (Aim_image_spread_bool)
        {

            Vector3 point_screen_point = Game_HC_UI.Instance.Aim_spread_random_point;

            Ray ray = Cam.ScreenPointToRay(point_screen_point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Finale_point = hit.point;
                print(hit.transform.name);
            }
            else
            {
                Finale_point = ray.direction * 4000f;
            }
            Fire_point.transform.LookAt(Finale_point, Fire_point.up);
        }


        Fire_id(Attack_mode_id);

    }

    /// <summary>
    /// ������� ������� �������� ��������� ������ ����-����� (��� �������)
    /// </summary>
    protected virtual void Fire_id(int _id_attack)
    {
        if (gameObject.activeSelf)
        {
            switch (_id_attack)
            {
                default:
                    Fire_normal();
                    break;
            }

            Attack_mode_id = 0;
        }
    }

    /// <summary>
    /// ���������� � ��������� ����� ��� �������
    /// </summary>
    /// <param name="_damage"></param>
    public void Fire(int _damage)
    {
        Damage = _damage;
        Fire();
    }

    /// <summary>
    /// �������� �� �����������
    /// </summary>
    /// <param name="_point_end">�������� ����� (���� �������)</param>
    public void Fire(Vector3 _point_end)
    {
        Fire_point.transform.LookAt(_point_end, Fire_point.up);
        Fire_point.transform.eulerAngles += new Vector3(Random.Range(-Spread_active, Spread_active), Random.Range(-Spread_active, Spread_active), Fire_point.transform.localRotation.z);

        Fire();
    }

    /// <summary>
    /// �������� �� ����������� � ��������� �����
    /// </summary>
    /// <param name="_point_end">�������� ����� (���� �������)</param>
    /// <param name="_damage">����</param>
    public void Fire(Vector3 _point_end, int _damage)
    {
        Damage = _damage;
        Fire(_point_end);
    }

    /// <summary>
    /// ������� � ����� ���������
    /// </summary>
    /// <param name="_force_spead">���� �������� (�� 0 �� 1)</param>
    public void Fire(float _force_spead)
    {
        Spread_active = Spread * _force_spead;
        Fire_point.transform.localRotation = Default_rotation_Fire_point;
        Fire_point.transform.eulerAngles += new Vector3(Random.Range(-Spread_active, Spread_active), Random.Range(-Spread_active, Spread_active), Fire_point.transform.localRotation.z);
        Fire();
    }

    /// <summary>
    /// ������� � ����� ��������� � ��������� �����
    /// </summary>
    /// <param name="_force_spead">���� �������� (�� 0 �� 1)</param>
    /// <param name="_damage">����</param>
    public void Fire(float _force_spead, int _damage)
    {
        Damage = _damage;
        Fire(_force_spead);
    }

    /// <summary>
    /// ������� �� ����������� � � ����� ��������
    /// </summary>
    /// <param name="_point_end">�������� ����� (���� �������)</param>
    /// <param name="_force_spead">���� �������� (�� 0 �� 1)</param>
    public void Fire(Vector3 _point_end, float _force_spead)
    {
        Spread_active = Spread * _force_spead;
        Fire(_point_end);
    }

    /// <summary>
    /// ������� �� �����������, � ����� �������� � ���
    /// </summary>
    /// <param name="_point_end">�������� ����� (���� �������)</param>
    /// <param name="_force_spead">���� �������� (�� 0 �� 1)</param>
    /// <param name="_damage">����</param>
    public void Fire(Vector3 _point_end, float _force_spead, int _damage)
    {
        Damage = _damage;
        Fire(_point_end, _force_spead);
    }


    /// <summary>
    /// ������� ����� �����
    /// </summary>
    /// <param name="_id_mode">����� �����</param>
    public void Mode_attack(int _id_mode)
    {
        Attack_mode_id = _id_mode;
    }

    /// <summary>
    /// ����� ����
    /// </summary>
    /// <param name="_target">����</param>
    public void New_target(Transform _target)
    {
        Target = _target;
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void End_attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Activity(bool _activity)
    {
        throw new System.NotImplementedException();
    }
}
