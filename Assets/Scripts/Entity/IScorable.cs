public interface IScorable
{
    public int Score { get; }

    public void AddScore()
    {
        HUDManager.Instance.UpdateScore(Score);
    }
}
