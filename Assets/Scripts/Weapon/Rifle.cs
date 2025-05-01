using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [Header("Rifle Attributes")]
    [SerializeField] internal GameObject originForFire;

    public override void Fire()
    {
        Debug.Log("Rifle Fire");
    }
}
