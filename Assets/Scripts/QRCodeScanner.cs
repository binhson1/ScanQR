using UnityEngine;
using UnityEngine.UI; // Để sử dụng UI
using ZXing;
using System.Collections;
using TMPro;
using System.Text.RegularExpressions;
using System;
using System.IO;
using OfficeOpenXml;
using System.Linq;

public class QRCodeScanner : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    public Renderer targetRenderer;
    private IBarcodeReader barcodeReader;

    // Tham chiếu tới Text UI trong Canvas
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI thirdText;
    public TextMeshProUGUI fourthText;


    public GameObject canva;
    private string excelFilePath;
    private string filePath;
    public TextMeshProUGUI secondCanvasText;
    [SerializeField] public LogManager logManager;

    void Start()
    {
        // Kiểm tra nếu chưa gán Text
        if (resultText == null)
        {
            Debug.LogError("Chưa gán Text UI trong Canvas!");
            // return;
        }

        // Display.displays[1].Activate();
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            Display.displays[2].Activate();
            Display.displays[3].Activate();

        }

        // Khởi tạo ZXing reader
        barcodeReader = new BarcodeReader();

        // Bắt đầu mở camera
        StartCamera();

        // Bật License của EPPlus (vì từ v5, EPPlus yêu cầu bật License cho non-commercial)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // Lấy đường dẫn thư mục gốc
        string baseDirectory = Path.GetDirectoryName(Application.dataPath);

        // Đường dẫn tới file Excel trong thư mục MyRelativeFolder
        string fileName = "Data";
        filePath = Path.Combine(baseDirectory, fileName);
        Debug.Log(filePath);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        // Kiểm tra file có tồn tại không
        if (!File.Exists(filePath))
        {
            // logManager.AddLog($"File không tồn tại: {filePath}");
            Debug.Log($"File không tồn tại: {filePath}");
            return;
        }
    }

    private void StartCamera()
    {
        // Khởi tạo camera
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            webCamTexture = new WebCamTexture(devices[0].name);
            webCamTexture.Play();
            if (targetRenderer != null)
            {
                targetRenderer.material.mainTexture = webCamTexture;
            }
            StartCoroutine(ScanQRCode());
            logManager.AddLog("Đã chạy được camera");
            logManager.AddLog("Đã chạy được camera");
            logManager.AddLog("Đã chạy được camera");

            Debug.Log("Đã chạy được camera");

        }
        else
        {
            Debug.LogError("Không tìm thấy camera nào!");
            logManager.AddLog("Không tìm thấy camera nào!");
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
            // GUI.DrawTexture(new Rect(1000, 30, Screen.width / 3.5f, Screen.height / 3.5f), webCamTexture, ScaleMode.StretchToFill);
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
        // canva.SetActive(true);
        StopCamera();

        // Đọc dữ liệu từ file Excel
        string name = "Không tìm thấy";
        string honor = "Không tìm thấy";
        string file = filePath + "/QRCheckIn.xlsx";
        if (File.Exists(file))
        {
            using (var package = new ExcelPackage(new FileInfo(file)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Bắt đầu từ hàng 2 (bỏ qua tiêu đề)
                    {
                        string id = worksheet.Cells[row, 1].Text; // Giả sử cột ID là cột 1
                        if (id == result)
                        {
                            name = worksheet.Cells[row, 2].Text; // Giả sử cột name là cột 2
                            honor = worksheet.Cells[row, 3].Text; // Giả sử cột honor là cột 3
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy file Excel tại: " + excelFilePath);
        }


        if (Display.displays.Length > 1)
        {
            // Hiển thị nội dung lên hai màn hình
            // Display.displays[1].Activate();
            //GIANG KHIET LY
            // Tách chuỗi theo dấu cách
            thirdText.text = name;
            string[] words = name.Split(' ');

            // Nối chuỗi với ký tự xuống dòng
            resultText.text = string.Join("\n", words);
            Debug.Log(name);
            // resultText.text = string.Join("\n", words); ; // Tên hiển thị ở màn hình chính
            secondCanvasText.text = honor; // Chức vụ hiển thị ở màn hình phụ
            fourthText.text = "MDRT";
            Debug.Log(honor);

        }
        else
        {
            // Hiển thị nội dung lên một màn hình
            resultText.text = $"Name: {name}\nHonor: {honor}";
        }


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
