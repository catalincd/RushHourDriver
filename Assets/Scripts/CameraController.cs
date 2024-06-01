using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Vector3 offset;
    public Vector3 targetOffset;
    public float cameraRollTime = 750.0f;
    private bool isDead = false;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
        if(target == null)
        {
            if(!isDead)
            {
                isDead = true;
                StartCoroutine(TimeoutCoroutine(cameraRollTime / 1000.0f));
            }
            
            return;
        }

        transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
        transform.LookAt(target.position + targetOffset);
    }

    IEnumerator TimeoutCoroutine(float timeoutDuration)
    {
        yield return new WaitForSeconds(timeoutDuration);
        OnTimeout();
    }

    void OnTimeout()
    {
       SceneManager.LoadScene("Scenes/MainScene");
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
