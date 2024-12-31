using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollingTextController : MonoBehaviour
{
    // Tham chiếu đến các thành phần UI
    public ScrollingTextWithChildren scrollingTextScript; // Tham chiếu đến Script ScrollingTextWithChildren
    public TMP_InputField speedSlider; // Slider điều chỉnh tốc độ
    public TMP_InputField startPosInput; // InputField cho startPositionX
    public TMP_InputField endPosInput;   // InputField cho endPositionX
    public TMP_InputField numDupInput;   // InputField cho numberOfDuplicates
    public TMP_InputField spacingPointInput; // InputField cho spacingPoint

    private void Start()
    {
        // Gán các giá trị mặc định ban đầu từ Script ScrollingTextWithChildren
        if (scrollingTextScript != null)
        {
            speedSlider.text = scrollingTextScript.speed.ToString();
            startPosInput.text = scrollingTextScript.startPositionX.ToString();
            endPosInput.text = scrollingTextScript.endPositionX.ToString();
            numDupInput.text = scrollingTextScript.numberOfDuplicates.ToString();
            spacingPointInput.text = scrollingTextScript.spacingPoint.ToString();
        }

        // Gán các sự kiện cho UI
        speedSlider.onEndEdit.AddListener(UpdateSpeed);
        startPosInput.onEndEdit.AddListener(UpdateStartPosition);
        endPosInput.onEndEdit.AddListener(UpdateEndPosition);
        numDupInput.onEndEdit.AddListener(UpdateNumberOfDuplicates);
        spacingPointInput.onEndEdit.AddListener(UpdateSpacingPoint);
    }

    // Hàm cập nhật tốc độ
    private void UpdateSpeed(string value)
    {
        if (float.TryParse(value, out float newSpeednewSpeed))
        {
            if (scrollingTextScript != null)
            {
                scrollingTextScript.speed = newSpeednewSpeed;
            }
        }
    }

    // Hàm cập nhật startPositionX
    private void UpdateStartPosition(string value)
    {
        if (float.TryParse(value, out float newStartPosition))
        {
            if (scrollingTextScript != null)
            {
                scrollingTextScript.startPositionX = newStartPosition;
            }
        }
    }

    // Hàm cập nhật endPositionX
    private void UpdateEndPosition(string value)
    {
        if (float.TryParse(value, out float newEndPosition))
        {
            if (scrollingTextScript != null)
            {
                scrollingTextScript.endPositionX = newEndPosition;
            }
        }
    }

    // Hàm cập nhật numberOfDuplicates
    private void UpdateNumberOfDuplicates(string value)
    {
        if (int.TryParse(value, out int newNumberOfDuplicates))
        {
            if (scrollingTextScript != null)
            {
                scrollingTextScript.numberOfDuplicates = newNumberOfDuplicates;
            }
        }
    }

    // Hàm cập nhật spacingPoint
    private void UpdateSpacingPoint(string value)
    {
        if (int.TryParse(value, out int newSpacingPoint))
        {
            if (scrollingTextScript != null)
            {
                scrollingTextScript.spacingPoint = newSpacingPoint;
            }
        }
    }
}
