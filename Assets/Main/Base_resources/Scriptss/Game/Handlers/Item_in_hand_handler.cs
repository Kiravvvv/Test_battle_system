//������ ��� ����� ������ � ���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Sych scripts / Game / Handlers / Item in hand handler")]
[DisallowMultipleComponent]
public class Item_in_hand_handler : MonoBehaviour
{

    [Tooltip("�������� ��� ��������")]
    public UnityEvent<Vector2> Change_type_item_event = new UnityEvent<Vector2>();

    [Tooltip("������ ��������� ������� ������ ��������")]
    [SerializeField]
    Item[] Item_hands_array = new Item[0];

    Item Active_item = null;//������� ������� ������ � �����

    private void Awake()
    {
        foreach(Item item in Item_hands_array)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void Change_hands_item(Vector2 _id_item)
    {

        if (Active_item != null)
            Active_item.gameObject.SetActive(false);

        for (int x = 0; x < Item_hands_array.Length; x++)
        {
            if (Item_hands_array[x].Index == _id_item)
            {
                Active_item = Item_hands_array[x];
                Active_item.gameObject.SetActive(true);
                break;
            }
        }

        Change_type_item_event.Invoke(_id_item);

    }
}
