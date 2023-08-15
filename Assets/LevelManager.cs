using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int playerCash = 0;
    public int PlayerCash { get => playerCash; set => playerCash = value; }
    public bool WasSpin { get; set; }
    [field: SerializeField] ReelControl Reel_1 { get; set; }
    [field: SerializeField] ReelControl Reel_2 { get; set; }
    [field: SerializeField] ReelControl Reel_3 { get; set; }

    [field:SerializeField] SymbolData Globe { get;set; }
    [field:SerializeField] SymbolData Bar { get;set; }
    [field:SerializeField] SymbolData Plum { get;set; }
    [field:SerializeField] SymbolData Bell { get;set; }
    [field:SerializeField] SymbolData Orange { get;set; }
    [field:SerializeField] SymbolData Cherry { get;set; }

    [Header("InputRef")]
    public InputActionReference spinInputRef;
    public InputActionReference stopInputRef;

    public InputAction SpinInputAction => spinInputRef.action;
    public InputAction StopInputAction => stopInputRef.action;

    private void OnEnable()
    {
        SpinInputAction.Enable();
        StopInputAction.Enable();
    }

    private void OnDisable()
    {
        SpinInputAction.Disable();
        StopInputAction.Disable();
    }

    private void Awake()
    {
        SpinInputAction.performed += _ =>
        {
            if (WasSpin)
                return;

            Reel_1.Move();
            Reel_2.Move();
            Reel_3.Move();
            WasSpin = true;
        };

        StopInputAction.performed += _ =>
        {
            if (WasSpin)
            {
                if (Reel_1.IsSpining)
                {
                    Reel_1.Stop();
                }
                else if (Reel_2.IsSpining)
                {
                    Reel_2.Stop();
                }
                else if (Reel_3.IsSpining)
                {
                    Reel_3.Stop();
                    CheckAllPayline();
                    WasSpin = false;
                }
            }
            else
            {
                Debug.Log("Press S to start");
            }
        };
    }

    public void CheckAllPayline()
    {
        PaylineCheck(1); // bottom
        PaylineCheck(2); // middle
        PaylineCheck(3); // top
    }

    void PaylineCheck(int idx)
    {
        var value_1 = Reel_1.SymbolList[idx].SymbolData;
        var value_2 = Reel_2.SymbolList[idx].SymbolData;
        var value_3 = Reel_3.SymbolList[idx].SymbolData;

        int pay = 0;

        if(value_1.name == Globe.name && value_2.name == Globe.name && value_3.name == Globe.name)
        {
            pay += 500;
        }
        else if(value_1.name == Bar.name && value_2.name == Bar.name && value_3.name == Bar.name)
        {
            pay += 100;
        }
        else if(value_1.name == Plum.name && value_2.name == Plum.name && value_3.name == Plum.name)
        {
            pay += 50;
        }else if(value_1.name == Bell.name && value_2.name == Bell.name && value_3.name == Bell.name)
        {
            pay += 20;
        }else if(value_1.name == Orange.name && value_2.name == Orange.name && value_3.name == Orange.name)
        {
            pay += 15;
        }
        else
        {
            int countCherry = 0;
            countCherry += value_1.name == Cherry.name ? 1 : 0;
            countCherry += value_2.name == Cherry.name ? 1 : 0;
            countCherry += value_3.name == Cherry.name ? 1 : 0;

            if(countCherry == 3)
            {
                pay += 10;
            }
            else if(countCherry == 2)
            {
                pay += 5;
            }
            else if(countCherry == 1)
            {
                pay += 2;
            }
        }

        Debug.Log($"{ value_1.name},{value_2.name},{value_3.name} : {pay}");
        PlayerCash += pay;
    }
}
