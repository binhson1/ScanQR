using UnityEngine;
using UnityEngine.UI; // Sử dụng Text
using TMPro;          // Sử dụng TextMeshPro nếu cần

public class ScrollingText : MonoBehaviour
{
    public float speed = 50f; // Tốc độ di chuyển
    public RectTransform textTransform; // RectTransform của Text
    public float startPositionX = -500f; // Vị trí bắt đầu (bên trái)
    public float endPositionX = 500f;   // Vị trí kết thúc (bên phải)

    private void Start()
    {
        if (textTransform == null)
        {
            textTransform = GetComponent<RectTransform>();
        }

        // Đặt Text ở vị trí bắt đầu
        textTransform.anchoredPosition = new Vector2(startPositionX, textTransform.anchoredPosition.y);
    }

    private void Update()
    {
        // Di chuyển Text từ trái sang phải
        textTransform.anchoredPosition += new Vector2(speed * Time.deltaTime, 0);

        // Khi vượt qua vị trí kết thúc, quay lại vị trí bắt đầu
        if (textTransform.anchoredPosition.x > endPositionX)
        {
            textTransform.anchoredPosition = new Vector2(startPositionX, textTransform.anchoredPosition.y);
        }
    }
}
