using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int maxBuilding = 5; //It stores the maximum number of buildings allowed
    [SerializeField] private Text gameStatus, guides; //It references UI elements for displaying game status and guides
    [SerializeField] private Text playerScoreText, enemyScoreText; //player n enemy scores
    [SerializeField] private Button playerAttackButton; // the player attack button

    private int totalBuilding, playerScore, enemyScore;
    internal bool isPlayerTurn, isGameOver; // It tracks internal variables for player turn and isGameOver

    internal List<BattleBuilding> playerCells = new List<BattleBuilding>();
    internal List<BattleBuilding> enemyCells = new List<BattleBuilding>();
    readonly List<BattleBuilding> playerBuildings = new List<BattleBuilding>();
    readonly List<BattleBuilding> enemyBuildings = new List<BattleBuilding>();

    public static System.Action SelectBuilding; //Likely used to trigger initial building placement on the grid
    public static System.Action SwapTurn; // Handles logic when switching between player and enemy turns

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        SelectBuilding += RandomlySelectBuildings;
        SwapTurn += OnSwapTurn;
    }

    void Start()
    {
        OnSwapTurn();
    }

    void OnDisable()
    {
        SelectBuilding -= RandomlySelectBuildings;
        SwapTurn -= OnSwapTurn;
    }

    /*  It determines the total number of buildings (totalBuilding) randomly within a range.
        It calls GetRandomCells twice to randomly select cells for player and enemy buildings from their respective cell lists (playerCells, enemyCells)
        and stores them in building lists (playerBuildings, enemyBuildings).
        c. It activates the building visuals (ActivateBuildings) for player and enemy
        buildings with appropriate states (showing or hiding the building image).*/
    public void RandomlySelectBuildings()
    {
        totalBuilding = Random.Range(1, maxBuilding + 1);
        GetRandomCells(playerCells, playerBuildings, totalBuilding);
        GetRandomCells(enemyCells, enemyBuildings, totalBuilding);

        ActivateBuildings(playerBuildings, true);
        ActivateBuildings(enemyBuildings, false);
    }

    //It iterates through a building list and activates their building image based
    //on the provided flag and sets the HasBuilding property to true.
    void ActivateBuildings(List<BattleBuilding> buildings, bool isBuildingImage)
    {
        foreach (var building in buildings)
        {
            building.BuildingImage(isBuildingImage);
            building.HasBuilding = true;
        }
    }

    //It shuffles the original cell list using a generic Shuffle function.
    //It selects the first n elements(limited by the original list size) and adds them to the building list.
    void GetRandomCells<T>(List<T> originalList, List<T> buildingList, int n)
    {
        Shuffle(originalList);

        // Set bool to true for the first n buildings
        for (int i = 0; i < Mathf.Min(n, originalList.Count); i++)
        {
            buildingList.Add(originalList[i]);
        }
    }

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            int k = Random.Range(0, n);
            (list[n - 1], list[k]) = (list[k], list[n - 1]);
            n--;

            //T temp = list[k];
            //list[k] = list[n - 1];
            //list[n - 1] = temp;
        }
    }

    void UpdateUI()
    {
        gameStatus.text = isPlayerTurn ? "Player Turn" : "Enemy Turn";
        guides.text = isPlayerTurn ? "ClickOn Enmey Cell ===>" : "Click On Attack Player Button";
        playerScoreText.text = $"Player Score: {playerScore}/{totalBuilding}";
        enemyScoreText.text = $"Enemy Score: {enemyScore}/{totalBuilding}";
    }

    //It toggles the isPlayerTurn flag. It activates/deactivates the player attack button based on the current turn
    void OnSwapTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        playerAttackButton.gameObject.SetActive(!isPlayerTurn);
        UpdateUI();
    }

    //It increments the appropriate score (player or enemy) based on the current turn.
    // It checks the win condition(CheckWinCondition) if the score reaches the total building count.
    public void BuildingHit()
    {
        if (isPlayerTurn)
        {
            playerScore++;
            if (playerScore == totalBuilding)
                CheckWinCondition(true);
        }
        else
        {
            enemyScore++;
            if (enemyScore == totalBuilding)
                CheckWinCondition(false);
        }
    }

    //It sets the isGameOver flag to true. It starts a coroutine(GameStatus) to display the win/lose message after a delay
    void CheckWinCondition(bool status)
    {
        isGameOver = true;
        StartCoroutine(GameStatus(status));
    }

    IEnumerator GameStatus(bool win)
    {
        yield return new WaitForSeconds(1);
        playerAttackButton.gameObject.SetActive(false);
        guides.text = string.Empty;
        if (win)
        {
            SoundManager.Instance.GameWin();
            gameStatus.text = "You Won";
            print("You Won");

        }
        else
        {
            SoundManager.Instance.GameLose();
            gameStatus.text = "You Lose";
            print("You Lose");
        }
        guides.text = string.Empty;
    }
}
