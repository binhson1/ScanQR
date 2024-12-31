using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResController : MonoBehaviour
{
    public TMP_InputField widthInputField;  // Tham chiếu đến TMP Input Field cho chiều rộng
    public TMP_InputField heightInputField; // Tham chiếu đến TMP Input Field cho chiều cao
    public Button applyButton;             // Nút để áp dụng
    public Canvas canvas;
    private CanvasScaler canvasScaler;      // Tham chiếu đến Canvas Scaler

    void Start()
    {
        // Lấy Canvas Scaler từ chính GameObject chứa script này
        canvasScaler = canvas.GetComponent<CanvasScaler>();
        if (canvasScaler == null)
        {
            Debug.LogError("CanvasScaler không được tìm thấy trên GameObject này!");
            return;
        }

        // Gán giá trị mặc định của CanvasScaler vào các ô nhập liệu
        InitializeInputFields();

        // Thêm sự kiện cho nút Apply
        applyButton.onClick.AddListener(ApplyNewResolution);
    }

    // Hàm khởi tạo dữ liệu vào Input Fields
    private void InitializeInputFields()
    {
        if (widthInputField != null && heightInputField != null)
        {
            // Lấy referenceResolution hiện tại và gán vào Input Fields
            widthInputField.text = canvasScaler.referenceResolution.x.ToString();
            heightInputField.text = canvasScaler.referenceResolution.y.ToString();
        }
        else
        {
            Debug.LogError("Input Fields không được gán trong Inspector!");
        }
    }

    // Hàm áp dụng resolution mới
    private void ApplyNewResolution()
    {
        if (widthInputField != null && heightInputField != null)
        {
            // Parse dữ liệu từ Input Fields
            if (int.TryParse(widthInputField.text, out int width) &&
                int.TryParse(heightInputField.text, out int height))
            {
                // Thay đổi Reference Resolution
                canvasScaler.referenceResolution = new Vector2(width, height);
                Debug.Log($"Reference Resolution đã được đặt thành: {width} x {height}");
            }
            else
            {
                Debug.LogError("Dữ liệu nhập không hợp lệ! Vui lòng nhập số nguyên.");
            }
        }
        else
        {
            Debug.LogError("Input Fields không được gán trong Inspector!");
        }
    }
}
