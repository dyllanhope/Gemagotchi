using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
    public GemType gemType;

    public int xIndex;
    public int yIndex;

    public bool isMatched;
    private Vector3 currentPos;
    private Vector3 targetPos;

    public bool isMoving;

    public Gems(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void SetIndices(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }
}

public enum GemType
{
    Ruby,
    Emerald,
    Sapphire,
    Amethyst,
    Diamond
}
