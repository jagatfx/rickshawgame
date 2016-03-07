using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;           
    public CameraControl m_CameraControl;   
    public Text m_MessageText;
	public Text m_ScoreText;
    public GameObject m_PlayerPrefab;         
	public PlayerManager[] m_Players;           

    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;


    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllPlayers();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllPlayers()
    {
		for (int i = 0; i < m_Players.Length; i++)
        {
			m_Players[i].m_Instance =
				Instantiate(m_PlayerPrefab, m_Players[i].m_SpawnPoint.position, m_Players[i].m_SpawnPoint.rotation) as GameObject;
			m_Players[i].m_PlayerNumber = i + 1;
			m_Players[i].Setup();
			PlayerCustomer pc = m_Players[0].m_Instance.GetComponent<PlayerCustomer>();
			m_Players [i].m_PlayerCustomer = pc;
        }
    }


    private void SetCameraTargets()
    {
		Transform[] targets = new Transform[m_Players.Length];

        for (int i = 0; i < targets.Length; i++)
        {
			targets[i] = m_Players[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(MissionStarting());
        yield return StartCoroutine(MissionPlaying());
        yield return StartCoroutine(MissionEnding());

//        SceneManager.LoadScene(0);
        StartCoroutine(GameLoop());
    }


	private IEnumerator MissionStarting()
    {
		ResetAll ();
		DisablePlayerControl ();

		m_CameraControl.SetStartPositionAndSize ();

		m_RoundNumber++;
		m_MessageText.text = "MISSION " + m_RoundNumber;
		updateScore ();

        yield return m_StartWait;
    }


	private void updateScore()
	{
		// TODO: do not hard-code for player1
		m_ScoreText.text = "Player 1 $"+m_Players[0].CurrentMoney;		
	}


	private IEnumerator MissionPlaying()
    {
		EnablePlayerControl ();
		m_MessageText.text = string.Empty;
		while (!MissionEndingCondition ()) {
			updateScore ();
			yield return null;	
		}
    }


	private IEnumerator MissionEnding()
    {
		DisablePlayerControl ();
		string message = EndMessage ();
		m_MessageText.text = message;
			
        yield return m_EndWait;
    }


	private bool MissionEndingCondition()
    {
        return false;
    }

    private string EndMessage()
    {
        string message = "GAME OVER!";
        return message;
    }


    private void ResetAll()
    {
		for (int i = 0; i < m_Players.Length; i++)
        {
			m_Players[i].Reset();
        }
    }


    private void EnablePlayerControl()
    {
		for (int i = 0; i < m_Players.Length; i++)
        {
			m_Players[i].EnableControl();
        }
    }


    private void DisablePlayerControl()
    {
		for (int i = 0; i < m_Players.Length; i++)
        {
			m_Players[i].DisableControl();
        }
    }

}
