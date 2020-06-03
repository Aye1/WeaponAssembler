using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPage : MonoBehaviour
{
    private List<EquipmentLayout> _equipments;
    private EquipmentButton _templateButton;

    private void Awake()
    {
        _equipments = new List<EquipmentLayout>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _templateButton = Resources.Load<EquipmentButton>("Prefabs/Equipment Button");

    }

    public void AddEquipment(EquipmentLayout equip)
    {
        if(!_equipments.Contains(equip))
        {
            _equipments.Add(equip);
            EquipmentButton newButton = Instantiate(_templateButton, Vector3.zero, Quaternion.identity, transform);
            newButton.Equipment = equip;
        }
    }
}
