using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    Camera Camera;
    Rigidbody Body;

    // Start is called before the first frame update
    void Start()
    {
        Body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera == null)
        {
            Camera = Camera.main;
            if (Camera == null)
            {
                return;
            }
        }

        Vector3 fromCam = transform.position - Camera.transform.position;
        fromCam.y = 0;
        fromCam = fromCam.normalized;
        Vector3 fromCamRight = new Vector3(fromCam.z, 0.0f, -fromCam.x);
        Debug.Log($"{fromCam} & {fromCamRight}");

        Vector3 dir = fromCam * Input.GetAxis("Vertical");
        dir += fromCamRight * Input.GetAxis("Horizontal");

        Body.velocity = dir * 10.0f;
    }
}
