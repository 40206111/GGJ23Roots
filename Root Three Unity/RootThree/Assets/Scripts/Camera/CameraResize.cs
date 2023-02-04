using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResize : MonoBehaviour
{
    [SerializeField]
    private float Speed = 1;

    // 1x1 (x,y,z) (-1, 2, -1)
    //10x10 (x,y,z) (-2, 7, -2)
    Vector3 Pos1x1 = new Vector3(-1, 2, -1);
    Vector3 TargetPos;


    private void Update()
    {
        var pos = transform.position;
        pos = Vector3.MoveTowards(pos, TargetPos, Speed);
        transform.position = pos;

    }

    public void ResizeCamera(int width, int height)
    {
        int size = Mathf.Max(width, height);
        float widthChange = size * 0.1f;
        float heightChange = size * 0.5f;

        TargetPos = new Vector3(Pos1x1.x - widthChange, Pos1x1.y + heightChange, Pos1x1.z - widthChange);
    }

    IEnumerator<YieldInstruction> MoveCamera()
    {
        var pos = transform.position;

        while (pos != TargetPos)
        {
            pos = Vector3.MoveTowards(pos, TargetPos, Speed);
            transform.position = pos;
            yield return null;
        }
    }

}
