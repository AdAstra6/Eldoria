using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    private bool fadeIn = false;
    private bool fadeOut = false;
    [SerializeField] private float fadeSpeed = 1f; // Adjust speed if needed
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);
        StartFadeIn();
    }

    // Called from the animation event at 0.4 seconds
    public void StartFadeIn()
    {
        fadeIn = true;
        fadeOut = false;
    }

    // Call this to fade out the UI
    public void StartFadeOut()
    {
        fadeOut = true;
        fadeIn = false;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime * fadeSpeed;
                if (myUIGroup.alpha >= 1)
                {
                    myUIGroup.alpha = 1;
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (myUIGroup.alpha > 0)
            {
                myUIGroup.alpha -= Time.deltaTime * fadeSpeed;
                if (myUIGroup.alpha <= 0)
                {
                    myUIGroup.alpha = 0;
                    fadeOut = false;
                }
            }
        }
    }
}
