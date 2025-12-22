using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Settings")]
    public float smoothTime = 0.1f;

    float currentHorizontal;
    float currentVertical;
    float currentMagnitude;

    void Update()
    {
        // Lấy input từ bàn phím (WASD / Arrow)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Tính độ lớn vector di chuyển
        Vector2 input = new Vector2(h, v);
        float magnitude = Mathf.Clamp01(input.magnitude);

        // Làm mượt chuyển động animation
        currentHorizontal = Mathf.Lerp(currentHorizontal, h, smoothTime);
        currentVertical = Mathf.Lerp(currentVertical, v, smoothTime);
        currentMagnitude = Mathf.Lerp(currentMagnitude, magnitude, smoothTime);

        // Gửi giá trị sang Animator
        animator.SetFloat("InputHorizontal", currentHorizontal);
        animator.SetFloat("InputVertical", currentVertical);
        animator.SetFloat("InputMagnitude", currentMagnitude);
    }
}
