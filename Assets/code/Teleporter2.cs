using UnityEngine;

public class Teleporter2 : MonoBehaviour
{
    [Header("Teloport")]
    public Transform targetLocation;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
                other.transform.position = targetLocation.position;
                other.transform.rotation = targetLocation.rotation;
                cc.enabled = true; 
            }
        }
    }
}