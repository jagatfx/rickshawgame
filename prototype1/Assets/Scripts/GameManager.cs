using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float startDelay = 3f;
    public float endDelay = 3f;
    public CameraControl cameraControl;
    public Text messageText;
    public Text playerText;
    public Text fareText;
    public Text scoresText;
    public Text timerText;
    public GameObject playerPrefab;
    public GameObject aiPlayerPrefab;
    public PlayerManager[] players;
    public int menuSceneId = 1;

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
            PlayerManager player = players [i];
            if (player.IsAIPlayer())
            {
                player.m_Instance = Instantiate(aiPlayerPrefab, players[i].spawnPoint.position,
                    players[i].spawnPoint.rotation) as GameObject;
            }
            else
            {
                player.m_Instance = Instantiate(playerPrefab, players[i].spawnPoint.position,
                    players[i].spawnPoint.rotation) as GameObject;
            }
            player.playerNumber = i + 1;
            player.Setup();
            PlayerCustomer pc = player.m_Instance.GetComponent<PlayerCustomer>();
            player.playerCustomer = pc;
        }
    }


    private void SetCameraTargets()
    {
        /// for now just track Player 1
//        Transform[] targets = new Transform[players.Length];
        Transform[] targets = new Transform[1];

        targets[0] = players[0].m_Instance.transform;
//        for (int i = 0; i < targets.Length; i++)
//        {
//            targets[i] = players[i].m_Instance.transform;
//            break;
//        }

        cameraControl.targets = targets;
        cameraControl.SetPlayerTransform (players [0].m_Instance.transform);
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
        MissionTimer.StartCountdown ();
        updateScore ();

        yield return startWait;
    }


    private void updateScore()
    {
        // TODO: figure out active player num for when more than one player
        timerText.text = "Time Remaining: "+FareTools.roundTwoDecimals(MissionTimer.TimeRemaining ());
        playerText.text = "Player 1";
        scoresText.text = "";
        for (int i = 0; i < players.Length; i++)
        {
            FarePlayerController fpc = players [i].playerCustomer.farePlayerController;
            scoresText.text += "Player "+(i+1)+": $" + fpc.money;
            if (null != fpc.fare)
            {
                scoresText.text += " Charge: $" + fpc.charge + "\n";
            }
            else
            {
                if (i == 0)
                {
                    fareText.text = "";
                }
                scoresText.text += "\n";
            }
            if (i == 0 && null != fpc.fareResponse)
            {
                fareText.text += " \"" + fpc.fareResponse.verbal + "\"";
            }
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
        return MissionTimer.IsTimeElapsed ();
    }

    private string EndMessage()
    {
        string message = "MISSION OVER!";
        int winningPlayer = 1;
        float winningScore = 0f;
        for (int i = 0; i < players.Length; i++)
        {
            FarePlayerController fpc = players [i].playerCustomer.farePlayerController;
            if (fpc.money > winningScore)
            {
                winningPlayer = i + 1;
                winningScore = fpc.money;
            }
        }
        if (winningScore < 1.0f)
        {
            message += "\nNobody " + winningPlayer + " Wins...";
        }
        else
        {
            message += "\nPlayer " + winningPlayer + " Wins!";
        }

        return message;
    }


    private void ResetAll()
    {
        MissionTimer.ResetTimer ();
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
