using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class FadeToBlackSystem : SingletonMonoBehavior<FadeToBlackSystem>
{
    [SerializeField] bool startFullAplha;
    [SerializeField] bool autoFadeOut;
    private Image image;

    Dictionary<GameObject, bool> test;

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
        SetFadePercentage(startFullAplha ? 1 : 0);
        SceneManager.sceneLoaded += CheckForAutoFadeOut;
    }

    private void CheckForAutoFadeOut(Scene arg0, LoadSceneMode arg1)
    {
        if (autoFadeOut)
        {
            StartCoroutine(HandleStartOfScene());
        }
    }

    private void SetFadePercentage(float percentage)
    {
        var temp = image.color;
        temp.a = percentage;
        image.color = temp;
    }

    private IEnumerator HandleStartOfScene()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(TryCueFadeOutOfBlack(1.5f));
    }

    public static IEnumerator TryCueFadeInToBlack(float duration)
    {
        if(Instance != null)
        {
            yield return Instance.CueFadeInToBlack(duration);
        }
    }

    public static IEnumerator TryCueFadeOutOfBlack(float duration)
    {
        if (Instance != null)
        {
            yield return Instance.CueFadeOutOfBlack(duration);
        }
    }

    public IEnumerator CueFadeOutOfBlack(float duration)
    {
        SetFadePercentage(1);

        float startTime = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup - startTime < duration)
        {
            SetFadePercentage(1 - (Time.realtimeSinceStartup - startTime) / duration);
            yield return null;
        }

        SetFadePercentage(0);
    }

    public IEnumerator CueFadeInToBlack(float duration)
    {
        SetFadePercentage(0);

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            SetFadePercentage((Time.realtimeSinceStartup - startTime) / duration);
            yield return null;
        }

        SetFadePercentage(1);
    }
}
