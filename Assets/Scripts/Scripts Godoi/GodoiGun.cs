using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GodoiGun : GodoiItem
{
    public abstract override void Use();

    public GameObject bulletImpactPrefab;
}
