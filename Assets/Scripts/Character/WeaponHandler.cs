using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;
    [SerializeField] float waitForChange;

    private Weapon _currentWeapon;
    private int _currentWeaponIndex = 0;

    private bool _inChangeWeapon = false;

    private void Start()
    {
        _currentWeapon = weapons[0];
        _currentWeapon.ActiveWeapon();
    }

    public void SelectWeapon(int weaponIndex) 
    {
        if(!_inChangeWeapon && _currentWeaponIndex != weaponIndex) 
        {
            _inChangeWeapon = true;
            _currentWeaponIndex = weaponIndex;
            StartCoroutine(ChangeWeapon(weaponIndex));
        } 
    }

    public IEnumerator ChangeWeapon(int weaponIndex) 
    {
        yield return new WaitForSeconds(waitForChange);
        _currentWeapon.DesactiveWeapon();
        _currentWeapon = weapons[weaponIndex];
        _currentWeapon.ActiveWeapon();
        _inChangeWeapon = false;
    }

    public void FireWeapon() 
    {
        if(!_inChangeWeapon)
            _currentWeapon.Fire();
    }
}
