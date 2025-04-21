using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
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
}
