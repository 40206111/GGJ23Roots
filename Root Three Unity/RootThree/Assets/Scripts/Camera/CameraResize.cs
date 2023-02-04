using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResize : MonoBehaviour
{
    [SerializeField]
    private float MoveTime = 1f;

    [SerializeField]
    AnimationCurve Curve;

    // 1x1 (x,y,z) (-1, 2, -1)
    //10x10 (x,y,z) (-2, 7, -2)
    Vector3 Pos1x1 = new Vector3(-1, 2, -1);
    Vector3 TargetPos;

    public void ResizeCamera(int width, int height)
    {
        int size = Mathf.Max(width, height);
        float widthChange = size * 0.1f;
        float heightChange = size * 0.5f;

        TargetPos = new Vector3(Pos1x1.x - widthChange, Pos1x1.y + heightChange, Pos1x1.z - widthChange);
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
