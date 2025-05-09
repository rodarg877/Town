using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float velocity = 2f;

    // Niveles de zoom: pares de altura y distancia
    private Vector3[] zoomOffsets = new Vector3[]
    {
        new Vector3(0f, 0.7f, -0.7f),
        new Vector3(0f, 0.9f, -0.9f),
        new Vector3(0f, 1f, -1f),
        new Vector3(0f, 1.3f, -1.3f)
    };

    private int currentZoomIndex = 0;

    private Vector3 currentOffset;

    void Start()
    {
        currentOffset = zoomOffsets[currentZoomIndex];
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll < 0f)
        {
            currentZoomIndex = Mathf.Min(currentZoomIndex + 1, zoomOffsets.Length - 1);
        }
        else if (scroll > 0f)
        {
            currentZoomIndex = Mathf.Max(currentZoomIndex - 1, 0);
        }

        // Suaviza el cambio de offset
        currentOffset = Vector3.Lerp(currentOffset, zoomOffsets[currentZoomIndex], Time.deltaTime * 5f);

        if (target != null)
        {
            Vector3 desiredPosition = target.position + currentOffset;
            transform.position = desiredPosition;
        }
    }
}
