using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    [SerializeField]
    float ReAimTime = 0.1f;
    float Timer;

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

        if(CurrentTarget != null && Input.GetButtonDown("Jump"))
        {
            CurrentTarget.SetRooted(true);
        }

        aimDir.x = Input.GetAxisRaw("HorizontalAim");
        aimDir.z = Input.GetAxisRaw("VerticalAim");
        aimDir = aimDir.normalized;
        if (aimDir == Vector3.zero)
        {
            HideReticule();
            return;
        }

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

    void HideReticule()
    {
        Reticule.parent = null;
        Reticule.position = FloorPos + Vector3.down * 5;
        CurrentTarget = null;
    }
}
