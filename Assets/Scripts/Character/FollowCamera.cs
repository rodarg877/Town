using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float velocity = 1f;

    public Vector3 offset = new Vector3(0f, 8f, -8f);

    void Update()
    {
        if (target != null) 
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, velocity);
        }
    }
}
