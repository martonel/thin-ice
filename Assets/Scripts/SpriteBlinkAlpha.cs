using System.Collections;
using UnityEngine;

public class SpriteBlinkAlpha : MonoBehaviour
{
    [Header("Sprites")]
    public SpriteRenderer spriteA;
    public SpriteRenderer spriteB;

    [Header("Blink settings")]
    public float blinkDuration = 4f;
    public float alphaDelta = 0.2f;   // -20%
    public float blinkSpeed = 8f;     // villogás gyorsasága

    private Coroutine blinkRoutine;

    public void StartBlink()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine()
    {
        float timer = 0f;

        float originalA = spriteA.color.a;
        float originalB = spriteB.color.a;

        float minA = Mathf.Clamp01(originalA - alphaDelta);
        float minB = Mathf.Clamp01(originalB - alphaDelta);

        while (timer < blinkDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);

            SetAlpha(spriteA, Mathf.Lerp(minA, originalA, t));
            SetAlpha(spriteB, Mathf.Lerp(minB, originalB, t));

            yield return null;
        }

        // visszaállítás pontosan az eredetire
        SetAlpha(spriteA, originalA);
        SetAlpha(spriteB, originalB);

        PlayerHearth playerHearth = GetComponentInParent<PlayerHearth>();
        if (playerHearth != null)
        {
            playerHearth.SetDamagable();
        }
        blinkRoutine = null;

    }

    void SetAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}
