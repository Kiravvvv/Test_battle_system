using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_interface_UI : Singleton<Player_interface_UI>
{

    [Tooltip("���������� �������� ������")]
    [SerializeField]
    Image Health_image = null;

    [Tooltip("�������� ��������� �����")]
    [SerializeField]
    Animator Damage_anim = null;

    [Tooltip("�������� ������������, ��� ����� �����������������")]
    [SerializeField]
    Image Interaction_info = null;

    [Tooltip("������ ������")]
    [SerializeField]
    Image Aim_image = null;


    /// <summary>
    /// �������� ���� (�������� �������)
    /// </summary>
    public void Damage_anim_effect()
    {
        Damage_anim.Play("Damage");
    }

    /// <summary>
    /// �������� ���������� ������
    /// </summary>
    /// <param name="_value">�������� �� 0 �� 1</param>
    public void Player_Health_info(float _value)
    {
        Health_image.fillAmount = _value;
    }


    /// <summary>
    /// ��������, ��� ����� ����������������� � ��������
    /// </summary>
    /// <param name="_active">�������� ��� ���������</param>
    public void Activity_Interaction_image(bool _active)
    {
        Interaction_info.gameObject.SetActive(_active);
    }


    /// <summary>
    /// �������� ��������� ����� � �������� �������
    /// </summary>
    public Vector3 Aim_spread_random_point
    {
        get
        {

            Vector3 point = Aim_image.rectTransform.position;//new Vector3(Random.Range(-Aim_image.rectTransform.sizeDelta.x, Aim_image.rectTransform.sizeDelta.x), Random.Range(-Aim_image.rectTransform.sizeDelta.y, Aim_image.rectTransform.sizeDelta.y), Aim_image.rectTransform.position.z);

            point.x += Random.Range(-Aim_image.rectTransform.sizeDelta.x / 2, Aim_image.rectTransform.sizeDelta.x / 2);
            point.y += Random.Range(-Aim_image.rectTransform.sizeDelta.y / 2, Aim_image.rectTransform.sizeDelta.y / 2);

            return point;
        }

    }

}
