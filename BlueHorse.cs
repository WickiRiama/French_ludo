using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BlueHorse : MonoBehaviour
{
	StateManager stateManager;
	bool isMoving;

	public Tile startingTile;
	Tile currentTile;

	Vector3 targetPosition;
	Vector3 targetRotation;
	Vector3 velocityPosition;
	float velocityRotation;
	float smoothTime = 0.25f;

	Tile[] path;
	int pathIndex = 0;

	// Start is called before the first frame update
	void Start()
	{
		stateManager = GameObject.FindObjectOfType<StateManager>();
		isMoving = false;
		setTarget(this.currentTile);
	}

	// Update is called once per frame
	void Update()
	{
		if (isMoving)
		{
			if (Vector3.Distance(this.transform.position, targetPosition) > 0.05f)
			{
				this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocityPosition, smoothTime);
				float angleY = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetRotation.y, ref velocityRotation, smoothTime);
				this.transform.rotation = Quaternion.Euler(0, angleY, 0);
			}
			else
			{
				if (path != null && pathIndex < path.Length)
				{
					setTarget(path[pathIndex]);
					pathIndex++;
				}
				else
				{
					isMoving = false;
					stateManager.isDoneMoving = true;
				}
			}
		}
	}

	void OnMouseUp()
	{
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
				// if (targetTile.GetType() =)
				targetTile = targetTile.nextTiles[0];
			}
			path[i] = targetTile;
		}
	}

	void setTarget(Tile tile)
	{
		if (!tile)
		{
			targetPosition = this.transform.position;
			targetRotation = this.transform.rotation.eulerAngles;
		}
		else
		{
			targetPosition = tile.transform.position;
			targetRotation = tile.transform.rotation.eulerAngles;
		}
		velocityPosition = Vector3.zero;
		velocityRotation = 0f;
		currentTile = tile;
	}
}
