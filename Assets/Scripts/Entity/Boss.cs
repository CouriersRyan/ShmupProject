public class Boss : Enemy
{
    private const float Bounds = 8.5f;
    
    protected override void Start()
    {
        base.Start();
        HUDManager.Instance.DisplayBossHP();
    }

    protected override void Update()
    {
        // shoot at the player and move as the basic behaviors
        shot.Shooting(PlayerController.Player.transform.position - transform.position);
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
