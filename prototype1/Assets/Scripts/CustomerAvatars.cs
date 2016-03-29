using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Types of events which have customer sound effects.
/// </summary>
public enum CustAudioTypes { Hail, Miss, HappyDropoff, NeutralDropoff, UnhappyDropoff };

/// <summary>
/// All of the attributes that make up a unique customer type.
/// </summary>
[System.Serializable]
public class CustomerAvatar
{
    public List<AudioClip> hail;
    public List<AudioClip> pass;
    public List<AudioClip> happyDropoff;
    public List<AudioClip> neutralDropoff;
    public List<AudioClip> unhappyDropoff;
}

/// <summary>
/// Handles Audio/Visual aspects of customers. Currently limited to playing audio.
/// </summary>
public class CustomerAvatars : MonoBehaviour{

    public static CustomerAvatars instance = null;
    public List<CustomerAvatar> customerAvatars;

    private AudioSource audioSource;

    void Awake()
    {
        // Init singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource> ();
        }
        else if (instance != this)
        {
            Debug.Log("Destroying extra CustomerAvatars instance.");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Returns a random avatar id.
    /// </summary>
    /// <returns></returns>
    public static int RandomId()
    {
        return Random.Range(0, instance.customerAvatars.Count);
    }

    /// <summary>
    ///  Plays a specific sound effect for the customer id
    /// </summary>
    /// <param name="id">Avatar ID to play sound effects for</param>
    /// <param name="type">Type of sound effect to play</param>
    public static void Play(int id, CustAudioTypes type)
    {
        // Only switch up the audio and play it if nothing is already playing.
        // If events are too close, the second one may not be played.
        // ToDo? Multiple channels to allow for more than 1 customer to talk? Or space them out better? Or just leave alone?
        if (!instance.audioSource.isPlaying)
        {
            AudioClip clip = null;
            CustomerAvatar avatar = instance.customerAvatars [id];
            switch (type)
            {
            case CustAudioTypes.Hail:
                clip = avatar.hail[Random.Range (0, avatar.hail.Count)];
                break;
            case CustAudioTypes.Miss:
                clip = avatar.pass[Random.Range(0, avatar.pass.Count)];
                break;
            case CustAudioTypes.HappyDropoff:
                clip = avatar.happyDropoff[Random.Range(0, avatar.happyDropoff.Count)];
                break;
            case CustAudioTypes.NeutralDropoff:
                clip = avatar.neutralDropoff[Random.Range(0, avatar.neutralDropoff.Count)];
                break;
            case CustAudioTypes.UnhappyDropoff:
                clip = avatar.unhappyDropoff[Random.Range(0, avatar.unhappyDropoff.Count)];
                break;
            default:
                Debug.Log("No " + type + " audio clip for id=" + id);
                break;
            }
            if (clip)
            {
                instance.audioSource.PlayOneShot(clip, 1.0f);
            }
        }
    }
}
