using System.Collections;
using System.Collections.Generic;
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
	}

	// Update is called once per frame
	void Update()
	{
		if (isDoneChangingPlayer && isDoneRolling)
		{
			if (!isDoneCheckingPath)
			{
				isDoneCheckingPath = true;
				for (int i = 0; i < 16; i++)
				{
					if (horses[i].owner == currentPlayer && !horses[i].isDoneCheckingPath)
					{
						isDoneCheckingPath = false;
						break;
					}
				}
				int canMoves = 0;
				for (int i = 0; i < 16; i++)
				{
					if (horses[i].owner == currentPlayer && horses[i].canMove)
					{
						canMoves++;
					}
				}
				if (canMoves == 0)
				{
					newTurn();
				}
			}
			else if (isDoneClicking && isDoneMoving && isDoneReturningStable)
			{
				newTurn();
			}
		}
	}

	private void newTurn()
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
			horses[i].isDoneCheckingPath = false;
		}

		//Replay in case of 6
		if (diceValue != 5)
		{
			currentPlayer = (PlayerId)(((int)currentPlayer + 1) % nbPlayers);
		}
	}
}
