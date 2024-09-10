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
        if (Input.GetKey(KeyCode.J))
        {
            Attack();
        }
    }

    public void Attack()
    {
            playerController.SetAnimState(_ATTACK);
            //playerController.SetAnimation();
    }
}
