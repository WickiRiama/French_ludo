using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
	public bool isDoneRolling;
	public bool isDoneClicking;
	public bool isDoneMoving;
	public bool isDoneReturningStable;

	public int diceValue;
	public PlayerId currentPlayer;
	int nbPlayers = 4;

	// Start is called before the first frame update
	void Start()
	{
		isDoneRolling = false;
		isDoneClicking = false;
		isDoneMoving = false;
		isDoneReturningStable = true;

		diceValue = 0;
		currentPlayer = PlayerId.BLUE;
	}

	// Update is called once per frame
	void Update()
	{
		if (isDoneRolling && isDoneClicking && isDoneMoving && isDoneReturningStable)
		{
			newTurn();
		}
	}

	private void newTurn()
	{
		isDoneRolling = false;
		isDoneClicking = false;
		isDoneMoving = false;
		isDoneReturningStable = true;

		currentPlayer = (PlayerId)(((int)currentPlayer + 1) % nbPlayers);
	}
}
