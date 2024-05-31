using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaGotchiManager : MonoBehaviour
{
    [SerializeField] List<Sprite> spriteList = new();

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        CheckMamaGatchiType();
    }

    public void CheckMamaGatchiType()
    {
        var points = gameManager.points;
        var goal = gameManager.goal;

        if (points >= goal * 0.2)
        {
            GetComponent<SpriteRenderer>().sprite = spriteList[0];
        }
        if (points >= goal * 0.4)
        {
            GetComponent<SpriteRenderer>().sprite = spriteList[1];
        }
        if (points >= goal * 0.6)
        {
            GetComponent<SpriteRenderer>().sprite = spriteList[2];
        }
        if (points >= goal * 0.8)
        {
            GetComponent<SpriteRenderer>().sprite = spriteList[3];
        }
        if (points >= goal * 0.95)
        {
            GetComponent<SpriteRenderer>().sprite = spriteList[4];
        }
    }
}
