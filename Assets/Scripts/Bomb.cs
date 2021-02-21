using DungeonGeneration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1);
    }
}
