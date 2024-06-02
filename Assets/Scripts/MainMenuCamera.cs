using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCamera : MonoBehaviour
{
    public float rollTime = 500.0f;
    public float speed = 10.0f;
    public float maxLookAngle = 10.0f;
    public float lookStride = 1300;
    public float lookStrideVariability = 1.35f;
    public bool disableLooking = false;
    public bool randomizeLooking = false;
    private bool isRolling = false;
    private float rollStartTime = 0.0f;
    private bool lookLeft = false;
    private float lastLook = 0.0f;
    private float lastLookAngle = 0.0f;
    private float lastStride = 0.0f;

    private float yDown = -0.5f;

    public CanvasGroup canvasGroup;

    void Start()
    {
        
    }

    void Update()
    {
        float cameraRollBias = isRolling? Mathf.Sin(Mathf.Clamp((Time.unscaledTime * 1000.0f - rollStartTime) / rollTime, 0.0f, 1.0f) * Mathf.PI / 2.0f) : 0.0f;
        //float cameraRollBias = isRolling? (Time.unscaledTime * 1000.0f - rollStartTime / rollTime) : 0.0f;
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        //Debug.Log($"TIME: {Time.unscaledTime * 1000.0f} STIME: {rollStartTime}");

        //314D79
        canvasGroup.alpha = 1.0f - cameraRollBias;
        
        float currentAngle = Mathf.Lerp(0.0f, lastLookAngle, Mathf.Sin((Time.unscaledTime * 1000.0f - lastLook) / lastStride * Mathf.PI));
        currentAngle = Mathf.Clamp(currentAngle, 0.0f, maxLookAngle);
        currentAngle = lookLeft? currentAngle : -currentAngle;

        if(!disableLooking)
            transform.eulerAngles = new Vector3(30.0f - (120.0f * cameraRollBias), 90.0f + currentAngle, 0.0f);


        if(Time.unscaledTime * 1000.0f - lastLook < lastStride) return;
        
        lastLook = Time.unscaledTime * 1000.0f;
        lastLookAngle = maxLookAngle * Mathf.Lerp(0.5f, 1.0f, Random.value); 
        lastStride = lookStride * Mathf.Lerp(1.0f / lookStrideVariability, lookStrideVariability, Random.value);
        lookLeft = randomizeLooking? (Random.value > 0.5f) : !lookLeft;
    }

    public void DoBackRoll()
    {
        isRolling = true;
        rollStartTime = Time.unscaledTime * 1000.0f;
    }
}
