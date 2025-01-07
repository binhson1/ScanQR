using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public ScrollingTextVertical scrollingTextVerticalScript; // Tham chiếu đến Script ScrollingTextVertical
    public ScrollingTextVerticalReverse scrollingTextVerticalReverseScript; // Tham chiếu đến Script ScrollingTextVerticalReverse

    public TMP_InputField speedSlider; // Slider điều chỉnh tốc độ

    private void Start()
    {
        // Gán các giá trị mặc định ban đầu từ Script ScrollingTextVertical
        if (scrollingTextVerticalScript != null)
        {
            speedSlider.text = scrollingTextVerticalScript.speed.ToString();
        }

        // Gán sự kiện cho UI
        speedSlider.onEndEdit.AddListener(UpdateSpeed);
    }
    public void UpdateSpeed(string value)
    {
        if (float.TryParse(value, out float newSpeed))
        {
            if (scrollingTextVerticalScript != null)
            {
                scrollingTextVerticalScript.speed = newSpeed;
            }

            if (scrollingTextVerticalReverseScript != null)
            {
                scrollingTextVerticalReverseScript.speed = newSpeed;
            }
        }
    }


}
