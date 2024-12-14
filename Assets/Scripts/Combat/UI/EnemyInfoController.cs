using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoController : MonoBehaviour
{
    public ProgressBarController healthBar;   
    public TextMeshProUGUI nameText;
    public GameObject StatusEffectPanel;
    public Image portrait;

    internal void UpdateInfo(Unit self)
    {
        healthBar.SetBarValue(self.CurrentHealth, self.MaxHealth);
        nameText.text = self.name;
        if (self.image != null)
            portrait.sprite = self.image;
    }
}