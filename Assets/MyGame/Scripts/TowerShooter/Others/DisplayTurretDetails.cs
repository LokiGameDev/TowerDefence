using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayTurretDetails : MonoBehaviour
{
    public Image turretImage;
    public Sprite[] turretSprites;
    public TMP_Text nameText,
                    healthText,
                    firerateText,
                    damageText,
                    rangeText;
    public GameObject turretRepairButton;
    private PlayerTurret currentTurret;

    private float duration = 1;
    private float fromY = -120,
                  toY = 80;

    public void FillTheTurretDetails(PlayerTurret turret)
    {
        currentTurret = turret;
        TurretData data = turret.GetTurretDetails();
        nameText.text = data.name;
        healthText.text = data.health.ToString();
        firerateText.text = data.fireRate.ToString();
        damageText.text = data.damage.ToString();
        rangeText.text = data.range.ToString();
        switch(data.name)
        {
            case "Shooter":
                turretImage.sprite = turretSprites[0];
                break;
            case "Slowdowner":
                turretImage.sprite = turretSprites[1];
                break;
            case "Stunner":
                turretImage.sprite = turretSprites[2];
                break;
            case "Destroyer":
                turretImage.sprite = turretSprites[3];
                break;
        }
        if(data.health <= 0)
        {
            turretRepairButton.SetActive(true);
        }
        else
        {
            turretRepairButton.SetActive(false);
        }
    }

    public void RepairTheTurret()
    {
        if(currentTurret==null)
        {
            return;
        }
        else
        {
            if(GameManager.Instance.Purchasing(100))
            {
                currentTurret.RepairTheTurret();
                FillTheTurretDetails(currentTurret);
            }
        }
    }

    public void StartToShow()
    {
        StartCoroutine(SlideFromBottom());
    }

    public void StopToShow()
    {
        StopAllCoroutines();
        StartCoroutine(SlideToBottom());
    }

    IEnumerator SlideFromBottom()
    {
        float t = 0f;
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 pos = rt.anchoredPosition;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;
            float ease = 1 - Mathf.Pow(1 - p, 3); // Ease Out

            pos.y = Mathf.Lerp(fromY, toY, ease);
            rt.anchoredPosition = pos;
            //cg.alpha = Mathf.Lerp(fromA, toA, ease);

            yield return null;
        }
    }

    IEnumerator SlideToBottom()
    {
        float t = 0f;
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 pos = rt.anchoredPosition;
        float startPos = pos.y;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;
            float ease = 1 - Mathf.Pow(1 - p, 3); // Ease Out

            pos.y = Mathf.Lerp(startPos, fromY, ease);
            rt.anchoredPosition = pos;
            //cg.alpha = Mathf.Lerp(fromA, toA, ease);

            yield return null;
        }
    }

    public void CloseTheTurretDetails()
    {
        UIManager.Instance.ShowTheTurretDetails(false);
    }

}
