using System;
using System.Collections;
using UnityEngine;

public class FlashDamageEffect : MonoBehaviour
{
    public Color flashColor = Color.red;
    public static float TimeToReset = 0.1f;
    private Color originalColor;
    private SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        originalColor = renderer.color;d
    }

    public void CreateFlashEffect()
    {
        StopCoroutine("ResetColor");
        renderer.color = flashColor;
        StartCoroutine("ResetColor");
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(TimeToReset);
        renderer.color = originalColor;
    }
}