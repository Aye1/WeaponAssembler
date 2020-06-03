using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EquipmentButton : MonoBehaviour
{
    public delegate void EquipmentSelected(EquipmentLayout equip);
    public static EquipmentSelected OnEquipmentSelected;

    private EquipmentLayout _equipment;
    public EquipmentLayout Equipment
    {
        get { return _equipment; }
        set
        {
            if(_equipment != value)
            {
                _equipment = value;
                GetComponentInChildren<Text>().text = _equipment.name;
            }
        }
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SendEquipment);
    }

    private void SendEquipment()
    {
        OnEquipmentSelected?.Invoke(Equipment);
    }
}
