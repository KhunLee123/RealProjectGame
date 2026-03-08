using UnityEngine;
using TMPro;

public class TerminalController : MonoBehaviour
{
    public TMP_InputField inputField;
    public CppCompiler compiler;
    
    // 🚨 จุดที่ Error: ต้องมีบรรทัดนี้เพื่อประกาศชื่อ toggleCanvas
    public ToggleCanvas toggleCanvas; 

    void Update()
    {
        if (inputField != null && inputField.isFocused)
        {
            // กด Shift + Enter เพื่อส่งโค้ด
            if (Input.GetKeyDown(KeyCode.Return) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                OnSubmitCode();
            }
        }
    }

    public void OnSubmitCode()
    {
        string playerCode = inputField.text;
        if (string.IsNullOrWhiteSpace(playerCode)) return;

        // 1. โชว์แค่สิ่งที่ผู้เล่นพิมพ์ลงบนจอ
        if (compiler.outputText != null)
        {
            compiler.outputText.text += "\n<color=yellow>user@terminal:~$</color> \n" + playerCode + "\n";
        }

        // =========================================================
        // 🚨 2. อัปเดต Boilerplate: สร้างฟังก์ชัน spawn() ซ่อนไว้ให้ผู้เล่นใช้
        // =========================================================
        string boilerplateCode = 
            "#include <iostream>\n" +
            "using namespace std;\n" +
            
            // แอบสร้างตัวแปรจำพิกัดแกน Z ไว้ (เริ่มที่ 0)
            "int _bridgeZ = 0;\n" + 
            
            // สร้างฟังก์ชัน spawn() ให้ผู้เล่นเรียกใช้
            "void spawn() {\n" +
            "    cout << \"SPAWN_BLOCK 0 0 \" << _bridgeZ << \"\\n\";\n" +
            "    _bridgeZ += 2;\n" + // พอเสกเสร็จ ให้ขยับพิกัด Z ไปข้างหน้า 2 หน่วย รอไว้สำหรับบล็อกต่อไป!
            "}\n" +
            
            "int main() {\n" +
            playerCode + "\n" + // <--- โค้ดของผู้เล่นจะมาอยู่ตรงนี้
            "return 0;\n" +
            "}";
        // =========================================================

        // 3. ส่งโค้ดฉบับเต็มขึ้น Cloud
        compiler.RunCppCode(boilerplateCode);

        // 4. ล้างช่องพิมพ์ และซ่อนหน้าต่าง
        inputField.text = "";
        if (toggleCanvas != null)
        {
            toggleCanvas.ShowOnlyOutput();
        }
    }
}