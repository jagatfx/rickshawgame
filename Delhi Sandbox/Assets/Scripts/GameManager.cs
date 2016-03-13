using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float startDelay = 3f;         
    public float endDelay = 3f;           
    public CameraControl cameraControl;   
    public Text messageText;
	public Text scoreText;
	public Text fareText;
	public Text moneyText;
    public GameObject playerPrefab;         
	public PlayerManager[] players;           

    private int missionNumber;              
    private WaitForSeconds startWait;     
    private WaitForSeconds endWait;


    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnAllPlayers();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllPlayers()
    {
		for (int i = 0; i < players.Length; i++)
        {
			players[i].m_Instance =
				Instantiate(playerPrefab, players[i].spawnPoint.position, players[i].spawnPoint.rotation) as GameObject;
			players[i].playerNumber = i + 1;
			players[i].Setup();
			PlayerCustomer pc = players[0].m_Instance.GetComponent<PlayerCustomer>();
			players [i].playerCustomer = pc;
        }
    }


    private void SetCameraTargets()
    {
		Transform[] targets = new Transform[players.Length];

        for (int i = 0; i < targets.Length; i++)
        {
			targets[i] = players[i].m_Instance.transform;
        }

        cameraControl.targets = targets;
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

		cameraControl.SetStartPositionAndSize ();

		missionNumber++;
		messageText.text = "MISSION " + missionNumber;
		updateScore ();

        yield return startWait;
    }


	private void updateScore()
	{
		// TODO: do not hard-code for player1
		scoreText.text = "Player 1 $"+players[0].playerCustomer.CurrentMoney;
		FarePlayerController fpc = players [0].playerCustomer.farePlayerController;
		moneyText.text = "Money: $" + fpc.money;
		if (null != fpc.fare) {
			moneyText.text += " Charge: $" + fpc.charge;
			fareText.text = "Fare type: " + fpc.fare.type;
		} else
		{
			fareText.text = "";
		}
		if (null != fpc.fareResponse) 
		{
			fareText.text += " \"" + fpc.fareResponse.verbal + "\"";
		}
	}


	private IEnumerator MissionPlaying()
    {
		EnablePlayerControl ();
		messageText.text = string.Empty;
		while (!MissionEndingCondition ()) {
			updateScore ();
			yield return null;	
		}
    }


	private IEnumerator MissionEnding()
    {
		DisablePlayerControl ();
		string message = EndMessage ();
		messageText.text = message;
			
        yield return endWait;
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
		for (int i = 0; i < players.Length; i++)
        {
			players[i].Reset();
        }
    }


    private void EnablePlayerControl()
    {
		for (int i = 0; i < players.Length; i++)
        {
			players[i].EnableControl();
        }
    }


    private void DisablePlayerControl()
    {
		for (int i = 0; i < players.Length; i++)
        {
			players[i].DisableControl();
        }
    }

}
