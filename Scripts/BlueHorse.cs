using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BlueHorse : MonoBehaviour
{
	public Tile startingTile;

	DiceRoller diceRoller;
	Tile currentTile;
	Tile[] path;

	// Start is called before the first frame update
	void Start()
	{
		diceRoller = GameObject.FindObjectOfType<DiceRoller>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void OnMouseUp()
	{
		Move();
	}

	private void Move()
	{
		CreatePath();
		// Debug.Log(path);
	}

	private void CreatePath()
	{
		Tile targetTile = currentTile;
		int nbMoves = diceRoller.value + 1;

		for (int i = 0; i < nbMoves; i++)
		{
			if (!targetTile)
			{
				targetTile = startingTile;
			}
			else
			{
				targetTile = targetTile.nextTiles[0];
			}
			// path.Append(targetTile);
			// Debug.Log("Path" + path[i].name);
		}

		this.transform.position = targetTile.transform.position;
		currentTile = targetTile;
	}
}
