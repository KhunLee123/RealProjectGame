using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CppCompiler : MonoBehaviour
{
    public TextMeshProUGUI outputText;
    
    // 🚨 1. เพิ่มตัวแปรสำหรับรับ "บล็อกต้นแบบ" ที่เราจะเสก
    [Header("ลาก Prefab บล็อก (Cube) มาใส่ช่องนี้")]
    public GameObject blockPrefab; 

    // 🚨 เพิ่มบรรทัดนี้: เอาไว้กำหนดจุดเริ่มต้นสะพาน
    [Header("ลากจุดเริ่มต้นสะพาน (Empty Object) มาใส่")]
    public Transform bridgeStartPoint;

    private string apiUrl = "https://wandbox.org/api/compile.json";

    public void RunCppCode(string playerCode)
    {
        outputText.text += "\n> Compiling and running...\n";
        StartCoroutine(SendCodeToServer(playerCode));
    }

    IEnumerator SendCodeToServer(string codeToRun)
    {
        WandboxRequest requestData = new WandboxRequest { compiler = "gcc-head", code = codeToRun, save = false };
        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                outputText.text += "<color=red>> [Network Error] " + request.error + "</color>\n";
            }
            else
            {
                string responseJson = request.downloadHandler.text;
                WandboxResponse response = JsonUtility.FromJson<WandboxResponse>(responseJson);

                if (response.status == "0") 
                {
                    // พิมพ์ผลลัพธ์ลงจอมอนิเตอร์ตามปกติ
                    string output = response.program_output;
                    outputText.text += "<color=green>" + output + "</color>\n";

                    // =========================================================
                    // 🚨 2. ระบบแปลคำสั่งเสกของ (Command Parser)
                    // =========================================================
                    // หั่นข้อความที่ได้มาทีละบรรทัด
                    string[] lines = output.Split('\n'); 
                    foreach (string line in lines)
                    {
                        // ถ้าบรรทัดไหนขึ้นต้นด้วยคำสั่งลับของเรา
                        if (line.StartsWith("SPAWN_BLOCK"))
                        {
                            // แยกคำด้วยช่องว่าง (เช่น "SPAWN_BLOCK", "1", "0", "5")
                            string[] parts = line.Split(' ');
                            if (parts.Length == 4 && blockPrefab != null)
                            {
                                // แปลงตัวอักษรให้เป็นตัวเลขทศนิยมพิกัด (float)
                                if (float.TryParse(parts[1], out float x) &&
                                    float.TryParse(parts[2], out float y) &&
                                    float.TryParse(parts[3], out float z))
                                {
                                    // 🚨 เสกบล็อกลงในโลก Unity!
                                    // 🚨 แก้วิธีคำนวณพิกัดเสกของใหม่ ให้เริ่มจากจุดที่เรากำหนด
                                if (bridgeStartPoint != null)
                                {
                                    // คำนวณให้สะพานยื่นไปด้านหน้า (forward) ของจุดเริ่มต้นเสมอ!
                                    Vector3 finalPos = bridgeStartPoint.position 
                                                       + (bridgeStartPoint.right * x) 
                                                       + (bridgeStartPoint.up * y) 
                                                       + (bridgeStartPoint.forward * z);

                                    // เสกบล็อกและหมุนให้ทิศทางตรงกับจุดเริ่มต้น
                                    Instantiate(blockPrefab, finalPos, bridgeStartPoint.rotation);
                                }
                                else
                                {
                                    // ถ้าลืมใส่จุดเริ่มต้น ก็ให้โผล่ที่ 0,0,0 เหมือนเดิม
                                    Vector3 spawnPos = new Vector3(x, y, z);
                                    Instantiate(blockPrefab, spawnPos, Quaternion.identity);
                                }
                                }
                            }
                        }
                    }
                    // =========================================================
                }
                else 
                {
                    outputText.text += "<color=red>[Compile Error]\n" + response.compiler_error + "</color>\n";
                }
            }
        }
    }
}

[System.Serializable] public class WandboxRequest { public string compiler; public string code; public bool save; }
[System.Serializable] public class WandboxResponse { public string status; public string program_output; public string program_error; public string compiler_error; }