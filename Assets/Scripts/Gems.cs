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

    public void MoveToTarget(Vector2 targetPos)
    {
        StartCoroutine(MoveCoroutine(targetPos));
    }
    private IEnumerator MoveCoroutine(Vector2 targetPos)
    {
        isMoving = true;
        float duration = 0.2f;

        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector2.Lerp(startPosition,targetPos,t);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
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
