using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class GameSceneUI : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI scoreText;

    public GameObject carObject;
    void Start()
    {
        
    }

    void Update()
    {
        if(carObject != null)
        {
            int score = (int) (carObject.transform.position.x * 100.0f);
            scoreText.text = $"SCORE: {score}";
        }

        float fps = 1.0f / Time.deltaTime;
        fpsText.text = "FPS: " + fps.ToString("0.00");
    }
}
