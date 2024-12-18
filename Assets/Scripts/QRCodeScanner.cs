using UnityEngine;
using UnityEngine.UI; // Để sử dụng UI
using ZXing;
using System.Collections;
using TMPro;
using System.Text.RegularExpressions;

public class QRCodeScanner : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    private IBarcodeReader barcodeReader;

    // Tham chiếu tới Text UI trong Canvas
    public TextMeshProUGUI resultText;
    public GameObject canva;
    public TextMeshProUGUI secondCanvasText;
    void Start()
    {
        // Kiểm tra nếu chưa gán Text
        if (resultText == null)
        {
            Debug.LogError("Chưa gán Text UI trong Canvas!");
            return;
        }

        if (Display.displays.Length > 1)
    {
        Display.displays[1].Activate(); // Kích hoạt màn hình phụ
    }

        // Khởi tạo ZXing reader
        barcodeReader = new BarcodeReader();

        // Bắt đầu mở camera
        StartCamera();
    }

    private void StartCamera()
    {
        // Khởi tạo camera
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            webCamTexture = new WebCamTexture(devices[0].name);
            webCamTexture.Play();
            StartCoroutine(ScanQRCode());
        }
        else
        {
            Debug.LogError("Không tìm thấy camera nào!");
        }
    }

    private void StopCamera()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
            webCamTexture = null;
        }
    }

    void OnGUI()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            // Vẽ camera feed lên toàn màn hình
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), webCamTexture, ScaleMode.StretchToFill);
        }
    }

    private IEnumerator ScanQRCode()
    {
        while (webCamTexture != null && webCamTexture.isPlaying)
        {
            try
            {
                // Lấy dữ liệu từ camera
                var data = webCamTexture.GetPixels32();
                var width = webCamTexture.width;
                var height = webCamTexture.height;

                // Quét mã QR
                var result = barcodeReader.Decode(data, width, height);
                if (result != null)
                {
                    Debug.Log("Mã QR: " + result.Text);

                    if (IsValidText(result.Text))
                    {
                        // Hiển thị kết quả và dừng camera
                        StartCoroutine(HandleResult(result.Text));
                        yield break; // Thoát vòng lặp quét
                    }
                }
            }
            catch
            {
                Debug.LogError("Lỗi khi quét mã QR.");
            }

            yield return new WaitForSeconds(0.5f); // Quét mỗi 0.5 giây
        }
    }

    private IEnumerator HandleResult(string result)
{
    // Tách chuỗi từ QR
    string[] parts = result.Split('-'); // Tách bằng dấu '-'
    string name = parts.Length > 0 ? parts[0].Trim() : "Không rõ tên";
    string title = parts.Length > 1 ? parts[1].Trim() : "Không rõ chức vụ";

    if (Display.displays.Length > 1)
    {
        // Hiển thị nội dung lên hai màn hình
        Display.displays[1].Activate();
        Debug.Log(name);
        resultText.text = name; // Tên hiển thị ở màn hình chính
        secondCanvasText.text = title; // Chức vụ hiển thị ở màn hình phụ
        Debug.Log(title);
    }
    else
    {
        // Hiển thị nội dung lên một màn hình
        resultText.text = name + "\n" + title; // Xuống dòng tên và chức vụ
    }

    canva.SetActive(true);    
    StopCamera();

    yield return new WaitForSeconds(5);

    canva.SetActive(false);
    secondCanvasText.text = "";
    StartCamera();
}


    private bool IsValidText(string text)
    {
        // Kiểm tra nếu chuỗi là một liên kết
        if (text.StartsWith("http://") || text.StartsWith("https://"))
        {
            return false;
        }

        // Kiểm tra nếu chuỗi chứa số hoặc ký tự đặc biệt
        // if (Regex.IsMatch(text, @"[^\p{L}\s]")) // `\p{L}` là ký tự chữ Unicode, `\s` là khoảng trắng
        // {
        //     return false;
        // }


        // Nếu tất cả điều kiện đều hợp lệ
        return true;
    }


    void OnDestroy()
    {
        StopCamera();
    }
}
