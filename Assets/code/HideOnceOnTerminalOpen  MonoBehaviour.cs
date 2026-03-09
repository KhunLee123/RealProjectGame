using UnityEngine;

public class HideOnceOnTerminalOpen : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToHide1;
    public GameObject objectToHide2;
    
    // เปลี่ยนจาก HintControl เป็น Hint ให้ตรงกับชื่อ Class ด้านบน
    public Hint hintScript; 

    private bool hasTriggered = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !hasTriggered)
        {
            // 1. ซ่อน Object
            if (objectToHide1 != null) objectToHide1.SetActive(false);
            if (objectToHide2 != null) objectToHide2.SetActive(false);

            // 2. ส่งสัญญาณไปบอกสคริปต์ Hint ให้แสดงข้อความ
            if (hintScript != null)
            {
                hintScript.ShowHint1();
            }

            hasTriggered = true;
            this.enabled = false;
        }
    }
}