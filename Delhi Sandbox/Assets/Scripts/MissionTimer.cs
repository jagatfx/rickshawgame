using UnityEngine;
using System.Collections;

public class MissionTimer : MonoBehaviour
{

	public float missionTime = 30.0f;

	private bool timeElapsed;
	private bool isCountingDown;
	private float timeRemaining;

	void SetupTimer ()
	{
		timeElapsed = false;
		isCountingDown = false;
		timeRemaining = missionTime;
		timeRemaining = missionTime;
	}

	// Use this for initialization
	void Start ()
	{
		SetupTimer ();
	}
	
	// Update is called once per frame
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

	public float TimeRemaining ()
	{
		return timeRemaining;
	}

	public void ResetTimer ()
	{
		SetupTimer ();
	}

	public void StartCountdown ()
	{
		isCountingDown = true;
	}

	public void PauseCountdown ()
	{
		isCountingDown = false;
	}

	public void AddTime (float addTime)
	{
		timeRemaining += addTime;
	}

	public void SubtractTime (float subTime)
	{
		timeRemaining -= subTime;
	}

	public bool IsTimeElapsed ()
	{
		return timeElapsed;
	}
}

