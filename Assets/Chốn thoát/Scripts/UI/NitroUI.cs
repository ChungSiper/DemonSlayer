using UnityEngine;
using UnityEngine.UI;

public class NitroUI : MonoBehaviour
{
    public Slider nitroSlider;
    NitroController nitro;

    void Start()
    {
        nitro = FindObjectOfType<NitroController>();
        nitroSlider.maxValue = nitro.maxNitro;
    }

    void Update()
    {
        nitroSlider.value = nitro.currentNitro;
    }
}
