using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isUsable;

    public GameObject gem;

    public Node(bool usable, GameObject _gem)
    {
        isUsable = usable;
        gem = _gem;
    }
}
