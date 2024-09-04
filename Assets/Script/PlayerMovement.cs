using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    SkeletonAnimation skeletalAnimation;
    Rigidbody2D playerRigidbody;


    void Awake()
    {
        skeletalAnimation = GetComponentInChildren<SkeletonAnimation>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
