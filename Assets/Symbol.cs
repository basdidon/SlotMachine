using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol : MonoBehaviour
{
    [SerializeField] SymbolData symbolData;
    public SymbolData SymbolData {
        get => symbolData;
        set
        {
            symbolData = value;
            SpriteRenderer.sprite = symbolData.Sprite;
        }
    }
    public SpriteRenderer SpriteRenderer { get; set; }
    public ReelControl ReelControl { get; set; }

    private void Awake()
    {
        SpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    }

    public void MoveSymbolDown()
    {
        StartCoroutine(MoveSymbolDownRoutine());
    }

    IEnumerator MoveSymbolDownRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.down * ReelControl.offset;
        float timeElapsed = 0f;
        float duration = ReelControl.offset / ReelControl.spinSpeed;

        while(timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / duration);
            yield return null;
            timeElapsed += Time.deltaTime;
        }

        transform.position = endPos;
    }
}
