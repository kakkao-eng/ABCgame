// นำเข้าฟังก์ชันและคลาสต่าง ๆ ที่จำเป็นสำหรับการควบคุมของผู้เล่น
using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    // คลาส PlayerController ใช้สำหรับควบคุมการกระทำของผู้เล่นในเกม
    public class PlayerController : MonoBehaviour
    {
        // ตัวแปรสำหรับเก็บข้อมูลสุขภาพของผู้เล่น
        Health health;

        // ฟังก์ชันเริ่มต้นที่จะถูกเรียกเมื่อเริ่มเกม
        private void Start()
        {
            // ดึงคอมโพเนนต์ Health ที่แนบอยู่กับ GameObject นี้
            health = GetComponent<Health>(); 
        }

        // ฟังก์ชันที่ถูกเรียกในทุกเฟรมของเกม
        private void Update()
        {
            // ถ้าผู้เล่นตาย ให้ไม่ทำอะไร
            if (health.IsDead()) return;
            // ตรวจสอบการกระทำต่อสู้
            if (InteractWithCombat()) return;
            // ตรวจสอบการเคลื่อนที่
            if (InteractWithMovement()) return;
            // print("Noting to do."); // ใช้สำหรับการดีบัก
        }

        // ฟังก์ชันสำหรับจัดการการต่อสู้
        private bool InteractWithCombat()
        {
            // ใช้ Raycast เพื่อตรวจสอบวัตถุที่อยู่ในเส้นทางการมอง
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
               // ตรวจสอบว่ามีคอมโพเนนต์ CombatTarget อยู่หรือไม่
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null)  continue; // ถ้าไม่มีให้ข้ามไป

                GameObject targetGameObject = target.gameObject;

                // ตรวจสอบว่าผู้เล่นสามารถโจมตีเป้าหมายนี้ได้หรือไม่
                if(!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue; // ถ้าไม่สามารถโจมตีได้ให้ข้ามไป
                }

                // ถ้าผู้เล่นคลิกเมาส์ซ้าย
                if(Input.GetMouseButtonDown(0)) 
                {
                    GetComponent<Fighter>().Attack(target.gameObject); // เริ่มการโจมตี
                }
                return true; // ถ้ามีการกระทำการต่อสู้เกิดขึ้น
            }
            return false; // ถ้าไม่มีการกระทำการต่อสู้
        }

        // ฟังก์ชันสำหรับจัดการการเคลื่อนที่
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            // ใช้ Raycast เพื่อตรวจสอบว่าคลิกที่ตำแหน่งใดในโลก 3D
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                // ถ้าผู้เล่นคลิกเมาส์ขวา
                if (Input.GetMouseButton(1))
                {
                    // เริ่มการเคลื่อนที่ไปยังตำแหน่งที่คลิก
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true; // ถ้ามีการกระทำการเคลื่อนที่เกิดขึ้น
            }
            return false; // ถ้าไม่มีการกระทำการเคลื่อนที่
        }

        // ฟังก์ชันสำหรับดึง Ray ที่มาจากตำแหน่งของเมาส์ในหน้าจอ
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
