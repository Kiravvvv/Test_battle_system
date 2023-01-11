using UnityEngine;

[System.Serializable]
public class Merge_item
{
    [field: SerializeField]
    public string Name { get; private set; } = "Название крафта";

    [Tooltip("Первый предмет для смешивания")]
    [field: SerializeField]
    public Vector2 First_item_id { get; private set; } = Vector2.zero;

    [Tooltip("Второй предмет для смешивания")]
    [field: SerializeField]
    public Vector2 Second_item_id { get; private set; } = Vector2.zero;

    [Tooltip("Какой предмет получаем в итоге")]
    [field: SerializeField]
    public Vector2 Final_item_id { get; private set; } = Vector2.zero;

}
