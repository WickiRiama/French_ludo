using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class StateManager : MonoBehaviour
{
	public bool isDoneChangingPlayer;
	public bool isDoneRolling;
	public bool isDoneCheckingPath;
	public bool isDoneClicking;
	public bool isDoneMoving;
	public bool isDoneReturningStable;

	public int diceValue;
	public PlayerId currentPlayer;
	int nbPlayers = 4;
	public Horse[] horses;
	DiceRoller dice;

	// Start is called before the first frame update
	void Start()
	{
		isDoneChangingPlayer = false;
		isDoneRolling = false;
		isDoneCheckingPath = false;
		isDoneClicking = false;
		isDoneMoving = false;
		isDoneReturningStable = true;

		diceValue = 0;
		currentPlayer = PlayerId.BLUE;
		dice = FindObjectOfType<DiceRoller>();
	}

	// Update is called once per frame
	void Update()
	{
		if (isDoneChangingPlayer && isDoneRolling)
		{
			if (!isDoneCheckingPath)
			{
				CheckLegalPath();
				isDoneCheckingPath = true;
			}
			if (IsMoveImpossible() || (isDoneClicking && isDoneMoving && isDoneReturningStable))
			{
				NewTurn();
			}
		}
	}

	private void NewTurn()
	{
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

		//Replay in case of 6
		if (diceValue != 6)
		{
			currentPlayer = (PlayerId)(((int)currentPlayer + 1) % nbPlayers);
		}
		dice.ResetDice();
	}

	void CheckLegalPath()
	{
		for (int i = 0; i < 16; i++)
		{
			if (horses[i].owner == currentPlayer)
			{
				horses[i].CreatePath();
			}
		}
	}

	bool IsMoveImpossible()
	{
		int canMove = 0;
		for (int i = 0; i < 16; i++)
		{
			if (horses[i].owner == currentPlayer && horses[i].canMove)
			{
				canMove++;
			}
		}
		return canMove == 0;
	}
}

