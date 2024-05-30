using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaGotchiManager : MonoBehaviour
{
    [SerializeField] List<Sprite> spriteList = new();
   
    public void ChangeMamaGotchiGem(int index)
    {
        //GetComponent<SpriteRenderer>.sprite = newSprite;
        GetComponent<SpriteRenderer>().sprite = spriteList[index];
    }
}
