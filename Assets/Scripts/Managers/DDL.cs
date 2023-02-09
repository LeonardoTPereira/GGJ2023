using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDL : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}