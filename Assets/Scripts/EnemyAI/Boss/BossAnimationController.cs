using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BossController bossController;
    
    
    private int isWalkingHash;


    private void Awake()
    {
        isWalkingHash = Animator.StringToHash("IsWalking");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationStates();
    }

    private void UpdateAnimationStates()
    {
        animator.SetBool(isWalkingHash, bossController.isWalking);
    }
}
