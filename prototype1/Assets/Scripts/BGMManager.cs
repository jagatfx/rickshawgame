using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour {

    public static BGMManager instance;
    public AudioClip normalBGM;
    public AudioClip carryingCustomer;

    private AudioSource bgmSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            bgmSource = GetComponent<AudioSource> ();
            bgmSource.clip = normalBGM;
            bgmSource.Play ();
        }
        else if (instance != this)
        {
            Debug.Log("Destroying extra BGMManager instance.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (MissionTimer.TimeRemaining () < 30.0f)
        {
            if (instance.bgmSource.clip != instance.carryingCustomer)
            {
                instance.bgmSource.clip = instance.carryingCustomer;
                instance.bgmSource.Play();
            }
        }
        else
        {
            if (instance.bgmSource.clip != instance.normalBGM)
            {
                instance.bgmSource.clip = instance.normalBGM;
                instance.bgmSource.Play();
            }
        }
    }

    public static void PickupCustomer()
    {
        instance.bgmSource.clip = instance.carryingCustomer;
        instance.bgmSource.Play();
    }

    public static void DropoffCustomer()
    {
        instance.bgmSource.clip = instance.normalBGM;
        instance.bgmSource.Play();
    }

}
