using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class StateManager : MonoBehaviour
{
	public GameObject[] horsePrefabs;
	public StableList[] stableTiles;
	public Tile[] startingTiles;

	public bool isDoneChangingPlayer;
	public bool isDoneRolling;
	public bool isDoneCheckingPath;
	public bool isDoneClicking;
	public bool isDoneMoving;
	public bool isDoneReturningStable;

	public PlayerId currentPlayer;
	public PlayerId winner;
	public PlayerAI[] players;
	public Horse[] horses;
	DiceRoller dice;
	public int diceValue;
	CameraPivot cameraPivot;
	public int[] score;

	readonly int nbPlayers = 4;

	// Start is called before the first frame update
	void Start()
	{
		// InitializeTurn();
		currentPlayer = FirstPlayer();
		winner = PlayerId.NONE;
		players = CreatePlayers();
		horses = CreateHorses();
		dice = FindObjectOfType<DiceRoller>();
		// diceValue = 0;
		cameraPivot = FindObjectOfType<CameraPivot>();
		score = new int[nbPlayers];
		NewTurn();
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

	void InitializeTurn()
	{
		isDoneChangingPlayer = true;
		isDoneRolling = false;
		isDoneCheckingPath = false;
		isDoneClicking = false;
		isDoneMoving = false;
		isDoneReturningStable = true;
	}

	PlayerId FirstPlayer()
	{
		int first = Random.Range(0, 4);
		Debug.Log("Random Player is " + (PlayerId)first);
		return (PlayerId)first;
	}

	PlayerAI[] CreatePlayers()
	{
		PlayerAI[] players = new PlayerAI[nbPlayers];
		for (int i = 0; i < nbPlayers; i++)
		{
			if (StartMenu.menu.isAIPlayer[i])
			{
				players[i] = new PlayerAI();
			}
		}
		return players;
	}

	Horse[] CreateHorses()
	{
		List<Horse> horses = new List<Horse>();
		for (int color = 0; color < 4; color++)
		{
			for (int stable = 0; stable < StartMenu.menu.nbHorses; stable++)
			{
				GameObject horseObject = Instantiate(horsePrefabs[color], stableTiles[color].stableTiles[stable].GetTransform());
				Horse horse = horseObject.GetComponent<Horse>();
				horse.startingTile = startingTiles[color];
				horse.startStable = stableTiles[color].stableTiles[stable];
				horses.Add(horse);
			}
		}
		return horses.ToArray();
	}

	public void NewTurn()
	{

		winner = WhoWins();
		if (winner != PlayerId.NONE)
		{
			Debug.Log(winner.ToString() + " has won !!!");
			return;
		}

		isDoneChangingPlayer = false;
		isDoneRolling = false;
		isDoneCheckingPath = false;
		isDoneClicking = false;
		isDoneMoving = false;
		isDoneReturningStable = true;
		for (int i = 0; i < horses.Length; i++)
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
		for (int i = 0; i < horses.Length; i++)
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
		for (int i = 0; i < horses.Length; i++)
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
		for (int i = 0; i < horses.Length; i++)
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
			if (score[i] == StartMenu.menu.nbHorses)
			{
				return (PlayerId)i;
			}
		}
		return PlayerId.NONE;
	}
}

[System.Serializable]
public class StableList
{
	public StableTile[] stableTiles;

}