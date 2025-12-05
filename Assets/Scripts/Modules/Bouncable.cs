using UnityEngine;

// Downward attacks from the player can bounce off of certain things like:
// spikes
// enemies
// other traps
// special bouncy areas
public class Bouncable : MonoBehaviour
{
    public readonly float bounceForce = 1f;

    public float GetBounceForce()
    {
        return bounceForce;
    }
}