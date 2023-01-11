using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_HC_UI : Singleton<Game_HC_UI>
{
    [Tooltip("Стартовая панель")]
    [SerializeField]
    GameObject Start_panel = null;

    [Tooltip("Игровая панель (во время активной игры)")]
    [SerializeField]
    GameObject Game_panel = null;

    [Tooltip("Панель конца игры (победа)")]
    [SerializeField]
    GameObject End_panel_win = null;

    [Tooltip("Панель конца игры (поражение)")]
    [SerializeField]
    GameObject End_panel_lose = null;



    [Space(20)]
    [Header("Игрока")]

    [Tooltip("Показатель жизней")]
    [SerializeField]
    Image Health_image = null;

    [Tooltip("Прицел игрока")]
    [SerializeField]
    Image Aim_image = null;

    private void Start()
    {
        All_off_panels();
        Start_panel.SetActive(true);
    }

    /// <summary>
    /// Выключить все панели
    /// </summary>
    void All_off_panels()
    {
        Start_panel.SetActive(false);
        Game_panel.SetActive(false);
        End_panel_win.SetActive(false);
        End_panel_lose.SetActive(false);
    }


    public void Start_game()
    {
        Game_administrator.Instance.Start_game();
        All_off_panels();
        Game_panel.SetActive(true);
    }


    /// <summary>
    /// Изменить показатель жизней
    /// </summary>
    /// <param name="_value">значение от 0 до 1</param>
    public void Change_health(float _value)
    {
        Health_image.fillAmount = _value;
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
