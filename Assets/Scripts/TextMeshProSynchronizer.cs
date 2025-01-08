using UnityEngine;
using TMPro;

public class SimpleTextMeshProSynchronizer : MonoBehaviour
{
    // TextMeshPro mẫu
    public TextMeshProUGUI sourceTextMeshPro;

    // Mảng các TextMeshPro sẽ đồng bộ với mẫu
    public TextMeshProUGUI[] targetTextMeshPros;

    private void Update()
    {
        // Kiểm tra nếu mẫu không null
        if (sourceTextMeshPro != null)
        {
            // Duyệt qua mảng các TextMeshPro và đồng bộ nội dung
            foreach (var targetTextMeshPro in targetTextMeshPros)
            {
                if (targetTextMeshPro != null)
                {
                    targetTextMeshPro.text = sourceTextMeshPro.text;
                }
            }
        }
    }
}
