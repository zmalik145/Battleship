using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitTarget;
    [SerializeField] private AudioClip missfire;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public void PlayMissFire()
    {
        if (audioSource)
        {
            audioSource.PlayOneShot(missfire);
        }
    }

    public void PlayHitTarget()
    {
        if (audioSource)
        {
            audioSource.PlayOneShot(hitTarget);
        }
    }

    public void GameWin()
    {
        if (audioSource)
        {
            audioSource.PlayOneShot(win);
        }
    }

    public void GameLose()
    {
        if (audioSource)
        {
            audioSource.PlayOneShot(lose);
        }
    }
}
