using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSizeController : MonoBehaviour
{
    public TMP_Text targetText;       // Văn bản cần thay đổi kích cỡ
    public TMP_InputField sizeInput; // Ô nhập số
    public Button increaseButton;    // Nút tăng
    public Button decreaseButton;    // Nút giảm
    private float currentFontSize; // Kích cỡ mặc định

    private void Start()
    {
        // Gán giá trị mặc định
        if (targetText != null)
            currentFontSize = targetText.fontSize;

        sizeInput.text = currentFontSize.ToString();

        // Gắn sự kiện cho các nút
        increaseButton.onClick.AddListener(IncreaseFontSize);
        decreaseButton.onClick.AddListener(DecreaseFontSize);

        // Gắn sự kiện cho ô nhập số
        sizeInput.onEndEdit.AddListener(UpdateFontSizeFromInput);
    }

    public void IncreaseFontSize()
    {
        currentFontSize++;
        UpdateFontSize();
    }

    public void DecreaseFontSize()
    {
        currentFontSize = Mathf.Max(1, currentFontSize - 1); // Đảm bảo kích cỡ không âm
        UpdateFontSize();
    }

    private void UpdateFontSizeFromInput(string input)
    {
        if (float.TryParse(input, out float newSize))
        {
            currentFontSize = Mathf.Max(1, newSize); // Đảm bảo kích cỡ không âm
            UpdateFontSize();
        }
    }

    private void UpdateFontSize()
    {
        if (targetText != null)
            targetText.fontSize = currentFontSize;

        sizeInput.text = currentFontSize.ToString(); // Cập nhật lại ô nhập số
    }
}
