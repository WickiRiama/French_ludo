using UnityEngine;

public class GameManager : MonoBehaviour
{
	public bool[] isAIPlayer;
	public int nbHorses;
	public static GameManager menu;
	public PlayerId winner;

	private void Awake()
	{
		if (menu != null)
		{
			Destroy(gameObject);
			return;
		}

		menu = this;
		DontDestroyOnLoad(gameObject);
		winner = PlayerId.NONE;
	}

	// Start is called before the first frame update
	void Start()
	{
		isAIPlayer = new bool[4];
		for (int i = 0; i < 4; i++)
		{
			isAIPlayer[i] = true;
		}
		nbHorses = 4;
	}

	public void SetPlayer(PlayerId player, bool set)
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

	public void SetHorseNumber(int number)
	{
		nbHorses = number;
	}


}
