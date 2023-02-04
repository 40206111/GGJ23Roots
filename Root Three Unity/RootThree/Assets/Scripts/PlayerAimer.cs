using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    [SerializeField]
    float ReAimTime = 0.1f;
    float Timer;

    [SerializeField]
    Transform Reticule;
    Vector3 FloorPos;

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
            float highest = 0.5f;
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
    }
}
