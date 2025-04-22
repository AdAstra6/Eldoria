using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePanelManager : MonoBehaviour
{
    [SerializeField] private Animator bookAnimator;
    [SerializeField] private ProfilePanelUImanager uimanager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextProfile()
    {
        // update profiles
        uimanager.startStatsFadeOut();
        bookAnimator.SetTrigger("RightFlipPage");
        uimanager.startStatsFadeIn();
    }
}
