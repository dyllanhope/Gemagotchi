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
        CheckBoard();
    }

    public bool CheckBoard()
    {
        Debug.Log("Checking");
        bool hasMatched = false;

        List<Gems> gemsToRemove = new();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gemBoard[x, y].isUsable)
                {
                    Gems gem = gemBoard[x, y].gem.GetComponent<Gems>();

                    if (!gem.isMatched)
                    {
                        MatchResult matchedGems = IsConnected(gem);

                        if (matchedGems.connectedGems.Count >= 3)
                        {
                            gemsToRemove.AddRange(matchedGems.connectedGems);
                            foreach (Gems gemItem in matchedGems.connectedGems)
                            {
                                gem.isMatched = true;
                            }

                            hasMatched = true;
                        }
                    }

                }
            }
        }

        return hasMatched;
    }

    MatchResult IsConnected(Gems gem)
    {
        List<Gems> connectedGems = new();
        GemType gemType = gem.gemType;

        connectedGems.Add(gem);

        //right
        CheckDirection(gem, new Vector2Int(1, 0), connectedGems);
        //left
        CheckDirection(gem, new Vector2Int(-1, 0), connectedGems);
        if (connectedGems.Count == 3)
        {
            Debug.Log("Normal horizontal match found, the gem type is: " + connectedGems[0].gemType);

            return new MatchResult
            {
                connectedGems = connectedGems,
                matchDirection = MatchDirection.Horizontal
            };
        }
        else if (connectedGems.Count > 3)
        {
            Debug.Log("Long horizontal match found, the gem type is: " + connectedGems[0].gemType);

            return new MatchResult
            {
                connectedGems = connectedGems,
                matchDirection = MatchDirection.LongHorizontal
            };
        }

        connectedGems.Clear();
        connectedGems.Add(gem);

        //up
        CheckDirection(gem, new Vector2Int(0, 1), connectedGems);
        //down
        CheckDirection(gem, new Vector2Int(0, -1), connectedGems);
        if (connectedGems.Count == 3)
        {
            Debug.Log("Normal vertical match found, the gem type is: " + connectedGems[0].gemType);

            return new MatchResult
            {
                connectedGems = connectedGems,
                matchDirection = MatchDirection.Vertical
            };
        }
        else if (connectedGems.Count > 3)
        {
            Debug.Log("Long vertical match found, the gem type is: " + connectedGems[0].gemType);

            return new MatchResult
            {
                connectedGems = connectedGems,
                matchDirection = MatchDirection.LongVertical
            };
        }
        else
        {
            return new MatchResult
            {
                connectedGems = connectedGems,
                matchDirection = MatchDirection.None
            };
        }
    }

    void CheckDirection(Gems gem, Vector2Int direction, List<Gems> connectedGems)
    {
        GemType gemType = gem.gemType;
        int x = gem.xIndex + direction.x;
        int y = gem.yIndex + direction.y;

        while (x >= 0 && x < width && y >= 0 && y < height)
        {
            if (gemBoard[x, y].isUsable)
            {
                Gems neighbourGem = gemBoard[x, y].gem.GetComponent<Gems>();

                if (!neighbourGem.isMatched && neighbourGem.gemType == gemType)
                {
                    connectedGems.Add(neighbourGem);

                    x += direction.x;
                    y += direction.y;
                }
                else
                {
                    break;
                }

            }
            else
            {
                break;
            }
        }
    }
}

public class MatchResult
{
    public List<Gems> connectedGems;
    public MatchDirection matchDirection;
}

public enum MatchDirection
{
    Vertical,
    Horizontal,
    LongVertical,
    LongHorizontal,
    Combined,
    None

}
