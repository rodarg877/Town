using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NewFollowCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 0.7f;
    public float height = 0.7f;
    public float followSpeed = 5f;
    public float rotationSpeed = 120f;
    public float maxMouseAngle = 30f;

    private float currentYaw = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        // Calculamos ángulo del mouse respecto al centro de la pantalla
        Vector2 mousePos = Input.mousePosition;
        float screenCenterX = Screen.width / 2f;
        float offsetX = (mousePos.x - screenCenterX) / screenCenterX; // -1 a 1
        float angle = offsetX * 70f;

        // Si pasa cierto ángulo, giramos la cámara
        if (Mathf.Abs(angle) > maxMouseAngle)
        {
            currentYaw += offsetX * rotationSpeed * Time.deltaTime;
        }

        // Calculamos posición de la cámara detrás del personaje
        Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        Vector3 desiredPosition = target.position + offset;

        // Mover y rotar suavemente
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 0.3f); // Mira al pecho/cabeza
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public Quaternion GetCameraYaw()
    {
        return Quaternion.Euler(0f, currentYaw, 0f);
    }

}

