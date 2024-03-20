using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
	public int value = 1;
	public Sprite[] faces;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void RollTheDice()
	{
		value = Random.Range(0, 6);
		this.GetComponent<Image>().sprite = faces[value];
		Debug.Log("Roll value: " + value.ToString());
	}
}
