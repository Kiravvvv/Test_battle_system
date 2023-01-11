//Базовое понимание о объекте
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Sych scripts / Game / Item")]
[DisallowMultipleComponent]
public class Item : MonoBehaviour
{
    
    [Tooltip("Id предмета для того, что бы ссылаться на его характеристики")]
    [field: SerializeField]
    public Vector2 Index { get; private set; } = new Vector2(-1, 0);
}
