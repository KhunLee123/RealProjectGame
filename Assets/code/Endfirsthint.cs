using UnityEngine;

public class Endfirsthint : MonoBehaviour
{
    // อ้างอิงถึงสคริปต์อื่นที่ต้องการส่งสัญญาณไปหา (เช่น Script Hint หรือ Door)
    public Hint hintScript; 

    // ฟังก์ชันนี้จะทำงานเมื่อมี Object อื่น (ที่มี Rigidbody) เดินเข้ามาในขอบเขต
    private void OnTriggerEnter(Collider other)
    {
        // เช็คว่าคนที่มาชนคือ "Player" ใช่ไหม (ต้องตั้ง Tag ที่ตัวผู้เล่นว่า Player ด้วยนะ)
        if (other.CompareTag("Player"))
        {
            Debug.Log("ผู้เล่นเดินมาสัมผัสจุดล่องหนแล้ว!");
            
            // ส่งสัญญาณไปที่สคริปต์อื่น
            if (hintScript != null)
            {
                hintScript.EndHint1();
            }

            // ถ้าอยากให้ทำงานครั้งเดียวแล้วหายไปเลย
            // gameObject.SetActive(false); 
        }
    }
}