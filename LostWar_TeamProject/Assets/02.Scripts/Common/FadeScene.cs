﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour
{
    [SerializeField] private GameObject fadeObject;
    [SerializeField] private CanvasGroup fadeCg;
    [SerializeField] private string sceneName;
    private float fadeDuration = 3.0f;

    private void Awake()
    {
        fadeObject = this.gameObject;
        fadeCg = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        if (fadeCg.alpha == 0)
        {
            Debug.Log("fade out");
            float rate = 1.0f / fadeDuration;
            float progress = 0.0f;
            while (progress <= 1.0f)
            {
                fadeCg.alpha = Mathf.Lerp(0f, 1f, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
            fadeCg.alpha = 1.0f;
            switch(sceneName)
            {
                case "Start":
                    SceneManager.LoadScene("Level1");
                    break;
                case "Finish":
                    SceneManager.LoadScene("Finish");
                    break;
                case "Main":
                    SceneManager.LoadScene("Main");
                    break;
            }
        }
        else if (fadeCg.alpha == 1)
        {
            Debug.Log("fade in");
            float rate = 1.0f / fadeDuration;
            float progress = 0.0f;
            while (progress <= 1f)
            {
                fadeCg.alpha = Mathf.Lerp(1f, 0f, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
            fadeCg.alpha = 0f;
            fadeObject.SetActive(false);
        }
    }
}