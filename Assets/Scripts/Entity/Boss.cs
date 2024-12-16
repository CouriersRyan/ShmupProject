using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    private const float Bounds = 6.5f;
    private Vector2 moveDir;
    
    protected override void Start()
    {
        base.Start();
        HUDManager.Instance.DisplayBossHP();
        StartCoroutine(Reposition());
    }

    protected override void Update()
    {
        // shoot at the player and move as the basic behaviors
        pb.MovePosition(moveDir * (speed * Time.deltaTime));
    }

    private IEnumerator Reposition()
    {
        Vector3 newPos = new Vector2(Random.Range(-Bounds, Bounds), transform.position.y);
        moveDir = (newPos - transform.position).normalized/2.5f;
        yield return new WaitForSeconds(2.5f);
        moveDir = Vector2.zero;
        StartCoroutine(ShootAndWait());
        StopCoroutine(Reposition());
    }

    private IEnumerator ShootAndWait()
    {
        moveDir = Vector2.zero;
        shot.Shooting(PlayerController.Player.transform.position - transform.position);
        yield return new WaitForSeconds(3f);
        StartCoroutine(Reposition());
        StopCoroutine(ShootAndWait());
    }

    public override bool KillEnemy()
    {
        bool result = base.KillEnemy();
        HUDManager.Instance.UpdateBossHP(0);
        HUDManager.Instance.GameWin();
        return result;
    }

    public override void DealDamage(int damage)
    {
        base.DealDamage(damage);
        if(gameObject.activeInHierarchy) HUDManager.Instance.UpdateBossHP((float)(Health)/MaxHealth);
    }
}
