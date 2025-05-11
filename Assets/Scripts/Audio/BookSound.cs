using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class BookSound : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        AudioManager.Instance.PlayScrollOpen();
    }
}