using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour {

    public static BGMManager instance = null;
    public AudioSource bgmSource;
    public AudioClip normalBGM;
    public AudioClip carryingCustomer;

    // Use this for initialization
    void Awake () {
        // Init singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("Destroying extra BGMManager instance.");
            Destroy(gameObject);
        }
        bgmSource.clip = normalBGM;
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
