using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuurentPlayerUI : MonoBehaviour
{
	public Material[] materials;
	StateManager stateManager;
	Renderer rend;

	// Start is called before the first frame update
	void Start()
	{
		stateManager = FindObjectOfType<StateManager>();
		rend = GetComponent<MeshRenderer>();
		if (stateManager != null)
		{
			rend.material = materials[(int)stateManager.winner];
		}
	}

	// Update is called once per frame
	void Update()
	{
	}
}
