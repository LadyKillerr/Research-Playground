using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private const string _ATTACK = "attack";

    PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void Attack()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            playerController.SetAnimState(_ATTACK);
            //playerController.SetAnimation();
        
        }

    }
}
