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
	public PlayerAI[] players;
	public int[] score;
	public Horse[] horses;
	DiceRoller dice;
	CameraPivot cameraPivot;
	// StartMenu menu;

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
		players = new PlayerAI[nbPlayers];
		for (int i = 0; i < nbPlayers; i++)
		{
			if (StartMenu.menu.isAIPlayer[i])
			{
				players[i] = new PlayerAI();
			}
		}
		
		score = new int[nbPlayers];
	}

	// Update is called once per frame
	void Update()
	{
		if (StartMenu.menu.isAIPlayer[(int)currentPlayer])
		{
			if (isDoneChangingPlayer)
			{
				players[(int)currentPlayer].PlayTurn();
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
		if (StartMenu.menu.isAIPlayer[(int)currentPlayer])
		{
			players[(int)currentPlayer].ResetPlayer();
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
		players[(int)currentPlayer].isDonePausing = true;
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

