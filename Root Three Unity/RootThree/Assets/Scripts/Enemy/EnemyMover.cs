using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    protected PlayerMover Player;
    protected Rigidbody Body;
    [SerializeField]
    protected float Speed = 5f;

    protected bool IsSpawnFalling = true;
    protected bool IsRooted = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Player == null)
        {
            Player = FindObjectOfType<PlayerMover>();
            if (Player == null)
            {
                Body.velocity = Vector3.zero;
                return;
            }
        }

        if (!IsRooted && !IsSpawnFalling)
        {
            Vector3 chase = TargettingDirection();

            Body.velocity = chase * Speed;
        }
    }

    protected virtual Vector3 TargettingDirection()
    {
        return (Player.transform.position - transform.position).normalized;
    }

    public void SetRooted(bool isRooted)
    {
        IsRooted = isRooted;
        Body.velocity = Vector3.zero;
        var pos = transform.position;
        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);
        transform.position = new Vector3(x, pos.y, z);
        GameGrid.Instance.TheGrid[z * GameGrid.Instance.Width + x].Root(this);

    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            IsSpawnFalling = false;
            SetRooted(true);
        }
    }

}

