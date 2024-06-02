using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class SettingsController : MonoBehaviour
{
    public TextMeshProUGUI anisotropicText;
    public TextMeshProUGUI aliasingText;
    public TextMeshProUGUI shadowsText;
    public TextMeshProUGUI vsyncText;

    private string[] anisotropicValues = new string[] {"OFF", "ON"};
    private string[] aliasingValues = new string[] {"OFF", "x2", "x4"};
    private string[] shadowsValues = new string[] {"OFF", "LOW", "HIGH"};
    private string[] vsyncValues = new string[] {"OFF", "ON"};

    private int anisotropicCount = 0;
    private int aliasingCount = 0;
    private int shadowsCount = 1;
    private int vsyncCount = 0;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void TriggerAnisotropic()
    {
        anisotropicCount++;
        anisotropicCount %= anisotropicValues.Length;

        anisotropicText.text = anisotropicValues[anisotropicCount];

        if(anisotropicCount == 0) QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
    	if(anisotropicCount == 1) QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
    }

    public void TriggerAntialiasing()
    {
        aliasingCount++;
        aliasingCount %= aliasingValues.Length;

        QualitySettings.antiAliasing = aliasingCount * 2;

        aliasingText.text = aliasingValues[aliasingCount];
    }

    public void TriggerShadows()
    {
        shadowsCount++;
        shadowsCount %= shadowsValues.Length;

        shadowsText.text = shadowsValues[shadowsCount];

        if(shadowsCount == 2)
        {
            QualitySettings.shadows = ShadowQuality.All;
            QualitySettings.shadowResolution = ShadowResolution.High;
            QualitySettings.shadowDistance = 50.0f;
            QualitySettings.shadowCascades = 4;
            QualitySettings.shadowProjection = ShadowProjection.CloseFit;
        }

        if(shadowsCount == 1)
        {
            QualitySettings.shadows = ShadowQuality.HardOnly;
            QualitySettings.shadowResolution = ShadowResolution.Low;
            QualitySettings.shadowDistance = 10.0f;
            QualitySettings.shadowCascades = 1;
            QualitySettings.shadowProjection = ShadowProjection.StableFit;
        }

        if(shadowsCount == 0)
        {
            QualitySettings.shadows = ShadowQuality.Disable;
        }

    }

    public void TriggerVsync()
    {
        vsyncCount++;
        vsyncCount %= vsyncValues.Length;

        QualitySettings.vSyncCount = vsyncCount;

        vsyncText.text = vsyncValues[vsyncCount];
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
