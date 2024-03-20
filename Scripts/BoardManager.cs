using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		DrawChessBoard();
	}
	void DrawChessBoard()
	{
		Vector3 widthLine = Vector3.right * 8;
		Vector3 heightLine = Vector3.forward * 8;

		for (int i = 0; i <= 8; i++)
		{
			Vector3 start = Vector3.forward * i;
			Debug.DrawLine(start, start + widthLine);
			for (int j = 0; j <= 8; j++)
			{
				start = Vector3.right * j;
				Debug.DrawLine(start, start + heightLine);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		DrawChessBoard();
	}
}
