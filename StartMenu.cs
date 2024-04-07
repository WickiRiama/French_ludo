using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;

public class StartMenu : MonoBehaviour
{
	Button button;
	int isPressed;
	readonly Color[] colors = { Color.white, Color.gray };
	public bool[] isAIPlayer;
	public static StartMenu menu;


	private void Awake()
	{
		if (menu != null)
		{
			Destroy(gameObject);
			return;
		}

		menu = this;
		DontDestroyOnLoad(gameObject);
	}

	// Start is called before the first frame update
	void Start()
	{
		isAIPlayer = new bool[4];
		for (int i = 0; i < 4; i++)
		{
			isAIPlayer[i] = true;
		}
		isPressed = 0;
	}


	public void PlayGame()
	{
		SceneManager.LoadScene(1);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void PlayerClicked()
	{
		string buttonName = EventSystem.current.currentSelectedGameObject.name;
		GameObject buttonObject = GameObject.Find(buttonName);
		button = buttonObject.GetComponent<Button>();
		isPressed = (int)buttonObject.GetComponent<Variables>().declarations["isPressed"];

		isPressed = (isPressed + 1) % 2;
		buttonObject.GetComponent<Variables>().declarations["isPressed"] = isPressed;
		ApplyColor(colors[isPressed]);
		Debug.Log(isPressed);
		SetPlayer((PlayerId)buttonObject.GetComponent<Variables>().declarations["owner"], isPressed == 1);
		Debug.Log(isAIPlayer[0]);
		Debug.Log(isAIPlayer[1]);
		Debug.Log(isAIPlayer[2]);
		Debug.Log(isAIPlayer[3]);
	}

	void ApplyColor(Color color)
	{
		ColorBlock buttonColors = button.colors;

		buttonColors.normalColor = color;
		buttonColors.selectedColor = color;
		buttonColors.pressedColor = color;
		buttonColors.highlightedColor = color;

		button.colors = buttonColors;
	}

	void SetPlayer(PlayerId player, bool set)
	{
		if (set)
		{
			isAIPlayer[(int)player] = false;
		}
		else
		{
			isAIPlayer[(int)player] = true;
		}
	}

}
