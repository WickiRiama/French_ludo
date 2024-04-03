using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAI
{
	StateManager stateManager;
	DiceRoller diceRoller;

	public PlayerAI()
	{
		stateManager = GameObject.FindObjectOfType<StateManager>();
		diceRoller = GameObject.FindObjectOfType<DiceRoller>();
	}

	public void PlayTurn()
	{
		if (stateManager.isDoneChangingPlayer)
		{
			if (!stateManager.isDoneRolling)
			{
				RollDice();
				return ;
			}
			if (!stateManager.isDoneCheckingPath)
			{
				stateManager.CheckLegalPath();
				stateManager.IsMoveImpossible();
				return;
			}
			if (!stateManager.isDoneClicking)
			{
				ChooseHorse();
				stateManager.isDoneClicking = true;
				return;
			}
		}
	}

	void RollDice()
	{
		diceRoller.RollTheDice();
	}

	Horse[] GetMovableHorses()
	{
		List<Horse> movableHorses = new();

		foreach (Horse horse in stateManager.horses)
		{
			if (horse.canMove)
			{
				movableHorses.Add(horse);
			}
		}
		return movableHorses.ToArray();
	}

	void ChooseHorse()
	{
		Horse[] movableHorses = GetMovableHorses();
		if (movableHorses.Length == 0)
		{
			return ;
		}
		movableHorses[Random.Range(0, movableHorses.Length)].DoTheMove();
	}
}
