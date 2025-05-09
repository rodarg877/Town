using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class WeaponAttributes : ScriptableObject
{
    public int maxBullets;
    public int damage;
    public float weaponDistance;
}
