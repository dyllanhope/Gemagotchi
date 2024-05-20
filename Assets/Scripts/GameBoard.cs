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

    private List<GameObject> gemsToDestroy = new();

    [SerializeField] private Gems selectedGem;
    [SerializeField] private bool isProcessingMove;

    public static GameBoard instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitialliseBoard();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Gems>())
            {
                if (isProcessingMove)
                {
                    return;
                }
                Gems gem = hit.collider.gameObject.GetComponent<Gems>();
                Debug.Log("I have clicked a gem, it is: " + gem.gameObject);

                SelectGem(gem);
            }
        }
    }

    void InitialliseBoard()
    {
        DestroyGems();
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
                gemsToDestroy.Add(gem);
            }
        }

        if (CheckBoard())
        {
            Debug.Log("There are matches, re-initialising the board..");
            InitialliseBoard();
        }
        else
        {
            Debug.Log("There are no matches, starting game..");
        }
    }

    private void DestroyGems()
    {
        if (gemsToDestroy != null)
        {
            foreach (GameObject gem in gemsToDestroy)
            {
                Destroy(gem);
            }
            gemsToDestroy.Clear();
        }
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

    public void SelectGem(Gems gem)
    {
        if (selectedGem == null)
        {
            Debug.Log(gem);
            selectedGem = gem;
        }
        else if (selectedGem == gem)
        {
            selectedGem = null;
        }
        else if (selectedGem != gem)
        {
            SwapGem(selectedGem, gem);
            selectedGem = null;
        }
    }

    private void SwapGem(Gems currentGem, Gems targetGem)
    {
        if (!IsAdjacent(currentGem, targetGem))
        {
            return;
        }

        DoSwap(currentGem, targetGem);

        isProcessingMove = true;

        StartCoroutine(ProcessMatches(currentGem, targetGem));
    }

    private void DoSwap(Gems currentGem, Gems targetGem)
    {
        GameObject temp = gemBoard[currentGem.xIndex, currentGem.yIndex].gem;

        gemBoard[currentGem.xIndex, currentGem.yIndex].gem = gemBoard[targetGem.xIndex, targetGem.yIndex].gem;
        gemBoard[targetGem.xIndex, targetGem.yIndex].gem = temp;

        int tempXIndex = currentGem.xIndex;
        int tempYIndex = currentGem.yIndex;
        currentGem.xIndex = targetGem.xIndex;
        currentGem.yIndex = targetGem.yIndex;
        targetGem.xIndex = tempXIndex;
        targetGem.yIndex = tempYIndex;

        currentGem.MoveToTarget(gemBoard[targetGem.xIndex, targetGem.yIndex].gem.transform.position);
        targetGem.MoveToTarget(gemBoard[currentGem.xIndex, currentGem.yIndex].gem.transform.position);
    }
    private bool IsAdjacent(Gems currentGem, Gems targetGem)
    {
        return Mathf.Abs(currentGem.xIndex - targetGem.xIndex) + Mathf.Abs(currentGem.yIndex - targetGem.yIndex) == 1;
    }

    private IEnumerator ProcessMatches(Gems currentGem, Gems targetGem)
    {
        yield return new WaitForSeconds(0.2f);

        bool hasMatch = CheckBoard();

        if (!hasMatch)
        {
            DoSwap(currentGem, targetGem);
        }
        isProcessingMove = false;
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
