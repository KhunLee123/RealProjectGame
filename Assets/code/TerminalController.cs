using UnityEngine;
using TMPro;

public class TerminalController : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_InputField inputField;
    
    [Header("System References")]
    public CppCompiler compiler;
    public ToggleCanvas toggleCanvas; 

    void Update()
    {
        if (inputField != null && inputField.isFocused)
        {
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

        // 🚨 เคลียร์ข้อความเก่าทิ้ง: สังเกตว่าใช้เครื่องหมาย = แทน += แล้วครับ
        if (compiler.outputText != null)
        {
            compiler.outputText.text = "<color=yellow>user@terminal:~$</color> \n" + playerCode + "\n";
        }

        string boilerplateCode = 
            "#include <iostream>\n" +
            "using namespace std;\n" +
            "int _bridgeZ = 0;\n" + 
            "void spawn() {\n" +
            "    cout << \"SPAWN_BLOCK 0 0 \" << _bridgeZ << \"\\n\";\n" +
            "    _bridgeZ += 2;\n" +
            "}\n" +
            "int main() {\n" +
            playerCode + "\n" + 
            "return 0;\n" +
            "}";

        compiler.RunCppCode(boilerplateCode);
        inputField.text = "";

        if (toggleCanvas != null)
        {
            toggleCanvas.ShowOutputState();
        }
    }
}