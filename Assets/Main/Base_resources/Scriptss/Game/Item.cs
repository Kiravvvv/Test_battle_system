//������� ��������� � �������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Sych scripts / Game / Item")]
[DisallowMultipleComponent]
public class Item : MonoBehaviour
{
    
    [Tooltip("Id �������� ��� ����, ��� �� ��������� �� ��� ��������������")]
    [field: SerializeField]
    public Vector2 Index { get; private set; } = new Vector2(-1, 0);
}
