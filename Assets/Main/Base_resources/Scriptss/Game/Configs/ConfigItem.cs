using UnityEngine;


[System.Serializable]
public class ConfigItem 
{
    [field: SerializeField]
    public string Name_category { get; private set; } = "Название категории";

    [field: SerializeField]
    public Item_parameter[] Item_array { get; private set; } = new Item_parameter[0];
}


[System.Serializable]
public class Item_parameter
{
    [field: SerializeField]
    public string Name { get; private set; } = null;

    [field: SerializeField]
    public Sprite Sprite { get; private set; } = null;

    [field: SerializeField]
    public GameObject Prefab { get; private set; } = null;

    [field: SerializeField]
    [field: Min(0)]
    public int Price { get; private set; } = 0;

    [field: SerializeField]
    [field: Min(0)]
    public int Damage { get; private set; } = 0;

    [field: SerializeField]
    [field: Min(0)]
    public int Armor { get; private set; } = 0;

    [field: SerializeField]
    [field: Min(0)]
    public int Addionation_health { get; private set; } = 0;
}
