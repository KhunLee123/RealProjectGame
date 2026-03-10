using UnityEngine;

public class Endfirsthint : MonoBehaviour
{
    public Hint hintScript; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Torch Sensor");
            if (hintScript != null)
            {
                hintScript.EndHint1();
            }
        }
    }
}