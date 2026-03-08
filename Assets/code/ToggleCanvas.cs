using UnityEngine;
using TMPro;
using UnityEngine.UI; // 🚨 ต้องเพิ่มบรรทัดนี้ เพื่อให้สคริปต์รู้จักคำสั่ง Image

public class ToggleCanvas : MonoBehaviour
{
    public Transform terminalWindow; 
    public TMP_InputField inputField;
    
    [Header("ลากหน้าต่าง Terminal (Image) มาใส่อีกรอบเพื่อคุมสีพื้นหลัง")]
    public Image backgroundImage; 

    public float popSpeed = 15f;

    private bool isOpen = false;
    private bool isShowingOutputOnly = false; // เอาไว้เช็คว่าตอนนี้โชว์แค่ผลลัพธ์อยู่ไหม

    void Start()
    {
        if (terminalWindow != null) terminalWindow.localScale = Vector3.zero;
    }

    void Update()
    {
        // 1. ถ้าโชว์แค่ตัวหนังสืออยู่ และผู้เล่นกด Esc -> ให้ปิดหน้าต่างทั้งหมดและกลับเข้าเกม
        if (isOpen && isShowingOutputOnly && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTerminal();
            return;
        }

        // 2. ถ้าไม่ได้โชว์ตัวหนังสือลอยๆ อยู่ ให้ใช้ปุ่ม F เปิด/ปิดได้ตามปกติ
        if (Input.GetKeyDown(KeyCode.F) && !isShowingOutputOnly)
        {
            if (isOpen) CloseTerminal();
            else OpenTerminal();
        }

        // แอนิเมชันยืดหด
        if (terminalWindow != null)
        {
            Vector3 targetScale = isOpen ? Vector3.one : Vector3.zero;
            terminalWindow.localScale = Vector3.Lerp(terminalWindow.localScale, targetScale, Time.unscaledDeltaTime * popSpeed);
        }
    }

    public void OpenTerminal()
    {
        isOpen = true;
        isShowingOutputOnly = false;

        // เปิดสีพื้นหลัง และ ช่องพิมพ์ กลับมา
        if (backgroundImage != null) backgroundImage.enabled = true;
        if (inputField != null) inputField.gameObject.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (inputField != null)
        {
            inputField.Select();
            inputField.ActivateInputField();
        }
    }

    public void CloseTerminal()
    {
        isOpen = false;
        isShowingOutputOnly = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ฟังก์ชันใหม่: เรียกใช้ตอนกดส่งโค้ด เพื่อลบภาพพื้นหลังทิ้ง
    public void ShowOnlyOutput()
    {
        isShowingOutputOnly = true;
        
        // สั่งปิด "สีพื้นหลัง" และ "ช่องพิมพ์" (เหลือแค่หน้าต่างใสๆ กับ Output)
        if (backgroundImage != null) backgroundImage.enabled = false;
        if (inputField != null) inputField.gameObject.SetActive(false);
    }
}