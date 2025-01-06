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
using System.Collections.Generic;

public class QRCodeScanner : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    public Renderer targetRenderer;
    private IBarcodeReader barcodeReader;

    // Tham chiếu tới Text UI trong Canvas
    public TextMeshProUGUI firstText;
    public TextMeshProUGUI secondText;
    public TextMeshProUGUI thirdText;
    public TextMeshProUGUI fourthText;
    public TextMeshProUGUI nameText;

    public GameObject logo1;
    public GameObject logo2;
    public GameObject logo3;
    public GameObject logo4;

    public int waitingTime = 5;
    private string excelFilePath;
    private string filePath;
    [SerializeField] public LogManager logManager;

    void Start()
    {
        // Display.displays[1].Activate();
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            Display.displays[2].Activate();
            Display.displays[3].Activate();
            Display.displays[4].Activate();
            Display.displays[5].Activate();
        }

        // Khởi tạo ZXing reader
        barcodeReader = new BarcodeReader();

        // Bắt đầu mở camera
        StartCamera();
        //StartCoroutine(HandleResult("ER MA8SNN99A8ULWL4XB65367LVSB"));

        // Bật License của EPPlus (vì từ v5, EPPlus yêu cầu bật License cho non-commercial)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // Lấy đường dẫn thư mục gốc
        string baseDirectory = Path.GetDirectoryName(Application.dataPath);

        // Đường dẫn tới file Excel trong thư mục MyRelativeFolder
        string fileName = "Data";
        filePath = Path.Combine(baseDirectory, fileName);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        // Kiểm tra file có tồn tại không
        if (!File.Exists(filePath))
        {
            // logManager.AddLog($"File không tồn tại: {filePath}");
            Debug.Log($"File không tồn tại: {filePath}");
            logManager.AddLog($"File không tồn tại: {filePath}");
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

            Debug.Log("Đã chạy được camera");
            //logManager.AddLog("Đã chạy được camera");

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
                        HandleResult(result.Text);
                        yield break; // Thoát vòng lặp quét
                    }
                    else
                    {
                        logManager.AddLog("Ma QR ko hop le: " + result.Text);
                    }
                }
            }
            catch
            {
                Debug.LogError("Lỗi khi quét mã QR.");
                logManager.AddLog("Lỗi khi quét mã QR.");
            }

            yield return new WaitForSeconds(2f); // Quét mỗi giây
        }
    }


    private void HandleResult(string result)
    {
        // StopCamera();
        // Debug.Log("OKEY");
        // Đọc dữ liệu từ file Excel
        string name = "";
        string honor1 = "";
        string honor2 = "";
        string honor3 = "";
        string honor4 = "";
        string honor5 = "";
        string honor6 = "";
        string honor7 = "";


        string file = filePath + "/QR code foyer to EG final_05012025.xlsx";
        // file = "D:/Unity/250102_Vinh danh_QR code-Revise.xlsx";
        // file = "D:/Unity/QR code foyer to EG final_05012025.xlsx";
        string id = "";

        if (File.Exists(file))
        {
            using (var package = new ExcelPackage(new FileInfo(file)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Bắt đầu từ hàng 2 (bỏ qua tiêu đề)
                    {
                        id = worksheet.Cells[row, 4].Text; // Giả sử cột ID là cột 1
                        if (id == result)
                        {
                            name = worksheet.Cells[row, 3].Text;
                            honor1 = worksheet.Cells[row, 6].Text;
                            honor2 = worksheet.Cells[row, 7].Text;
                            honor3 = worksheet.Cells[row, 8].Text;
                            honor4 = worksheet.Cells[row, 9].Text;
                            honor5 = worksheet.Cells[row, 10].Text;
                            honor6 = worksheet.Cells[row, 11].Text;
                            honor7 = worksheet.Cells[row, 12].Text;
                            break;
                        }
                    }
                    if (id != result)
                    {
                        logManager.AddLog("Khong tim thay QR: " + result);
                        Debug.Log("Khong tim thay QR: " + result);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy file Excel tại: " + excelFilePath);
            logManager.AddLog("Không tìm thấy file Excel tại: " + excelFilePath);
        }
        if (id == result)
        {
            // Đặt lại tất cả các Text trước khi hiển thị dữ liệu mới
            logo1.SetActive(false);
            logo2.SetActive(false);
            logo3.SetActive(false);
            logo4.SetActive(false);
            nameText.text = "";
            firstText.text = "";
            secondText.text = "";
            thirdText.text = "";
            fourthText.text = "";

            // Hiển thị tên
            nameText.text = name;

            // Hiển thị dữ liệu honor
            List<string> honors = new List<string> { honor1, honor2, honor3, honor4, honor5, honor6, honor7 };
            int textIndex = 0; // Chỉ số để theo dõi Text đang sử dụng

            for (int i = 0; i < honors.Count; i++)
            {
                if (!string.IsNullOrEmpty(honors[i])) // Kiểm tra nếu dữ liệu honor không rỗng
                {
                    switch (textIndex)
                    {
                        case 0:
                            firstText.text = honors[i];
                            textIndex++;
                            break;
                        case 1:
                            secondText.text = honors[i];
                            textIndex++;
                            break;
                        case 2:
                            thirdText.text = honors[i];
                            textIndex++;
                            break;
                        case 3:
                            fourthText.text = honors[i];
                            textIndex++;
                            break;
                        case 4:
                            firstText.text += "\n" + honors[i];
                            textIndex++;
                            break;
                        case 5:
                            thirdText.text += "\n" + honors[i];
                            textIndex++;
                            break;
                        case 6:
                            fourthText.text += "\n" + honors[i];
                            textIndex++;
                            break;
                    }
                }
            }

            // Nếu thiếu dữ liệu, kích hoạt các logo tương ứng
            if (string.IsNullOrEmpty(firstText.text)) logo1.SetActive(true);
            if (string.IsNullOrEmpty(secondText.text)) logo2.SetActive(true);
            if (string.IsNullOrEmpty(thirdText.text)) logo3.SetActive(true);
            if (string.IsNullOrEmpty(fourthText.text)) logo4.SetActive(true);
        }

        // yield return new WaitForSeconds(waitingTime);
        // firstText.text = "";
        // secondText.text = "";
        // thirdText.text = "";
        // fourthText.text = "";

        // StartCamera();
    }

    private bool IsValidText(string text)
    {
        // Kiểm tra nếu chuỗi là một liên kết
        if (text.StartsWith("http://") || text.StartsWith("https://"))
        {
            return false;
        }

        if (text == "")
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
