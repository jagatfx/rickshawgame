using System;
using UnityEngine;

[Serializable]
public class PlayerManager
{
    public Transform spawnPoint;

    [HideInInspector] public int playerNumber;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public PlayerCustomer playerCustomer;

    public void Setup()
    {
    }


    public void DisableControl()
    {
        // TODO
    }


    public void EnableControl()
    {
        // TODO
    }


    public void Reset()
    {
        m_Instance.transform.position = spawnPoint.position;
        m_Instance.transform.rotation = spawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
