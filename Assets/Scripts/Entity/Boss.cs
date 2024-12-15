public class Boss : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        // shoot at the player and move as the basic behaviors
        shot.Shooting(PlayerController.Player.transform.position - transform.position);
    }

    public override bool KillEnemy()
    {
        bool result = base.KillEnemy();
        HUDManager.Instance.GameWin();
        return result;
    }
}
