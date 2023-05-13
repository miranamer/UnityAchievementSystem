using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed = 10f;
    public float explosiveForce = 20f;
    public float teleportTimer = 0;
    public float teleportTime = 1.5f;
    public bool onSpeedPad = false;

    public int teleportCounter = 0;
    
    public Rigidbody playerRb;
    public AchievementManager achievementManager;

    public List<string> achievementsToUnlock;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        achievementManager = GameObject.Find("AchievementManager").GetComponent<AchievementManager>();
    }

    void Update()
    {
        HandleMovement();
        CheckAchievementsToUnlock();
    }

    void CheckAchievementsToUnlock()
    {
        if (achievementsToUnlock.Count > 0)
        {
            if (!achievementManager.achievementTMP.gameObject.activeInHierarchy)
            {
                UnlockAchievement(achievementsToUnlock[0]);
                achievementsToUnlock.RemoveAt(0);
            }
        }
    }

    void UnlockAchievement(string achievementName)
    {

        if (achievementManager.achievementTMP.gameObject.activeInHierarchy) // if another achievement is curr popped up
        {
            achievementsToUnlock.Add(achievementName);
        }
        
        else if (!achievementManager.achievements[achievementName].achievementUnlocked)
        {
            StartCoroutine(achievementManager.ShowAchievement(achievementManager.achievements[achievementName], achievementManager.achievementTMP));
        }
    }

    void HandleMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(-verticalMovement, 0, horizontalMovement) * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "BouncePad")
        {
            UnlockAchievement("JumpPad");
            playerRb.AddForce(Vector3.up * explosiveForce, ForceMode.Impulse);
        }

        if (other.gameObject.name == "Teleport1")
        {
            if (Time.time >= teleportTimer + teleportTime)
            {
                TeleportPlayer(other, "Teleport2");
            }
        }

        if (other.gameObject.name == "Teleport2")
        {
            if (Time.time >= teleportTimer + teleportTime)
            {
                TeleportPlayer(other, "Teleport1");
            }
        }

        if(other.gameObject.name == "SpeedPad")
        {
            UnlockAchievement("SpeedPad");

            speed = 20f;
            onSpeedPad = true;
        }

        if(other.gameObject.name != "SpeedPad" && onSpeedPad)
        {
            speed = 10f;
            onSpeedPad = false;
        }
    }

    void TeleportPlayer(Collider other, string teleportPoint)
    {

        UnlockAchievement("Teleport");

        Transform parent = other.gameObject.transform.parent;
        Transform tp = parent.Find(teleportPoint);

        transform.position = new Vector3(tp.position.x, transform.position.y, tp.position.z);
        teleportTimer = Time.time;

        teleportCounter++;
    }
}
