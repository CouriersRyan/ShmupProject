/// <summary>
/// Interface representing an object with Health that can be dealt damage to.
/// </summary>
public interface IHealth
{
    public int Health { get; }
    public void DealDamage(int damage);

    public void DealDamage(DamageInfo damageInfo);
}

/// <summary>
/// Structure representing damage data from an attack.
/// </summary>
public struct DamageInfo
{
    public int Damage;
}
