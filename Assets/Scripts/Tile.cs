using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Tile : MonoBehaviour
{
	public Tile[] nextTiles;
	public Horse currentHorse;
	public bool isStair;
	protected PlayerId owner;

	// Start is called before the first frame update
	void Start()
	{
		owner = (PlayerId)GetComponent<Variables>().declarations["Owner"];
		isStair = false;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public Tile GetNextTile(Horse horse)
	{
		if (nextTiles.Length == 0)
		{
			return null;
		}
		if (nextTiles.Length == 2 && this.owner == horse.owner)
		{
			return nextTiles[1];
		}
		if (nextTiles.Length == 4)
		{
			for (int i = 0; i < 4; i++)
			{
				if (nextTiles[i].currentHorse == null)
				{
					return nextTiles[i];
				}
			}
		}
		return nextTiles[0];
	}

	public virtual bool CanComeHere(Horse horse, bool isFinalTile)
	{
		if (!currentHorse)
		{
			return true;
		}
		if (isFinalTile && currentHorse.owner != horse.owner)
		{
			return true;
		}
		return false;
	}
}
