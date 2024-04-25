using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class StairTile : Tile
{
	public int value;
	StateManager stateManager;

	void Start()
	{
		owner = (PlayerId)GetComponent<Variables>().declarations["Owner"];
		stateManager = FindObjectOfType<StateManager>();
		isStair = true;
	}

	public override bool CanComeHere(Horse horse, bool isFinalTile)
	{
		if (currentHorse)
		{
			return false;
		}
		if (value != stateManager.diceValue)
		{
			return false;
		}
		return true;
	}

}
