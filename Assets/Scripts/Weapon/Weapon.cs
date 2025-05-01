using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    internal WeaponAttributes weaponAttributes;
    internal float _currentBullets;
    internal float _bulletsInCharger;

    [Header("Weapon Attributes")]
    [SerializeField] internal GameObject weaponInCharacter;
    [SerializeField] internal Animator anim;

    public abstract void Fire();
    public void ActiveWeapon()
    {
        gameObject.SetActive(true);
        weaponInCharacter.SetActive(false);
    }

    public void DesactiveWeapon()
    {
        gameObject.SetActive(false);
        weaponInCharacter.SetActive(true);
    }
}
