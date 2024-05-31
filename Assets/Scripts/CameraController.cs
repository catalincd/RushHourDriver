using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Vector3 offset;
    public Vector3 targetOffset;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
        transform.LookAt(target.position + targetOffset);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
