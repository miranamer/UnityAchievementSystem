using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementManager : MonoBehaviour
{

    public Dictionary<string, Achievement> achievements;
    public TextMeshProUGUI achievementTMP;

    void Start()
    {
        InitialiseAchievements();
    }

    void InitialiseAchievements()
    {
        if(achievements != null)
        {
            return;
        }

        achievements = new Dictionary<string, Achievement>();
        achievements.Add("Teleport", new Achievement("Teleport Discovered!", achievementTMP));
        achievements.Add("SpeedPad", new Achievement("Speed Pad Discovered!", achievementTMP));
        achievements.Add("JumpPad", new Achievement("Jump Pad Discovered!", achievementTMP));
    }

    public IEnumerator ShowAchievement(Achievement achievement, TextMeshProUGUI achievementTMP)
    {
        achievementTMP.text = "Achievement Unlocked - " + achievement.achievementName;
        achievementTMP.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        achievementTMP.gameObject.SetActive(false);
        achievement.achievementUnlocked = true;
    }
    
}

public class Achievement
{
    public string achievementName { get; set; }
    public TextMeshProUGUI achievementGui { get; set; }
    public bool achievementUnlocked { get; set; }

    public Achievement(string achievementName, TextMeshProUGUI achievementGui)
    {
        this.achievementName = achievementName;
        this.achievementGui = achievementGui;
        this.achievementUnlocked = false;
    }
}
