using UnityEngine;
using UnityEngine.UI; // For Text
using TMPro;          // For TextMeshPro if needed

public class ScrollingTextWithChildren : MonoBehaviour
{
    public float speed = 50f;                     // Speed of text movement
    public RectTransform textTransform;          // RectTransform of the parent text
    public float startPositionX = -500f;         // Starting position (off-screen to the left)
    public float endPositionX = 500f;            // Ending position (off-screen to the right)
    public GameObject textPrefab;                // Prefab for child text objects
    public int childCount = 3;                   // Number of child text objects
    public float childSpacing = 50f;             // Spacing between child objects

    private RectTransform[] childTransforms;     // Array to store child RectTransforms

    private void Start()
    {
        if (textTransform == null)
        {
            textTransform = GetComponent<RectTransform>();
        }

        // Initialize parent text at the starting position
        textTransform.anchoredPosition = new Vector2(startPositionX, textTransform.anchoredPosition.y);

        // Create child objects
        InitializeChildTexts();
    }

    private void Update()
    {
        // Move the parent text from left to right
        textTransform.anchoredPosition += new Vector2(speed * Time.deltaTime, 0);

        // Loop parent text back to start if it exceeds the end position
        if (textTransform.anchoredPosition.x > endPositionX)
        {
            textTransform.anchoredPosition = new Vector2(startPositionX, textTransform.anchoredPosition.y);
        }

        // Update child texts' positions relative to the parent
        UpdateChildPositions();
    }

    private void InitializeChildTexts()
    {
        // Destroy existing children if any (useful for runtime updates)
        foreach (Transform child in transform)
        {
            // Destroy(child.gameObject);
        }

        // Initialize array for child RectTransforms
        childTransforms = new RectTransform[childCount];

        // Create child objects
        for (int i = 0; i < childCount; i++)
        {
            GameObject newChild = Instantiate(textPrefab, transform); // Create child under parent
            RectTransform childRect = newChild.GetComponent<RectTransform>();
            childTransforms[i] = childRect;

            // Position child relative to parent with spacing
            float offset = -(i + 1) * childSpacing; // Negative offset to position behind parent
            childRect.anchoredPosition = new Vector2(textTransform.anchoredPosition.x + offset, textTransform.anchoredPosition.y);
        }
    }

    private void UpdateChildPositions()
    {
        // Update each child's position relative to the parent
        for (int i = 0; i < childCount; i++)
        {
            if (childTransforms[i] != null)
            {
                float offset = -(i + 1) * childSpacing; // Maintain spacing
                childTransforms[i].anchoredPosition = new Vector2(textTransform.anchoredPosition.x + offset, textTransform.anchoredPosition.y);
            }
        }
    }

    public void SetChildCount(int newCount)
    {
        // Update the number of child objects at runtime
        childCount = newCount;
        InitializeChildTexts(); // Recreate children with new count
    }
}
