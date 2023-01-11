using UnityEngine;

[System.Serializable]
public class Merge_item
{
    [field: SerializeField]
    public string Name { get; private set; } = "�������� ������";

    [Tooltip("������ ������� ��� ����������")]
    [field: SerializeField]
    public Vector2 First_item_id { get; private set; } = Vector2.zero;

    [Tooltip("������ ������� ��� ����������")]
    [field: SerializeField]
    public Vector2 Second_item_id { get; private set; } = Vector2.zero;

    [Tooltip("����� ������� �������� � �����")]
    [field: SerializeField]
    public Vector2 Final_item_id { get; private set; } = Vector2.zero;

}
