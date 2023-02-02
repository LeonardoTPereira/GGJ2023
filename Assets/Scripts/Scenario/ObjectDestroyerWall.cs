using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyerWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(col.gameObject);
    }
}
