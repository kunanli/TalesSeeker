using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : baseSingleton<ItemDataManager>
{
    [SerializeField]
    public List<baseItemData> ItemDataObject;
}
