using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class StateManager : MonoBehaviour
{
	public bool isDoneChangingPlayer;
	public bool isDoneRolling;
	public bool isDoneCheckingPath;
	public bool isOutlineOn;
	public bool isDoneClicking;
	public bool isDoneMoving;
	public bool isDoneReturningStable;

	public int diceValue;
	public PlayerId currentPlayer;
	int nbPlayers = 4;
	PlayerAI[] isAIPlayer;
	public int[] score;
	public Horse[] horses;
	DiceRoller dice;
	CameraPivot cameraPivot;

	// Start is called before the first frame update
	void Start()
	{
		isDoneChangingPlayer = true;
		isDoneRolling = false;
		isDoneCheckingPath = false;
		isOutlineOn = false;
		isDoneClicking = false;
		isDoneMoving = false;
		isDoneReturningStable = true;

		diceValue = 0;
		currentPlayer = PlayerId.BLUE;
		dice = FindObjectOfType<DiceRoller>();
		cameraPivot = FindObjectOfType<CameraPivot>();
		isAIPlayer = new PlayerAI[nbPlayers];
		isAIPlayer[0] = null;
		isAIPlayer[1] = new PlayerAI();
		isAIPlayer[2] = new PlayerAI();
		isAIPlayer[3] = new PlayerAI();
		score = new int[nbPlayers];
	}

	// Update is called once per frame
	void Update()
	{
		if (isAIPlayer[(int)currentPlayer] != null)
		{
			if (isDoneChangingPlayer)
			{
				isAIPlayer[(int)currentPlayer].PlayTurn();
			}
		}
		else if (isDoneChangingPlayer && isDoneRolling)
		{
			if (!isDoneCheckingPath)
			{
				CheckLegalPath();
				IsMoveImpossible();
			}
			if (isDoneChangingPlayer && isDoneRolling && isDoneCheckingPath && isDoneClicking && isDoneMoving && isDoneReturningStable)
			{
					NewTurn();
			}
		}
	}

	public void NewTurn()
	{
		
		PlayerId winner = WhoWins();
		if (winner != PlayerId.NONE)
		{
			Debug.Log(winner.ToString() + " has won !!!");
			return ;
		}

		isDoneChangingPlayer = false;
		isDoneRolling = false;
		isDoneCheckingPath = false;
		isDoneClicking = false;
		isDoneMoving = false;
		isDoneReturningStable = true;
		for (int i = 0; i < 16; i++)
		{
			horses[i].canMove = false;
		}
		if (isAIPlayer[(int)currentPlayer] != null)
		{
			isAIPlayer[(int)currentPlayer].ResetPlayer();
		}
		//Replay in case of 6
		if (diceValue != 6)
		{
			currentPlayer = (PlayerId)(((int)currentPlayer + 1) % nbPlayers);
		}
		cameraPivot.MoveCamera();
		dice.ResetDice();
	}

	public void CheckLegalPath()
	{
		for (int i = 0; i < 16; i++)
		{
			if (horses[i].owner == currentPlayer)
			{
				horses[i].CreatePath();
			}
		}
		isDoneCheckingPath = true;
	}

	public void IsMoveImpossible()
	{
		int canMove = 0;
		for (int i = 0; i < 16; i++)
		{
			if (horses[i].owner == currentPlayer && horses[i].canMove)
			{
				canMove++;
			}
		}
		if (canMove == 0)
		{
			StartCoroutine(MoveImpossibleCoroutine());
		}
	}

	public void PauseAI()
	{
		StartCoroutine(PlayerAIPauseCoroutine());
	}

	IEnumerator MoveImpossibleCoroutine()
	{
		yield return new WaitForSeconds(1f);
		NewTurn();
	}

	IEnumerator PlayerAIPauseCoroutine()
	{
		yield return new WaitForSeconds(1f);
		isAIPlayer[(int)currentPlayer].isDonePausing = true;
	}

	public void DisableOutline()
	{
		for (int i = 0; i < 16; i++)
		{
			if (horses[i].owner == currentPlayer)
			{
				horses[i].outline.enabled = false;
			}
		}
	}

	PlayerId WhoWins()
	{
		for (int i = 0; i < this.score.Length; i++)
		{
			if (score[i] == 4)
			{
				return (PlayerId) i;
			}
		}
		return PlayerId.NONE;
	}
}

