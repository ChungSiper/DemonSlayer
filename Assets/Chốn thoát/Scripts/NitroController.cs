using UnityEngine;

public class NitroController : MonoBehaviour
{
    public float maxNitro = 100f;
    public float currentNitro = 0f;

    public float nitroForce = 3000f;
    public float consumeRate = 25f;

    Rigidbody rb;
    CarCamera cam;
    SpeedLineVFX speedVFX;

    public bool IsUsingNitro { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<CarCamera>();
        speedVFX = GetComponent<SpeedLineVFX>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftControl) && currentNitro > 0f)
        {
            UseNitro();
        }
        else
        {
            StopNitro();
        }
    }

    void UseNitro()
    {
        IsUsingNitro = true;
        rb.AddForce(transform.forward * nitroForce, ForceMode.Acceleration);
        currentNitro -= consumeRate * Time.fixedDeltaTime;

        cam.NitroZoom(true);
        speedVFX.SetActive(true);
    }

    void StopNitro()
    {
        IsUsingNitro = false;
        cam.NitroZoom(false);
        speedVFX.SetActive(false);
    }

    public void AddNitro(float amount)
    {
        currentNitro = Mathf.Clamp(currentNitro + amount, 0, maxNitro);
    }
}
