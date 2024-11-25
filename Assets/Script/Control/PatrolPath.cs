// ใช้สำหรับนำเข้าฟังก์ชันและคลาสต่าง ๆ ที่จำเป็นใน Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    // คลาส PatrolPath ใช้สำหรับกำหนดเส้นทางการเดินของ NPC
    public class PatrolPath : MonoBehaviour
    {
        // กำหนดรัศมีของ Gizmo สำหรับจุด waypoint
        const float waypointGizmoRadius = 0.3f;

        // ฟังก์ชันที่เรียกใช้เมื่อแสดง Gizmos ใน Editor
        private void OnDrawGizmos()
        {
            // วนลูปผ่านลูกที่อยู่ใน GameObject
            for (int i = 0; i < transform.childCount; i++)
            {
                // ดึงดัชนีของ waypoint ถัดไป
                int j = GetNextIndex(i);
                // วาด sphere ที่ตำแหน่งของ waypoint
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                // วาดเส้นเชื่อมจาก waypoint ปัจจุบันไปยัง waypoint ถัดไป
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        // ฟังก์ชันเพื่อดึงดัชนีของ waypoint ถัดไป
        public int GetNextIndex(int i)
        {
            // ถ้าปัจจุบันเป็น waypoint สุดท้าย ให้กลับไปที่ waypoint แรก
            if(i + 1 == transform.childCount)
            {
                return 0;
            }
            // ถ้าไม่ใช่ให้กลับดัชนีถัดไป
            return i + 1;
        }

        // ฟังก์ชันเพื่อดึงตำแหน่งของ waypoint ที่ระบุ
        public Vector3 GetWaypoint(int i)
        {
            // คืนค่าตำแหน่งของลูก GameObject ที่ระบุ
            return transform.GetChild(i).position;
        }
    }
}