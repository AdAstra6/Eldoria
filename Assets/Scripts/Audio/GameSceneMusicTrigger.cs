using UnityEngine;

public class GameSceneMusicTrigger : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.gameTheme != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameTheme);
        }
        else
        {
            Debug.LogWarning("AudioManager or gameTheme is not assigned.");
        }
    }
}
