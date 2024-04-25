using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour
{
	int isPressed;
	readonly Color[] colors = { Color.white, Color.gray };
	public GameObject firstHorsenumberButton;
	Button lastHorsesNumberButton;

	// Start is called before the first frame update
	void Start()
	{
		if (GameManager.menu.winner != PlayerId.NONE)
		{
			this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
			this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
		}
		else
		{
			this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
			this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
		}
		isPressed = 0;
		lastHorsesNumberButton = firstHorsenumberButton.GetComponent<Button>();
	}

	// Update is called once per frame
	void Update()
	{
		
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
		Button button = buttonObject.GetComponent<Button>();
		isPressed = (int)buttonObject.GetComponent<Variables>().declarations["isPressed"];

		isPressed = (isPressed + 1) % 2;
		buttonObject.GetComponent<Variables>().declarations["isPressed"] = isPressed;
		ApplyColor(colors[isPressed], button);
		GameManager.menu.SetPlayer((PlayerId)buttonObject.GetComponent<Variables>().declarations["owner"], isPressed == 1);
	}

	public void HorsesNumberClicked()
	{
		string buttonName = EventSystem.current.currentSelectedGameObject.name;
		Button button = GameObject.Find(buttonName).GetComponent<Button>();

		if (buttonName != lastHorsesNumberButton.name)
		{
			ApplyColor(colors[1], button);
			ApplyColor(colors[0], lastHorsesNumberButton);
			lastHorsesNumberButton = button;
			GameManager.menu.SetHorseNumber(int.Parse(buttonName.Split()[0]));
		}
	}


	void ApplyColor(Color color, Button button)
	{
		ColorBlock buttonColors = button.colors;

		buttonColors.normalColor = color;
		buttonColors.selectedColor = color;
		buttonColors.pressedColor = color;
		buttonColors.highlightedColor = color;

		button.colors = buttonColors;

		if (color == Color.white)
		{
			button.GameObject().GetComponent<UnityEngine.UI.Outline>().enabled = false;
		}
		else
		{
			button.GameObject().GetComponent<UnityEngine.UI.Outline>().enabled = true;
		}

	}

}
