using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake() 
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start() 
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
    }
    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform platevisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
    }

}
