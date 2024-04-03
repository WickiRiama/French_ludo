using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
	float pivotAngle = 90f;
	float angleVelocity;
	float theAngle;
	StateManager stateManager;
	// Start is called before the first frame update
	void Start()
	{
		stateManager = FindFirstObjectByType<StateManager>();
	}

	// Update is called once per frame
	void Update()
	{
		theAngle = Mathf.SmoothDampAngle(this.transform.rotation.eulerAngles.y, pivotAngle*((int)stateManager.currentPlayer), ref angleVelocity, 0.25f);
		this.transform.rotation = Quaternion.Euler(new Vector3(0, theAngle, 0));
	}
}
