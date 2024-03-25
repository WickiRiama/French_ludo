using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Horse : MonoBehaviour
{
	// First tile of pathway
	public Tile startingTile;

	// State
	StateManager stateManager;
	bool isMoving;
	Tile currentTile;

	// Start
	Transform startStable;

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
		isMoving = false;
		startStable = this.transform;
		setTarget(null);
	}

	void Update()
	{
		if (isMoving)
		{
			// Not arrived yet
			if (Vector3.Distance(this.transform.position, target.position) > 0.05f)
			{
				this.transform.position = Vector3.SmoothDamp(this.transform.position, target.position, ref velocityPosition, smoothTime);
				float angleY = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, target.eulerAngles.y, ref velocityRotation, smoothTime);
				this.transform.rotation = Quaternion.Euler(0, angleY, 0);
			}
			// Arrived at target
			else
			{
				// Go to next target in the path
				if (path != null && pathIndex < path.Length)
				{
					setTarget(path[pathIndex]);
					pathIndex++;
				}
				// Arrived at the end of the path
				else
				{
					if (TryGetComponent(out Variables id))
					{
						Debug.Log("Id " + id.declarations["Owner"]);
					}
					else
					{
						Debug.Log("Nope");
					}
					isMoving = false;
					stateManager.isDoneMoving = true;
				}
			}
		}
	}

	void OnMouseUp()
	{
		// The dice has been rolled, and no other hosre has been selected
		if (stateManager.isDoneRolling && !stateManager.isDoneClicking)
		{
			stateManager.isDoneClicking = true;
			CreatePath();
			this.isMoving = true;
		}
	}

	private void CreatePath()
	{
		int nbMoves = stateManager.diceValue + 1;
		Tile targetTile = currentTile;
		path = new Tile[nbMoves];
		pathIndex = 0;

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
			path[i] = targetTile;
		}
	}

	void setTarget(Tile target)
	{
		if (target == null)
		{
			this.target = this.startStable;
		}
		else
		{
			this.target = target.transform;
		}
		velocityPosition = Vector3.zero;
		velocityRotation = 0f;
		currentTile = target;
	}
}
