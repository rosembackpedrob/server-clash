using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GodoiItem : MonoBehaviour
{
    public GodoiItemInfo itemInfo;
    public GameObject itemGameObject;

    public abstract void Use();

}
