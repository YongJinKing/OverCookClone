using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomAnimator : MonoBehaviour
{
    private const string IS_CLEANING = "isCleaning";
    private Animator animator;
    private void Start() 
    {
        animator = GetComponent<Animator>();
    }
    private void Update() 
    {

    }
}
