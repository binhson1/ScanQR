using UnityEngine;
using UnityEngine.UI; // Sử dụng Text
using TMPro;          // Sử dụng TextMeshPro nếu cần

public class ScrollingTextVertical : MonoBehaviour
{
    public float speed = 50f; // Tốc độ di chuyển
    public RectTransform textTransform; // RectTransform của Text
    public float startPositionY = 500f; // Vị trí bắt đầu (trên cùng)
    public float endPositionY = -500f;  // Vị trí kết thúc (dưới cùng)
    public int numberOfDuplicates = 2; // Số lượng bản sao
    public int spacingPoint = 550;     // Khoảng cách giữa các bản sao

    private RectTransform[] duplicateTextTransforms; // Lưu trữ các bản sao
    private Text textComponent;
    private TextMeshProUGUI textMeshProComponent;

    private void Start()
    {
        if (textTransform == null)
        {
            Debug.LogError("Text Transform chưa được gán!");
            return;
        }

        // Lấy thành phần Text hoặc TextMeshPro từ textTransform
        textComponent = textTransform.GetComponent<Text>();
        textMeshProComponent = textTransform.GetComponent<TextMeshProUGUI>();

        if (textComponent == null && textMeshProComponent == null)
        {
            Debug.LogError("Không tìm thấy thành phần Text hoặc TextMeshPro trong Text Transform!");
            return;
        }

        // Khởi tạo mảng các RectTransform cho các bản sao
        duplicateTextTransforms = new RectTransform[numberOfDuplicates];

        // Đặt vị trí ban đầu cho Text gốc
        textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, startPositionY);

        // Tạo các bản sao
        for (int i = 0; i < numberOfDuplicates; i++)
        {
            GameObject duplicateText = Instantiate(textTransform.gameObject, textTransform.parent);
            RectTransform duplicateTransform = duplicateText.GetComponent<RectTransform>();

            // Tính toán vị trí ban đầu của từng bản sao
            float offset = spacingPoint * (i + 1);
            duplicateTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x, startPositionY + offset);

            // Lưu trữ bản sao trong mảng
            duplicateTextTransforms[i] = duplicateTransform;
        }
    }

    private void Update()
    {
        if (textComponent != null)
        {
            foreach (var duplicateTransform in duplicateTextTransforms)
            {
                var duplicateText = duplicateTransform.GetComponent<Text>();
                duplicateText.text = textComponent.text;
                duplicateText.fontSize = textComponent.fontSize;
            }
        }
        else if (textMeshProComponent != null)
        {
            foreach (var duplicateTransform in duplicateTextTransforms)
            {
                var duplicateTMP = duplicateTransform.GetComponent<TextMeshProUGUI>();
                duplicateTMP.text = textMeshProComponent.text;
                duplicateTMP.fontSize = textMeshProComponent.fontSize;
            }
        }
        // Di chuyển Text gốc
        textTransform.anchoredPosition += new Vector2(0, -speed * Time.deltaTime);

        // Di chuyển các bản sao
        foreach (var duplicateTransform in duplicateTextTransforms)
        {
            duplicateTransform.anchoredPosition += new Vector2(0, -speed * Time.deltaTime);
        }

        // Xử lý khi Text gốc vượt qua vị trí kết thúc
        if (textTransform.anchoredPosition.y < endPositionY)
        {
            // Đặt lại Text gốc phía trên phần tử cuối cùng
            RectTransform lastDuplicate = duplicateTextTransforms[numberOfDuplicates - 1];
            textTransform.anchoredPosition = new Vector2(
                textTransform.anchoredPosition.x,
                lastDuplicate.anchoredPosition.y + spacingPoint
            );
        }

        // Xử lý khi các Text sao chép vượt qua vị trí kết thúc
        for (int i = 0; i < duplicateTextTransforms.Length; i++)
        {
            if (duplicateTextTransforms[i].anchoredPosition.y < endPositionY)
            {
                // Đặt lại Text sao chép phía trên phần tử cuối cùng
                RectTransform previous = i == 0 ? textTransform : duplicateTextTransforms[i - 1];
                duplicateTextTransforms[i].anchoredPosition = new Vector2(
                    duplicateTextTransforms[i].anchoredPosition.x,
                    previous.anchoredPosition.y + spacingPoint
                );
            }
        }
    }
}
