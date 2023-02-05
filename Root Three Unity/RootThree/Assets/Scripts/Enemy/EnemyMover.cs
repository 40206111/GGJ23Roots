using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eColours
{
    None    =   0,
    Red     =   1 << 1,
    Orange  =   1 << 2,
    Yellow  =   1 << 3,
    Green   =   1 << 4,
    Blue    =   1 << 5,
    Purple  =   1 << 6,
}
public class EnemyMover : MonoBehaviour
{
    protected PlayerMover Player;
    protected Rigidbody Body;
    [SerializeField]
    protected EnemyMovementData MoveData;

    protected bool IsSpawnFalling = true;
    public bool GetIsSpawnFalling { get { return IsSpawnFalling; } }
    protected bool IsRooted = false;
    public bool GetIsRooted { get { return IsRooted; } }

    public eColours Colour;

    Animator Anim;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        Body = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        StartCoroutine(SetNormalTrigger());
    }

    IEnumerator<YieldInstruction> SetNormalTrigger()
    {
        yield return null;
        if (Anim.gameObject.activeSelf)
        {
            Anim.SetTrigger("Normal");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (GameManager.Instance.State != GameManager.eGameState.Running) return;

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

            Body.velocity = chase * MoveData.Speed;
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

        GameManager.Instance.EnemyRooted(this);

        Body.constraints |= RigidbodyConstraints.FreezeAll;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (IsSpawnFalling && collision.collider.CompareTag("Floor"))
        {
            IsSpawnFalling = false;
            Body.constraints = RigidbodyConstraints.FreezePositionY | Body.constraints;
        }
    }

    public void DefeatEnemy(bool doEffects = true)
    {
        GameManager.Instance.EnemyDestroyed(this);
        StartCoroutine(WaitToDelete(doEffects));
    }

    IEnumerator<YieldInstruction> WaitToDelete(bool doEffects)
    {

        if (Anim.gameObject.activeSelf)
        {
            Anim.SetTrigger("Defeat");
        }
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }

}

