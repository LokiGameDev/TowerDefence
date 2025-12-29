using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager Instance;

    public RectTransform tooltipRect;
    public TextMeshProUGUI tooltipText;
    public Vector2 offset = new Vector2(15, -15);

    void Awake()
    {
        Instance = this;
        Hide();
    }

    void Update()
    {
        tooltipRect.position = Input.mousePosition + (Vector3)offset;
    }

    public void Show(string text)
    {
        if(text == "")
            return;
        tooltipText.text = text;
        tooltipRect.gameObject.SetActive(true);
    }

    public void Hide()
    {
        tooltipRect.gameObject.SetActive(false);
    }
}
