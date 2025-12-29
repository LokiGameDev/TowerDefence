using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MenuButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TMP_Text buttonText;
    private float originalSize = 30f;
    private float enlargedSize = 40f;
    private float targetSize = 30f;
    private float speed = 15f;
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        buttonText.fontSize = Mathf.Lerp(buttonText.fontSize, targetSize, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetSize = enlargedSize;
        Color color = buttonText.color;
        color.a = 1f;
        buttonText.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetSize = originalSize;
        Color color = buttonText.color;
        color.a = 0.8f;
        buttonText.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        targetSize = originalSize;
        Color color = buttonText.color;
        color.a = 0.8f;
        buttonText.color = color;
        AudioManager.Instance.PlayTheAudioClip(AudioType.MouseClick);
    }
}
