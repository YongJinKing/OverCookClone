using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;


    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawPlateTimer;
    private float spawPlateTimerMax = 4f;
    private int platesSpawnAmout;
    private int platesSpawnAmoutMax = 4;

    private void Update() {
        spawPlateTimer += Time.deltaTime;
        if(spawPlateTimer > spawPlateTimerMax)
        {
            spawPlateTimer = 0f;

            if(platesSpawnAmout < platesSpawnAmoutMax)
            {
                platesSpawnAmout++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
