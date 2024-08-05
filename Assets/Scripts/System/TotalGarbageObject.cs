using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TotalGarbageObject : MonoBehaviour
{
    public int GetGarbageCount()
    {
        return transform.GetComponentsInChildren<GarbageObject>().Length;
    }
}
