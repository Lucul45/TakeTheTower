using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameDataManager : Singleton<FrameDataManager>
{
    [SerializeField] private TextMeshProUGUI _startupFrameText;
    [SerializeField] private TextMeshProUGUI _activeFrameText;
    [SerializeField] private TextMeshProUGUI _cooldownFrameText;
    [SerializeField] private TextMeshProUGUI _advantageFrameText;
    
    public void ChangeFrameDataUI(AttackData attackData)
    {
        _startupFrameText.text = "Start up frames : " + attackData.AttackStartup;
        _activeFrameText.text = "Active frames : ";
        _cooldownFrameText.text = "Cooldown frames : " + attackData.AttackCooldown;
        _advantageFrameText.text = "Advantage frames : ";
    }
}
