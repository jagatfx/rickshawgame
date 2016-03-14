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

    public AudioSource audioSource;
    public List<CustomerAvatar> customerAvatars;

    void Awake()
    {
        // Init singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
            switch (type)
            {
                case CustAudioTypes.Hail:
                    instance.audioSource.clip = instance.customerAvatars[id].hail[Random.Range(0, instance.customerAvatars[id].hail.Count)];
                    break;
                case CustAudioTypes.Miss:
                    instance.audioSource.clip = instance.customerAvatars[id].pass[Random.Range(0, instance.customerAvatars[id].pass.Count)];
                    break;
                case CustAudioTypes.HappyDropoff:
                    instance.audioSource.clip = instance.customerAvatars[id].happyDropoff[Random.Range(0, instance.customerAvatars[id].happyDropoff.Count)];
                    break;
                case CustAudioTypes.NeutralDropoff:
                    instance.audioSource.clip = instance.customerAvatars[id].neutralDropoff[Random.Range(0, instance.customerAvatars[id].neutralDropoff.Count)];
                    break;
                case CustAudioTypes.UnhappyDropoff:
                    instance.audioSource.clip = instance.customerAvatars[id].unhappyDropoff[Random.Range(0, instance.customerAvatars[id].unhappyDropoff.Count)];
                    break;
                default:
                    Debug.Log("No " + type + " audio clip for id=" + id);
                    break;
            }
            instance.audioSource.Play();
        }
    }
}
