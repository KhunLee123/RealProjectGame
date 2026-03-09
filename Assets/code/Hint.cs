using UnityEngine;
using System.Collections; 

public class Hint : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject hint1;
    
    [Header("Animation Settings")]
    public float duration = 0.3f; 
    
    private CanvasGroup canvasGroup; 
    private Coroutine currentEffect; 

    void Start()
    {
        if (hint1 != null) 
        {
            canvasGroup = hint1.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = hint1.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0;
            hint1.transform.localScale = Vector3.zero;
            hint1.SetActive(false); 
        }
    }

    public void ShowHint1()
    {
        if (hint1 == null) return;

        hint1.SetActive(true); 
        if (currentEffect != null) StopCoroutine(currentEffect); 
        currentEffect = StartCoroutine(AnimateHint(1, Vector3.one)); 
    }

    public void EndHint1()
    {
        if (hint1 == null) return;

        if (currentEffect != null) StopCoroutine(currentEffect);
        currentEffect = StartCoroutine(AnimateHint(0, Vector3.zero, true)); 
    }

    IEnumerator AnimateHint(float targetAlpha, Vector3 targetScale, bool hideAtEnd = false)
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        Vector3 startScale = hint1.transform.localScale;

        while (time < duration)
        {
            // 🚨 แก้ตรงนี้ครับ! ใช้ Time.unscaledDeltaTime แทน
            time += Time.unscaledDeltaTime; 
            
            float t = time / duration;
            float smoothT = Mathf.SmoothStep(0, 1, t);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, smoothT);
            hint1.transform.localScale = Vector3.Lerp(startScale, targetScale, smoothT);
            
            yield return null; 
        }

        canvasGroup.alpha = targetAlpha;
        hint1.transform.localScale = targetScale;

        if (hideAtEnd) hint1.SetActive(false);
        
        currentEffect = null;
    }
}