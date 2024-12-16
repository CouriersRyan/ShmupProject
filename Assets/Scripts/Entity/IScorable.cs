/// <summary>
/// Interface representing objects in the scene that have a score value to be gained, such as an enemy or a pickup.
/// </summary>
public interface IScorable
{
    public int Score { get; }

    /// <summary>
    /// Called to update the score of the player with the IScorable's Score property.
    /// </summary>
    public void AddScore()
    {
        HUDManager.Instance.UpdateScore(Score);
    }
}
