using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingCourotine());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator LoadingCourotine()
    {
        yield return new WaitForSeconds(9f);
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");

    }
}