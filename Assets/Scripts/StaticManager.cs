/// <summary>
/// Holds all the important static variables and methods.
/// </summary>
public static class StaticManager
{
    #region Fields

    /// <summary>
    /// Returns if the players wants to restart the scene ("the game").
    /// </summary>
    public static bool isRestart = false;

    /// <summary>
    /// Holds the text for end game with the cause of death.
    /// </summary>
    public static string causeOfDeath = "";

    /// <summary>
    /// Returns if the game has ended.
    /// </summary>
    public static bool endGame = false;

    /// <summary>
    /// Returns if the playes is invulnerable.
    /// </summary>
    public static bool invulnerability;

    #endregion
}
