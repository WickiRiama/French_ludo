using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
	public Sprite[] faces;

	StateManager stateManager;

	// Start is called before the first frame update
	void Start()
	{
		stateManager = GameObject.FindObjectOfType<StateManager>();
		ResetDice();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void RollTheDice()
	{
		if (stateManager.isDoneChangingPlayer && !stateManager.isDoneRolling)
		{
			int value = Random.Range(1, 7);
			// int value = 6;
			this.GetComponent<Image>().sprite = faces[value];
			stateManager.diceValue = value;
			stateManager.isDoneRolling = true;
		}
	}

	public void ResetDice()
	{
		this.GetComponent<Image>().sprite = faces[0];
		stateManager.diceValue = 0;
	}
}
