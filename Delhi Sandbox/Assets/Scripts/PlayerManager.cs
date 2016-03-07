using System;
using UnityEngine;

[Serializable]
public class PlayerManager
{
	public Transform m_SpawnPoint;

	[HideInInspector] public int m_PlayerNumber;
	[HideInInspector] public GameObject m_Instance;
	[HideInInspector] public PlayerCustomer m_PlayerCustomer;

	public float CurrentMoney { get { return m_PlayerCustomer.CurrentMoney; }}


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
		m_Instance.transform.position = m_SpawnPoint.position;
		m_Instance.transform.rotation = m_SpawnPoint.rotation;

		m_Instance.SetActive(false);
		m_Instance.SetActive(true);
	}
}
