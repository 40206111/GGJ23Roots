using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResize : MonoBehaviour
{
    [SerializeField]
    private float MoveTime = 1f;

    [SerializeField]
    AnimationCurve Curve;

    // 1x1 (x,y,z) (-0.5, 2, -0.5)
    //10x10 (x,y,z) (-0.5, 13, -0,5)
    Vector3 Pos1x1 = new Vector3(-0.5f, 2, -0.5f);
    Vector3 TargetPos;

    public void Awake()
    {
        transform.position = Pos1x1;
        Vector3 rotation = new Vector3(60, 45, 0);
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void ResizeCamera(int width, int height)
    {
        int size = Mathf.Max(width, height);
        float heightChange = size;

        TargetPos = new Vector3(Pos1x1.x, Pos1x1.y + heightChange, Pos1x1.z);
        StartCoroutine(MoveCamera());
    }

    IEnumerator<YieldInstruction> MoveCamera()
    {
        var startPos = transform.position;
        float timePassed = 0;
        yield return null;
        while (true)
        {
            float t = Mathf.Clamp01(timePassed / MoveTime);
            transform.position = Vector3.Lerp(startPos, TargetPos, Curve.Evaluate(t));
            if(t >= 1f)
            {
                break;
            }

            yield return null;
            timePassed += Time.deltaTime;
        }

    }

}
