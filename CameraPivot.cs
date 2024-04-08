using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
	float pivotAngle = 90f;
	float angleVelocity;
	float theAngle;
	bool isMoving;
	StateManager stateManager;
	// Start is called before the first frame update
	void Start()
	{
		stateManager = FindFirstObjectByType<StateManager>();
		// isMoving = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (isMoving)
		{
			theAngle = Mathf.SmoothDampAngle(this.transform.rotation.eulerAngles.y, pivotAngle*((int)stateManager.currentPlayer), ref angleVelocity, 0.25f);
			this.transform.rotation = Quaternion.Euler(new Vector3(0, theAngle, 0));

			if (Math.Abs(this.transform.rotation.eulerAngles.y - pivotAngle*((int)stateManager.currentPlayer)) < 0.5f)
			{
				stateManager.isDoneChangingPlayer = true;
				isMoving = false;
			}
		}
	}

	public void MoveCamera()
	{
		isMoving = true;
		Debug.Log("Camera is moving");
	}
}
