using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_interface_UI : Singleton<Player_interface_UI>
{

    [Tooltip("Показатель сдоровья игрока")]
    [SerializeField]
    Image Health_image = null;

    [Tooltip("Аниматор получения урона")]
    [SerializeField]
    Animator Damage_anim = null;

    [Tooltip("Картинка показывающий, что можно взаимодействовать")]
    [SerializeField]
    Image Interaction_info = null;

    [Tooltip("Прицел игрока")]
    [SerializeField]
    Image Aim_image = null;


    /// <summary>
    /// Получить урон (анимация мигания)
    /// </summary>
    public void Damage_anim_effect()
    {
        Damage_anim.Play("Damage");
    }

    /// <summary>
    /// Изменить показатели жизней
    /// </summary>
    /// <param name="_value">Значение от 0 до 1</param>
    public void Player_Health_info(float _value)
    {
        Health_image.fillAmount = _value;
    }


    /// <summary>
    /// Показать, что можно взаимодействовать с объектом
    /// </summary>
    /// <param name="_active">Включить или отключить</param>
    public void Activity_Interaction_image(bool _active)
    {
        Interaction_info.gameObject.SetActive(_active);
    }


    /// <summary>
    /// Получить случайную точку в картинке прицела
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
