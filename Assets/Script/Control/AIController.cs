// นำเข้าฟังก์ชันและคลาสต่าง ๆ ที่จำเป็นสำหรับการควบคุม AI
using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    // คลาส AIController ใช้สำหรับควบคุมพฤติกรรมของ AI ในเกม
    public class AIController : MonoBehaviour
    {
        // กำหนดค่าความห่างที่ AI จะไล่ล่าผู้เล่น
        [SerializeField] float chaseDistance = 5f;
        // เวลาที่ AI จะสงสัยหลังจากไม่เห็นผู้เล่น
        [SerializeField] float suspicionTime = 3f;
        // เส้นทางลาดตระเวน
        [SerializeField] PatrolPath patrolPath;
        // ระยะห่างที่ AI จะต้องถึง waypoint
        [SerializeField] float waypointTolerance = 1f;
        // เวลาที่ AI จะอยู่ที่ waypoint
        [SerializeField] float waypointDwellTime = 3f;
        // ความเร็วที่ AI จะลาดตระเวน
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        // ตัวแปรสำหรับเก็บข้อมูลการต่อสู้, สุขภาพ, และการเคลื่อนที่
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        // ตำแหน่งของ AI เมื่อไม่ทำอะไร
        Vector3 guardPosition;
        // เวลาที่ AI ไม่เห็นผู้เล่น
        float timeSinceLastSawPlayer = Mathf.Infinity;
        // เวลาที่ AI มาถึง waypoint
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        // ดัชนีของ waypoint ปัจจุบัน
        int currentWaypointIndex = 0;

        // ฟังก์ชันเริ่มต้นที่จะถูกเรียกเมื่อเริ่มเกม
        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = transform.position; // เก็บตำแหน่งเริ่มต้นของ AI
        }

        // ฟังก์ชันที่ถูกเรียกในทุกเฟรมของเกม
        private void Update()
        {
            if (health.IsDead()) return; // ถ้าศัตรูตายให้ไม่ทำอะไร

            // ตรวจสอบว่าศัตรูอยู่ในระยะโจมตีผู้เล่นหรือไม่
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour(); // ถ้าอยู่ในระยะโจมตีให้ทำการโจมตี
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour(); // ถ้าอยู่ในระยะสงสัยให้ทำพฤติกรรมสงสัย
            }
            else
            {
                PatrolBehaviour(); // ถ้าไม่พบผู้เล่นให้ทำพฤติกรรมลาดตระเวน
            }
           
            UpdateTimers(); // อัปเดตตัวจับเวลา

        }

        // ฟังก์ชันอัปเดตตัวจับเวลา
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        // ฟังก์ชันสำหรับพฤติกรรมลาดตระเวน
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            // ถ้ามีเส้นทางลาดตระเวน
            if (patrolPath != null)
            {
                if (AtWaypoint()) // ตรวจสอบว่าอยู่ที่ waypoint หรือไม่
                {
                    timeSinceArrivedAtWaypoint = 0; // รีเซ็ตตัวจับเวลา
                    CycleWaypoint(); // เลื่อนไปยัง waypoint ถัดไป
                }
                nextPosition = GetCurrentWaypoint(); // ดึงตำแหน่ง waypoint ปัจจุบัน
            }

            // ถ้าอยู่ที่ waypoint นานกว่ากำหนด ให้เคลื่อนที่ไปยัง waypoint
            if(timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
              mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        // ฟังก์ชันตรวจสอบว่าอยู่ที่ waypoint หรือไม่
        private bool AtWaypoint()
        {
            float distanceTowaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceTowaypoint < waypointTolerance; // คืนค่าความห่างจาก waypoint
        }
        
        // ฟังก์ชันเลื่อนไปยัง waypoint ถัดไป
        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        // ฟังก์ชันดึงตำแหน่ง waypoint ปัจจุบัน
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        // ฟังก์ชันสำหรับพฤติกรรมสงสัย
        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction(); // ยกเลิกการกระทำปัจจุบัน
        }

        // ฟังก์ชันสำหรับพฤติกรรมโจมตี
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0; // รีเซ็ตตัวจับเวลาที่ไม่เห็นผู้เล่น
            fighter.Attack(player); // ทำการโจมตีผู้เล่น
        }

        // ฟังก์ชันตรวจสอบว่าศัตรูอยู่ในระยะโจมตีผู้เล่นหรือไม่
        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance; // คืนค่าความห่างจากผู้เล่น
        }

        // เรียกโดย Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue; // เปลี่ยนสี Gizmo
            Gizmos.DrawWireSphere(transform.position, chaseDistance); // วาดวงกลมแสดงระยะการไล่ล่าของ AI
        }
    }
}
