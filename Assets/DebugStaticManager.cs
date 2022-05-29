using UnityEngine;

/// <summary>
/// Debugs StaticManager script.
/// </summary>
public class DebugStaticManager : MonoBehaviour
{
    /// <summary>
    /// If on, debug static values from static manager
    /// </summary>
    public bool debug;

    /// <summary>
    /// Display values for static manager when the scene is loaded.
    /// </summary>
    void Start()
    {
        if (debug)
        {
            Debug.Log("isRestart: " + StaticManager.isRestart);
            Debug.Log("causeOfDeath: " + StaticManager.causeOfDeath);
            Debug.Log("endGame: " + StaticManager.endGame);
        }
    }
}
