using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] float splashScreenLength;

    bool inputPressed;

    void Start()
    {
        StartCoroutine(HandleSplashScreen());
    }

    private void WaitForInput() => inputPressed = true;

    private IEnumerator HandleSplashScreen()
    {
        inputPressed = false;

        Controller.OnInteract += WaitForInput;
        yield return new WaitUntil(() => inputPressed);
        Controller.OnInteract -= WaitForInput;

        StartCoroutine(SceneTools.TransitionToScene(SceneTools.NextSceneIndex));
    }
}
