using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum EffectType
    {
        None,
        HoverScale,
        HoverColor,
        HoverShake,
        HoverGlow
    }

    public EffectType effectType = EffectType.None;

    public float speed = 10f;

    public float scaleAmount = 1.1f;

    public Color hoverColor = Color.yellow;
    private Color originalColor;

    public float glowAlpha = 0.6f;

    private Image img;
    private RectTransform rect;

    public GameObject scaleObject;

    [TextArea]
    public string tooltipText;

    void Awake()
    {
        if(scaleObject!=null) rect = scaleObject.GetComponent<RectTransform>();
        else rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        if (img) originalColor = img.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipManager.Instance?.Show(tooltipText);
        switch (effectType)
        {
            case EffectType.None:
                break;
            case EffectType.HoverScale:
                StartCoroutine(ScaleEffect(Vector3.one * scaleAmount));
                break;

            case EffectType.HoverColor:
                if (img) img.color = hoverColor;
                break;

            case EffectType.HoverShake:
                StartCoroutine(Shake());
                break;

            case EffectType.HoverGlow:
                if (img)
                {
                    Color c = img.color;
                    c.a = glowAlpha;
                    img.color = c;
                }
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance?.Hide();
        if (effectType == EffectType.None) return;
        StartCoroutine(ScaleEffect(Vector3.one));

        if (img)
        {
            Color c = originalColor;
            img.color = c;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (effectType == EffectType.None) return;
        rect.localScale = Vector3.one;
        AudioManager.Instance.PlayTheAudioClip(AudioType.MouseClick);
    }

    private IEnumerator Shake()
    {
        float t = 0f;
        Vector3 originalPos = rect.anchoredPosition;

        while (t < 0.3f)
        {
            t += Time.deltaTime;
            float strength = Mathf.Lerp(10, 0, t / 0.3f);
            rect.anchoredPosition = (Vector2)originalPos + Random.insideUnitCircle * strength;
            yield return null;
        }

        rect.anchoredPosition = originalPos;
    }

    private IEnumerator ScaleEffect(Vector3 targetScale)
    {
        Vector3 initialScale = rect.localScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            rect.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        rect.localScale = targetScale;
    }
}