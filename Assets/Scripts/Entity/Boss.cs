using System.Collections;
using UnityEngine;

/// <summary>
/// Boss Enemy type, uses coroutines to facilitate a slightly more complex behavior. And can display an HP bar.
/// </summary>
public class Boss : Enemy
{
    // fields
    private const float Bounds = 6.5f;
    private Vector2 moveDir;
    
    //methods
    protected override void Start()
    {
        base.Start();
        // show boss hp when boss first spawns in.
        HUDManager.Instance.DisplayBossHP();
        // start the AI
        StartCoroutine(Reposition());
    }

    protected override void Update()
    {
        // resolve any movement through the physics body.
        pb.MovePosition(moveDir * (speed * Time.deltaTime));
    }

    /// <summary>
    /// Represents a movement behavior from the boss for a short period of time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Reposition()
    {
        // choose a random spot to move to.
        Vector3 newPos = new Vector2(Random.Range(-Bounds, Bounds), transform.position.y);
        
        // move to the new location over 2.5 seconds.
        moveDir = (newPos - transform.position).normalized/2.5f;
        yield return new WaitForSeconds(2.5f);
        
        // when done, set movement to zero and change behavior.
        moveDir = Vector2.zero;
        StartCoroutine(ShootAndWait());
        StopCoroutine(Reposition());
    }

    /// <summary>
    /// Represents the boss shooting bullets at the enemy through their Shot and waiting.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootAndWait()
    {
        moveDir = Vector2.zero;
        
        // shoot at the player and wait.
        shot.Shooting(PlayerController.Player.transform.position - transform.position);
        yield return new WaitForSeconds(3f);
        
        // when done, change behavior.
        StartCoroutine(Reposition());
        StopCoroutine(ShootAndWait());
    }

    
    public override bool KillEnemy()
    {
        bool result = base.KillEnemy();
        HUDManager.Instance.UpdateBossHP(0);
        HUDManager.Instance.GameWin(); // when does dies invoke the game win event.
        return result;
    }

    public override void DealDamage(int damage)
    {
        base.DealDamage(damage);
        // update boss hp bar to show health total
        if(gameObject.activeInHierarchy) HUDManager.Instance.UpdateBossHP((float)(Health)/MaxHealth);
    }
}
