using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePanelUImanager : MonoBehaviour
{
    [SerializeField] FadeScript statsFadeIn;
    [SerializeField] BookPanel Book;
    void Start()
    {
       
       Book.StartStatsFadeIn += () => startStatsFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startStatsFadeIn()
    {
        statsFadeIn.StartFadeIn();
        
    }
    public void startStatsFadeOut()
    {
        statsFadeIn.StartFadeOut();
    }
}
