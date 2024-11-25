using UnityEngine;
using UnityEngine.UI; // สำหรับการใช้งาน UI
using System.Collections;

public class QuestManager : MonoBehaviour
{
    public int killTarget = 3;
    private int killCount = 0;
    public Text questText;
    private int currentQuestIndex = 1;
    private bool isQuestComplete = false; // เพิ่มตัวแปรสถานะ

    void Start()
    {
        StartQuest();
    }

    private void StartQuest()
    {
        killCount = 0;
        isQuestComplete = false; // รีเซ็ตสถานะเมื่อเริ่มเควสใหม่
        UpdateQuestText();
    }

    public void EnemyKilled()
    {
        if (isQuestComplete) return; // หยุดการทำงานหากเควสจบแล้ว
        killCount++;
        if (killCount >= killTarget)
        {
            CompleteQuest();
        }
        else
        {
            UpdateQuestText();
        }
    }

    private void CompleteQuest()
    {
        isQuestComplete = true; // ตั้งสถานะเควสจบแล้ว
        questText.text = "Quest Complete!";
        StartCoroutine(WaitAndEndQuest(30)); // ตั้งเวลานับถอยหลังสำหรับจบเควส
    }

    private IEnumerator WaitAndEndQuest(float waitTime)
    {
        float countdown = waitTime;
        while (countdown > 0)
        {
            questText.text = "Quest Complete! Ending in: " + Mathf.FloorToInt(countdown) + " seconds...";
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        // เมื่อเวลาหมด แสดงข้อความสุดท้าย
        questText.text = "All Quests Completed! Thank you for playing.";
    }

    private void UpdateQuestText()
    {
        int remainingEnemies = killTarget - killCount;
        questText.text = "Quest " + currentQuestIndex + ": Enemies Left: " + remainingEnemies;
    }
}

