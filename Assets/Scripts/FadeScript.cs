using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    private bool fadeIn = false;
    private bool fadeOut = false;
    [SerializeField] private float fadeOutSpeed = 1f; // Adjust speed if needed
    public float FadeOutSpeed
    {
        get { return fadeOutSpeed; }
        set { fadeOutSpeed = value; }
    }
    [SerializeField] private float fadeInSpeed = 1f; // Adjust speed if needed
    public float FadeInSpeed
    {
        get { return fadeInSpeed; }
        set { fadeInSpeed = value; }
    }

    // Called from the animation event at 0.4 seconds
    public void StartFadeIn()
    {
        // Set the alpha to 0 before starting the fade in
        myUIGroup.alpha = 0;
        fadeIn = true;
        fadeOut = false;
    }

    // Call this to fade out the UI
    public void StartFadeOut()
    {
        // Set the alpha to 1 before starting the fade out
        myUIGroup.alpha = 1;
        fadeOut = true;
        fadeIn = false;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime * fadeInSpeed;
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
                myUIGroup.alpha -= Time.deltaTime * fadeOutSpeed;
                if (myUIGroup.alpha <= 0)
                {
                    myUIGroup.alpha = 0;
                    fadeOut = false;
                }
            }
        }
    }
    void Start()
    {
        myUIGroup.alpha = 0;
    }
}

