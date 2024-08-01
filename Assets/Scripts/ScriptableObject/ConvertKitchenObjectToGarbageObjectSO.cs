using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ConvertKitchenObjectToGarbageObjectSO : ScriptableObject
{
    public KitchenObjectSO input;
    public GarbageObjectSO[] output;
}
