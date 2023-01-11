using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "System_configs", menuName = "Sych SO / New System Configs", order = 0)]
public class System_configs : ScriptableObject
{
    #region Singleton
    private static System_configs instance;
    public static System_configs Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load("System_configs") as System_configs;

                if (instance == null)
                {
                    Debug.LogError("System_configs Instance is null");
                }

            }

            return instance;
        }
    }
    #endregion

    public ConfigItem[] Items_category_array = new ConfigItem[0];

    public Merge_item[] Merge_item_array = new Merge_item[0];

    public Player_config Player_config = new Player_config();

    //public ConfigItem Weapon, Armor, Item;
}

