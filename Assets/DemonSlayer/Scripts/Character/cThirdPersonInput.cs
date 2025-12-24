using UnityEngine;

public class cThirdPersonInput : MonoBehaviour
{
    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;
    [HideInInspector] public float magnitude;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(horizontal, vertical);
        magnitude = Mathf.Clamp01(input.magnitude);
    }
}