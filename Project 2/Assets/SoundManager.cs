using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public AudioClip stepSound;
    public AudioClip explosionSound;

    private AudioSource _source;
    public static SoundManager Instance = null;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _source = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this);
        }
    }
  

  
    public void Step ()
    {
        _source.PlayOneShot(stepSound);
    }

    public void Explosion()
    {
        _source.PlayOneShot(explosionSound);
    }
}
