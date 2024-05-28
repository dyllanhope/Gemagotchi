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
    public int points;

    public bool isGameEnded;

    public TextMeshProUGUI pointsTxt;
    public TextMeshProUGUI movesTxt;
    public TextMeshProUGUI goalTxt;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(int _moves, int _goal)
    {
        moves = _moves;
        goal = _goal;
    }

    void Update()
    {
        pointsTxt.text = "Points: " + points.ToString();
        movesTxt.text = "Moves: " + moves.ToString();
        goalTxt.text = "Goal: " + goal.ToString();
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves)
    {
        points += _pointsToGain;
        if (_subtractMoves)
        {
            moves--;
        }
        if (points >= goal)
        {
            isGameEnded = true;

            backgroundPanel.SetActive(true);
            TextMeshProUGUI messageText = FetchDisplayMessageObject(victoryPanel, "CongratsText");
            messageText.text = "Congratulations, you got " + points + " points in x moves!";
            victoryPanel.SetActive(true);
            GameBoard.instance.gemParent.SetActive(false);
            return;
        }
        if (moves == 0)
        {
            isGameEnded = true;

            backgroundPanel.SetActive(true);
            TextMeshProUGUI messageText = FetchDisplayMessageObject(losePanel, "MessageText");
            messageText.text = "Unfortunately you only got " + points + " points in x moves!";
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
