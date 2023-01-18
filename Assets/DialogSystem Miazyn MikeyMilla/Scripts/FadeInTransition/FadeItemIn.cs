using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeItemIn : MonoBehaviour
{
    Color fadeInColor;
    Image image;

    void Start()
    {
        fadeInColor = new Color(1, 1, 1, 0);
        image = GetComponent<Image>();
        image.color = fadeInColor;
        StartCoroutine(FadeColorIn());
    }

    IEnumerator FadeColorIn()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, i);

            yield return null;
        }
        Destroy(this);
    }

    [ContextMenu("Fade")]
    public void TestFade()
    {
        fadeInColor = new Color(1, 1, 1, 0);
        image = GetComponent<Image>();
        image.color = fadeInColor;
        StartCoroutine(TestFadeColorIn());
    }

    IEnumerator TestFadeColorIn()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, i);

            yield return null;
        }
    }
}