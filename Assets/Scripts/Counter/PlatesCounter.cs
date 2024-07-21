using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;


    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawPlateTimer;
    private float spawPlateTimerMax = 4f;
    private int platesSpawnAmout;
    private int platesSpawnAmoutMax = 4;

    private void Update() 
    {
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
    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            //플레이어가 아무것도 안들고 있을때

            if(platesSpawnAmout > 0)
            {
                platesSpawnAmout--;

                KitchenObject.SpawKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
