using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BookPanel : MonoBehaviour
{
    public Action StartStatsFadeIn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Startstatsfadein()
    {
        StartStatsFadeIn.Invoke();
    }
}
