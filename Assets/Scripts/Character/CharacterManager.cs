using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private Rigidbody _rig;

    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 10f;

    private Vector3 _movement;

    public void Update()
    {
        Movement();
        WeaponController();
        RotateTowardMouse();
    }

    private void FixedUpdate()
    {
        _rig.MovePosition(_rig.position + _movement * _moveSpeed * Time.fixedDeltaTime);
    }

    private void WeaponController() 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(1);
        }


        if (Input.GetMouseButton(0))
        {
            _animator.SetBool("Firing", true);
            _weaponHandler.FireWeapon();
        }
        else
        {
            _animator.SetBool("Firing", false);
        }
    }

    private void ChangeWeapon(int index) 
    {
        _animator.SetInteger("WeaponIndex", index);
        _weaponHandler.SelectWeapon(index);
    }

    private void Movement() 
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");

        _movement = _movement.normalized;

        _animator.SetFloat("SpeedX", _movement.x);
        _animator.SetFloat("SpeedY", _movement.z);
    }

    void RotateTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 lookPos = Vector3.zero;

        if (Physics.Raycast(ray, out hit, 100f)) 
        {
            lookPos = hit.point;
        }

        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;

        transform.LookAt(transform.position + lookDir, Vector3.up);
    }

}
