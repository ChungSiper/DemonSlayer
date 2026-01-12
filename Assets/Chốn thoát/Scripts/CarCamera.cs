using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public Camera cam;
    public float normalFOV = 60f;
    public float nitroFOV = 75f;
    public float smooth = 5f;

    float targetFOV;

    void Start()
    {
        targetFOV = normalFOV;
    }

    void Update()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * smooth);
    }

    public void NitroZoom(bool enable)
    {
        targetFOV = enable ? nitroFOV : normalFOV;
    }
}

