using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject backgroundPanel;
    public GameObject victoryPanel;
    public GameObject losePanel;

    public int goal;
    public int moves;
    public int totalMoves;
    public int points;

    public bool isGameEnded;

    public TextMeshProUGUI pointsTxt;
    public TextMeshProUGUI movesTxt;
    public TextMeshProUGUI goalTxt;

    MamaGotchiManager mamaGotchiManager;

    private void Awake()
    {
        Instance = this;
        mamaGotchiManager = FindObjectOfType<MamaGotchiManager>();
    }

    private void Start()
    {
        totalMoves = moves;
    }

    public void Initialize(int _moves, int _goal)
    {
        moves = _moves;
        goal = _goal;
    }

    void Update()
    {
        pointsTxt.text = "Points: " + points.ToString("00");
        movesTxt.text = "Moves: " + moves.ToString("00");
        goalTxt.text = "Goal: " + goal.ToString("00");
    }
    public void CheckMamaGatchiUpgrade()
    {
        bool winState = false;
        int tempIndex = -1;
        int currentIndex = mamaGotchiManager.GetCurrentIndex();

        if (points >= goal)
        {
            winState = true;
            tempIndex = 5;
        }
        else if(points >= goal * 0.80)
        {
            tempIndex = 4;
        }
        else if (points >= goal * 0.64)
        {
            tempIndex = 3;
        }
        else if (points >= goal * 0.48)
        {
            tempIndex = 2;
        }
        else if (points >= goal * 0.32)
        {
            tempIndex = 1;
        }
        else if (points >= goal * 0.16)
        {
            tempIndex = 0;
        }
        if (tempIndex > currentIndex)
        {
            mamaGotchiManager.UpgradeMamaGatchi(winState);
        }
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves)
    {
        points += _pointsToGain;
        CheckMamaGatchiUpgrade();
        if (_subtractMoves)
        {
            moves--;
        }
        if (points >= goal)
        {
            isGameEnded = true;

            backgroundPanel.SetActive(true);
            TextMeshProUGUI messageText = FetchDisplayMessageObject(victoryPanel, "CongratsText");
            messageText.text = "Congratulations, you got " + points + " points in under " + totalMoves + " moves!";
            victoryPanel.SetActive(true);
            GameBoard.instance.gemParent.SetActive(false);
            return;
        }
        if (moves == 0)
        {
            isGameEnded = true;

            backgroundPanel.SetActive(true);
            TextMeshProUGUI messageText = FetchDisplayMessageObject(losePanel, "MessageText");
            messageText.text = "Unfortunately you only got " + points + " points in under " + totalMoves + " moves!";
            losePanel.SetActive(true);
            GameBoard.instance.gemParent.SetActive(false);
            return;
        }
    }

    public TextMeshProUGUI FetchDisplayMessageObject(GameObject panel, string childName)
    {
        TextMeshProUGUI[] children = panel.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI child in children)
        {
            if (child.name == childName)
            {
                return child;
            }
        }

        return null;
    }

    public void WinGame()
    {
        SceneManager.LoadScene(0);
    }
    public void LoseGame()
    {
        SceneManager.LoadScene(0);
    }
}
