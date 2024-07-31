using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    public override void Interact(Player player)
    {
        if(player.HasKitchenObjectOnTheTop())
        {
            player.GetKitchenObjectOnTheTop().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
