using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;

public class StartMenu : MonoBehaviour
{
	public Button button;
	int isPressed;
	readonly Color[] colors = {Color.white, Color.gray};

	// Start is called before the first frame update
	void Start()
	{
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
		button = GameObject.Find(buttonName).GetComponent<Button>();
		isPressed = (int) GameObject.Find(buttonName).GetComponent<Variables>().declarations["isPressed"];
		isPressed = (isPressed + 1) % 2;
		GameObject.Find(buttonName).GetComponent<Variables>().declarations["isPressed"] = isPressed;
		ApplyColor(colors[isPressed]);
		Debug.Log(isPressed);
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

}
