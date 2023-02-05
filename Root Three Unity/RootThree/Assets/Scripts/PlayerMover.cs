using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    Camera Camera;
    Rigidbody Body;

    [SerializeField]
    float MoveSpeed = 7f;
    [SerializeField]
    float WalkSpeedPercentage = 0.3f;

    bool Grounded = false;

    Animator Anim;

    // Start is called before the first frame update
    void Awake()
    {
        Body = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
    }

    public void SetUp()
    {
        Anim.SetTrigger("Normal");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State != GameManager.eGameState.Running)
        {
            Body.velocity = Vector3.zero;
            return;
        }

        if (Camera == null)
        {
            Camera = Camera.main;
            if (Camera == null)
            {
                return;
            }
        }

        Vector3 fromCam = Camera.transform.forward;
        //Vector3 fromCam = transform.position - Camera.transform.position;
        fromCam.y = 0;
        fromCam = fromCam.normalized;
        Vector3 fromCamRight = new Vector3(fromCam.z, 0.0f, -fromCam.x);

        Vector3 dir = fromCam * Input.GetAxis("Vertical");
        dir += fromCamRight * Input.GetAxis("Horizontal");

        Body.velocity = dir * MoveSpeed * (Input.GetButton("Walk") ? WalkSpeedPercentage : 1f);
    }

    public void Stop()
    {
        Body.velocity = Vector3.zero;
    }

    protected void OnCollisionEnter(Collision collision)
    {
    	if (!Grounded && collision.collider.CompareTag("Floor"))
        {
            Grounded = true;
            Body.constraints = RigidbodyConstraints.FreezePositionY | Body.constraints;
        }
        if (GameManager.Instance.State != GameManager.eGameState.Running) return;

        if (collision.collider.CompareTag("Enemy"))
        {
            GameManager.Instance.SetState(GameManager.eGameState.Ended);
            Anim.SetTrigger("Defeat");
        }
    }
}
