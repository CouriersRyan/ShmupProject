public interface IHealth
{
    public int Health { get; }
    public void DealDamage(int damage);

    public void DealDamage(DamageInfo damageInfo);
}

public struct DamageInfo
{
    public int Damage;
}
