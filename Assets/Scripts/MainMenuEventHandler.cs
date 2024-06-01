using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuEventHandler : MonoBehaviour
{
    public Canvas mainMenu;
    public Canvas creditsMenu;
    public Canvas optionsMenu;
    public MainMenuCamera cameraController;

    private bool started = false;

    public void GoToPlay()
    {
        if(started) return;
        started = true;    
        cameraController.DoBackRoll();

        StartCoroutine(TimeoutCoroutine(0.75f));
    }

    IEnumerator TimeoutCoroutine(float timeoutDuration)
    {
        yield return new WaitForSeconds(timeoutDuration);
        OnTimeout();
    }

    void OnTimeout()
    {
       SceneManager.LoadScene("Scenes/GameScene");
    }

    public void BackToMainMenu()
    {
        creditsMenu.enabled = false;
        optionsMenu.enabled = false;
        mainMenu.enabled = true;
    }

    public void GoToOptions()
    {
        creditsMenu.enabled = false;
        optionsMenu.enabled = true;
        mainMenu.enabled = false;
    }

    public void GoToCredits()
    {
        creditsMenu.enabled = true;
        optionsMenu.enabled = false;
        mainMenu.enabled = false;
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
