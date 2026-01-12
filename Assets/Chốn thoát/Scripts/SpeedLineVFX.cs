using UnityEngine;

public class SpeedLineVFX : MonoBehaviour
{
    public ParticleSystem speedLines;

    public void SetActive(bool active)
    {
        if (active && !speedLines.isPlaying)
            speedLines.Play();
        else if (!active && speedLines.isPlaying)
            speedLines.Stop();
    }
}
