using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public int width = 6;
    public int height = 8;

    public float spacingX;
    public float spacingY;

    public GameObject[] gemPrefabs;

    private Node[,] gemBoard;
    public GameObject gemBoardGO;

    //public ArrayLayout arrayLayout;
    public static GameBoard instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitialliseBoard();
    }

    void InitialliseBoard()
    {
        gemBoard = new Node[width, height];

        spacingX = (float)((width - 1) / 2) + 3;
        spacingY = (float)(height - 1) / 2;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 position = new Vector2(x - spacingX, y - spacingY);
                int randomIndex = Random.Range(0, gemPrefabs.Length);

                GameObject gem = Instantiate(gemPrefabs[randomIndex], position, Quaternion.identity);
                gem.GetComponent<Gems>().SetIndices(x, y);
                gemBoard[x, y] = new Node(true, gem);
            }
        }
    }
}
