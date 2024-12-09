using UnityEngine;

public class ToggleFullscreen : MonoBehaviour
{
    void Update()
    {
        // Kiểm tra xem người dùng có nhấn F11 hay không
        if (Input.GetKeyDown(KeyCode.F11))
        {
            // Chuyển đổi giữa chế độ cửa sổ và toàn màn hình
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
