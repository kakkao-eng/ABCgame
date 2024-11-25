// นำเข้าฟังก์ชันและคลาสต่าง ๆ ที่จำเป็นสำหรับการจัดการสุขภาพ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core
{
    // คลาส Health ใช้สำหรับจัดการค่าชีวิตของตัวละคร
    public class Health : MonoBehaviour
    {
        // ตัวแปรสำหรับเก็บค่าชีวิต
        [SerializeField] float healthPoints = 100f;

        // ตัวแปรสำหรับตรวจสอบสถานะการตาย
        bool isDead = false;

        // ฟังก์ชันตรวจสอบว่าสถานะตัวละครตายหรือไม่
        public bool IsDead()
        {
            return isDead;
        }

        // ฟังก์ชันสำหรับจัดการความเสียหายที่ตัวละครได้รับ
        public void TakeDamage(float damage)
        {
            // คำนวณค่าชีวิตที่ลดลง โดยไม่ให้ต่ำกว่า 0
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            // ถ้าค่าชีวิตเป็น 0 ให้เรียกฟังก์ชัน Die
            if (healthPoints == 0)
            {
                Die();
            }
        }

        // ฟังก์ชันสำหรับจัดการการตายของตัวละคร
        private void Die()
        {
            FindObjectOfType<QuestManager>().EnemyKilled();
            // ถ้าตัวละครตายแล้ว ให้ไม่ทำอะไร
            if (isDead) return;

            isDead = true; // ตั้งค่าสถานะการตายเป็นจริง
            GetComponent<Animator>().SetTrigger("die"); // เรียกใช้งานอนิเมชันตาย
            GetComponent<ActionScheduler>().CancelCurrentAction(); // ยกเลิกการกระทำปัจจุบัน
            
            // ถ้าตัวละครเป็นบอส ให้โหลดฉากที่ 2
            if (gameObject.tag == "Boss")
            {
                SceneManager.LoadScene(2);
            }

            // ถ้าตัวละครเป็นผู้เล่น ให้โหลดฉากที่ 3
            if (gameObject.tag == "Player")
            {
                SceneManager.LoadScene(3);
            }
        }

        // ฟังก์ชัน Update ที่จะถูกเรียกในทุกเฟรมของเกม
        void Update()
        {

        }
    }
}