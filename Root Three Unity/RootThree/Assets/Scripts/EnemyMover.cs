using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    PlayerMover Player;
    Rigidbody Body;
    [SerializeField]
    float Speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player == null)
        {
            Player = FindObjectOfType<PlayerMover>();
            if(Player == null)
            {
                Body.velocity = Vector3.zero;
                return;
            }
        }

        Vector3 chase = (Player.transform.position - transform.position).normalized;

        Body.velocity = chase * Speed;
    }
}
