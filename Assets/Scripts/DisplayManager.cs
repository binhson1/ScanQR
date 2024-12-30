using UnityEngine;
using TMPro;

public class DisplayManager : MonoBehaviour
{
    public TMP_Dropdown[] dropdowns; // Mảng chứa 4 TMP_Dropdown
    public Camera[] cameras;         // Mảng chứa 4 Camera

    void Start()
    {
        // Khởi động camera
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
        // Gán giá trị mặc định cho Dropdown từ các Camera
        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].ClearOptions();
            for (int j = 0; j < Display.displays.Length; j++)
            {
                dropdowns[i].options.Add(new TMP_Dropdown.OptionData($"Monitor {j}"));
                // Debug.Log($"Display {j}");
            }
            dropdowns[i].value = cameras[i].targetDisplay;
        }
    }
    public void ApplyChanges()
    {
        // Gán lại Display cho Camera dựa vào lựa chọn
        for (int i = 0; i < dropdowns.Length; i++)
        {
            int selectedDisplay = dropdowns[i].value;
            cameras[i].targetDisplay = selectedDisplay;
        }

        // lần đầu chuyển đổi
        //cameras[3].targetDisplay = 0;
        //cameras[0].targetDisplay = 3;

        ////////////////////////////

        //cameras[0].targetDisplay = 3;
        //cameras[1].targetDisplay = 1;
        //cameras[2].targetDisplay = 2;
        //cameras[3].targetDisplay = 0;
        //cameras[4].targetDisplay = 4;    

        //////////////////////
        // lần thứ 2 chuyển đổi

        //cameras[0].targetDisplay = 3;
        //cameras[1].targetDisplay = 1;
        //cameras[2].targetDisplay = 2;
        //cameras[3].targetDisplay = 4;
        //cameras[4].targetDisplay = 0;    

        // cameras[0].targetDisplay = 4; lúc này bỏ display = 3 
        // cameras[3].targetDisplay = 3; bị thiếu ở chỗ này, display = 3 bị bỏ đi mà không được gán cho camera nào
        // cameras[4].targetDisplay = 0;
    }
}
