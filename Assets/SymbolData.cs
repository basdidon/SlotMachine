using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Symbol")]
public class SymbolData : ScriptableObject
{
    [field:SerializeField] public Sprite Sprite { get; set; }
}
