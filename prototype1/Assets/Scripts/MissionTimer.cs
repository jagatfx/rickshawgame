using UnityEngine;
using System.Collections;

public class MissionTimer : MonoBehaviour
{

    public static float missionTime = 90.0f;

    private static bool timeElapsed;
    private static bool isCountingDown;
    private static float timeRemaining;

    /// <summary>
    /// Singleton meant to manage timer.
    /// </summary>
    public static MissionTimer instance = null;

    private static void SetupTimer ()
    {
        timeElapsed = false;
        isCountingDown = false;
        timeRemaining = missionTime;
        timeRemaining = missionTime;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupTimer ();
        }
        else if (instance != this)
        {
            Debug.Log("Destroying extra MissionTimer instance.");
            Destroy(gameObject);
        }
    }

    void Update ()
    {
        if (!timeElapsed && isCountingDown)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0.0f)
            {
                timeElapsed = true;
            }
        }
    }

    public static float TimeRemaining ()
    {
        return timeRemaining;
    }

    public static void ResetTimer ()
    {
        SetupTimer ();
    }

    public static void StartCountdown ()
    {
        isCountingDown = true;
    }

    public static void PauseCountdown ()
    {
        isCountingDown = false;
    }

    public static void AddTime (float addTime)
    {
        timeRemaining += addTime;
    }

    public static void SubtractTime (float subTime)
    {
        timeRemaining -= subTime;
    }

    public static bool IsTimeElapsed ()
    {
        return timeElapsed;
    }
}

