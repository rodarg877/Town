using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCaracterMove : MonoBehaviour
{
    public float moveSpeed = 0.7f;
    public NewFollowCamera camFollow;
    [SerializeField] private Animator _animator;
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.freezeRotation = true;

        
    }

    void Update()
    {
        HandleInput();
        UpdateAnimations();
        WeaponController();
        RotateTowardMouse();
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void HandleInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        Quaternion yaw = camFollow.GetCameraYaw();
        moveDirection = yaw * inputDir;
    }

    private void MoveCharacter()
    {
        Vector3 move = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    private void RotateTowardMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            Vector3 lookPoint = hit.point;
            lookPoint.y = transform.position.y; // ignorar altura para rotación plana

            Vector3 direction = (lookPoint - transform.position).normalized;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                rb.MoveRotation(targetRot);
            }
        }
    }

    private void UpdateAnimations()
    {
        Vector3 localDir = Quaternion.Inverse(camFollow.GetCameraYaw()) * moveDirection;
        _animator.SetFloat("SpeedX", localDir.x);
        _animator.SetFloat("SpeedY", localDir.z);
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
}