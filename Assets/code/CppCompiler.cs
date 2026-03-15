using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CppCompiler : MonoBehaviour
{
    [Header("Output UI")]
    public TextMeshProUGUI outputText;
    
    [Header("Spawn System")]
    public GameObject blockPrefab; 
    public Transform bridgeStartPoint; 

    [Header("Room Door 1")]
    public GameObject targetDoor1;

    [Header("Room Door 2")]
    public GameObject targetDoor2;

    [Header("Room Door 4")]
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;

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
                    string output = response.program_output;
                    outputText.text += "<color=green>" + output + "</color>\n";

                    string[] lines = output.Split('\n'); 
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("SPAWN_BLOCK"))
                        {
                            string[] parts = line.Split(' ');
                            if (parts.Length == 4 && blockPrefab != null)
                            {
                                if (float.TryParse(parts[1], out float x) && float.TryParse(parts[2], out float y) && float.TryParse(parts[3], out float z))
                                {
                                    if (bridgeStartPoint != null)
                                    {
                                        Vector3 finalPos = bridgeStartPoint.position + (bridgeStartPoint.right * x) + (bridgeStartPoint.up * y) + (bridgeStartPoint.forward * z);
                                        Instantiate(blockPrefab, finalPos, bridgeStartPoint.rotation);
                                    }
                                    else
                                    {
                                        Vector3 spawnPos = new Vector3(x, y, z);
                                        Instantiate(blockPrefab, spawnPos, Quaternion.identity);
                                    }
                                }
                            }
                        }
                        else if (line.Trim() == "OPEN_DOOR_CMD")
                        {
                            if (targetDoor1 != null)
                            {
                                targetDoor1.SetActive(false); 
                                outputText.text += "\n<color=#00FFFF>> Door Unlocked!</color>\n";
                            }
                        }
                        else if (line.StartsWith("UNLOCK"))
                        {
                            string[] parts = line.Split(' ');
                            if (parts.Length == 4 && targetDoor2 != null)
                            {
                                if (parts[1] == "2" && parts[2] == "7" && parts[3] == "1")
                                {
                                    targetDoor2.SetActive(false); 
                                    outputText.text += "\n<color=#00FFFF>> Door Unlocked!</color>\n";
                                }
                            }
                        }
                        else if (line.Trim() == "OPEN_DOOR_CMD 1")
                        {
                            if (door1 != null) door1.SetActive(false); 
                            outputText.text += "\n<color=#00FFFF>> Door Unlocked!</color>\n";
                        }
                         else if (line.Trim() == "OPEN_DOOR_CMD 2")
                        {
                            if (door2 != null) door2.SetActive(false); 
                            outputText.text += "\n<color=#00FFFF>> Door Unlocked!</color>\n";
                        }
                        else if (line.Trim() == "OPEN_DOOR_CMD 3")
                        {
                            if (door3 != null) door3.SetActive(false); 
                            outputText.text += "\n<color=#00FFFF>> Door Unlocked!</color>\n";
                        }               
                    }
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