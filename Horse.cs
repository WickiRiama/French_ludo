using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Horse : MonoBehaviour
{
	// State
	StateManager stateManager;
	public bool canMove;
	bool isMoving;
	bool isReturningHome;
	Tile currentTile;

	// Start
	public Tile startingTile;
	public StableTile startStable;
	public PlayerId owner;

	// Move
	Transform target;
	Tile[] path;
	int pathIndex = 0;

	// Animation
	Vector3 velocityPosition;
	float velocityRotation;
	float smoothTime = 0.25f;

	void Start()
	{
		stateManager = GameObject.FindObjectOfType<StateManager>();
		canMove = false;
		isMoving = false;
		isReturningHome = false;
		owner = (PlayerId)GetComponent<Variables>().declarations["Owner"];
		SetTarget(null);
		this.transform.SetPositionAndRotation(startStable.transform.position, startStable.transform.rotation);
	}

	void Update()
	{
		if (isMoving)
		{
			// Not arrived yet
			if (Vector3.Distance(this.transform.position, target.position) > 0.05f)
			{
				MoveTo(target);
			}
			// Arrived at target
			else
			{
				// Go to next target in the path
				if (path != null && pathIndex < path.Length)
				{
					SetTarget(path[pathIndex]);
					if (path[pathIndex].currentHorse)
					{
						path[pathIndex].currentHorse.ReturnHome();
					}
					pathIndex++;
				}
				// Arrived at the end of the path
				else
				{
					isMoving = false;
					canMove = false;
					if (this.currentTile)
					{
						this.currentTile.currentHorse = this;
					}
					stateManager.isDoneMoving = true;
				}
			}
		}
		else if (isReturningHome)
		{
			if (Vector3.Distance(this.transform.position, target.position) > 0.05f)
			{
				MoveTo(target);
			}
			// Arrived at target
			else
			{
				isReturningHome = false;
				stateManager.isDoneReturningStable = true;
			}
		}
	}

	void OnMouseUp()
	{
		// The dice has been rolled, and no other hosre has been selected
		if (stateManager.isDoneCheckingPath && !stateManager.isDoneClicking && stateManager.currentPlayer == owner && canMove)
		{
			stateManager.isDoneClicking = true;
			if (this.currentTile)
			{
				this.currentTile.currentHorse = null;
			}
			this.isMoving = true;
		}
	}

	public void CreatePath()
	{
		int nbMoves = stateManager.diceValue + 1;
		pathIndex = 0;
		Tile targetTile = currentTile;
		if (!targetTile)
		{
			if (nbMoves == 6)
			{
				path = new Tile[1];
				path[0] = startingTile;
				canMove = true;
			}
			return;
		}

		path = new Tile[nbMoves];
		for (int i = 0; i < nbMoves; i++)
		{
			targetTile = targetTile.nextTiles[0];
			if (!CanMoveTo(targetTile, i == nbMoves - 1))
			{
				return;
			}
			path[i] = targetTile;
		}
		canMove = true;
	}

	void SetTarget(Tile target)
	{
		if (target == null)
		{
			this.target = this.startStable.transform;
		}
		else
		{
			this.target = target.transform;
		}
		velocityPosition = Vector3.zero;
		velocityRotation = 0f;
		currentTile = target;
	}

	void MoveTo(Transform goal)
	{
		this.transform.position = Vector3.SmoothDamp(this.transform.position, goal.position, ref velocityPosition, smoothTime);
		float angleY = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, goal.eulerAngles.y, ref velocityRotation, smoothTime);
		this.transform.rotation = Quaternion.Euler(0, angleY, 0);
	}

	bool CanMoveTo(Tile tile, bool isLast)
	{
		if (!tile.currentHorse)
		{
			return true;
		}
		else if (isLast && tile.currentHorse.owner != this.owner)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void ReturnHome()
	{
		stateManager.isDoneReturningStable = false;
		SetTarget(null);
		this.isReturningHome = true;
	}
}
