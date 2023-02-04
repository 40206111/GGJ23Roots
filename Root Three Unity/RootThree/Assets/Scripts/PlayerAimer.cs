using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    [SerializeField]
    float ReAimTime = 0.1f;
    float Timer;

    [SerializeField]
    Transform BeamRoot;
    [SerializeField]
    LineRenderer Beam;
    bool ShootingBeam = false;

    [SerializeField]
    float AimAssistDegrees = 10f;
    [SerializeField]
    Transform Reticule;
    Vector3 FloorPos;
    EnemyMover CurrentTarget;

    Vector3 aimDir;

    // Start is called before the first frame update
    void Start()
    {
        Timer = ReAimTime;
        FloorPos = Reticule.position;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if (CurrentTarget != null && Input.GetButtonDown("Jump"))
        {
            if (!ShootingBeam)
            {
                CurrentTarget.SetRooted(true);
                Vector3 beamTarget = CurrentTarget.GetComponentInChildren<MeshRenderer>().transform.position;
                StartCoroutine(BeamRoutine(beamTarget));
            }
        }

        aimDir.x = Input.GetAxisRaw("HorizontalAim");
        aimDir.z = Input.GetAxisRaw("VerticalAim");
        aimDir = aimDir.normalized;
        if (aimDir == Vector3.zero)
        {
            HideReticule();
        }
        else
        {
            if (Timer > ReAimTime)
            {
                while (Timer > ReAimTime)
                {
                    Timer -= ReAimTime;
                }

                EnemyMover[] enemies = FindObjectsByType<EnemyMover>(FindObjectsSortMode.None);
                float highest = Mathf.Cos(Mathf.Deg2Rad * AimAssistDegrees);
                EnemyMover chosen = null;
                foreach (EnemyMover e in enemies)
                {
                    Vector3 toE = (e.transform.position - transform.position).normalized;
                    float testDot = Vector3.Dot(toE, aimDir);
                    if (testDot > highest)
                    {
                        highest = testDot;
                        chosen = e;
                    }
                }
                if (chosen != null)
                {
                    Reticule.parent = chosen.transform;
                    Reticule.localPosition = FloorPos;
                    CurrentTarget = chosen;
                }
                else
                {
                    HideReticule();
                }
            }
        }
        Vector3 targetDir = Vector3.zero;
        if (CurrentTarget != null)
        {
            targetDir = CurrentTarget.transform.position - transform.position;
        }
        else if (aimDir != Vector3.zero)
        {
            targetDir = aimDir;
        }
        else
        {
            targetDir = GetComponent<Rigidbody>().velocity;
        }
        if (targetDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, targetDir, Vector3.up), 0);
        }
    }

    void HideReticule()
    {
        Reticule.parent = null;
        Reticule.position = FloorPos + Vector3.down * 5;
        CurrentTarget = null;
    }

    IEnumerator BeamRoutine(Vector3 targetPos)
    {
        ShootingBeam = true;
        Vector3 startPos = BeamRoot.position;
        Vector3 journey = targetPos - startPos;
        Vector3 journeyDir = journey.normalized;
        float distance = journey.magnitude;
        float beamLength = 5.0f;
        float beamSpeed = 40.0f;
        float journeyTime = distance / beamSpeed;

        float elapsedTime = 0.0f;

        while (true)
        {
            Beam.SetPosition(0, startPos + journeyDir * Mathf.Clamp(beamSpeed * elapsedTime, 0, distance));
            Beam.SetPosition(1, startPos + journeyDir * Mathf.Clamp(beamLength + beamSpeed * elapsedTime, 0, distance));

            if (elapsedTime >= journeyTime)
            {
                break;
            }

            yield return null;
            elapsedTime += Time.deltaTime;
        }
        ShootingBeam = false;
    }
}
