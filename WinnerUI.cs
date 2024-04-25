using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerUI : MonoBehaviour
{
	public Material[] materials;
	Renderer rend;

	// Start is called before the first frame update
	void Start()
	{
		rend = GetComponent<MeshRenderer>();
		rend.material = materials[(int)GameManager.menu.winner];
	}

	// Update is called once per frame
	void Update()
	{
	}
}
