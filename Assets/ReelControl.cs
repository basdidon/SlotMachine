using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelControl : MonoBehaviour
{
    [field: SerializeField] List<SymbolData> SymbolDataset { get; set; }
    [field:SerializeField] public List<Symbol> SymbolList { get; set; }
    [SerializeField] public float offset = 2.5f;
    [SerializeField] public float spinSpeed;
    public bool IsSpining { get; set; }

    private void Awake()
    {
        SymbolList = new();
        for (int i = 0; i < SymbolDataset.Count; i++)
        {
            var clone = new GameObject($"{SymbolDataset[i].name}");
            clone.transform.parent = transform;
            clone.transform.localPosition = offset * (i - 2) * Vector3.up;
            Symbol symbol = clone.AddComponent<Symbol>();
            symbol.SymbolData = SymbolDataset[i];
            symbol.ReelControl = this;
            SymbolList.Add(symbol);
        }
    }

    public System.Action OnMoveDone;

    public void Move()
    {
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        IsSpining = true;
        while (IsSpining)
        {
            var temp = SymbolList[0];
            SymbolList.Remove(temp);
            SymbolList.Add(temp);
            foreach (var symbol in SymbolList)
            {
                symbol.MoveSymbolDown();
            }
            var duration = offset / spinSpeed;
            yield return ResetSymbol(temp, duration);
        }
    }

    public void Stop()
    {
        IsSpining = false;
    }

    IEnumerator ResetSymbol(Symbol symbol,float duration)
    {
        yield return new WaitForSeconds(duration);
        symbol.transform.localPosition = offset * (SymbolList.Count - 3) * Vector3.up;
    }
}