using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public struct EquipmentPageBinding
{
    public EquipmentType equipmentType;
    public EquipmentPage page;
}

public class EquipmentSelector : MonoBehaviour
{
    public EquipmentList equipments;
    public List<EquipmentPageBinding> pages;

#pragma warning disable 0649
    [SerializeField] private EquipmentVisual currentEquipment;
#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        currentEquipment.ShouldCatchRaycast = false; ;
        PopulatePages();
    }

    private void Update()
    {
        currentEquipment.transform.position = Input.mousePosition;
    }

    private void PopulatePages()
    {
        foreach (EquipmentLayout equip in equipments.equipments)
        {
            EquipmentPageBinding binding = pages.First(x => x.equipmentType == equip.type);
            if (binding.Equals(default(EquipmentPageBinding)))
            {
                Debug.LogError("No equipment page found for type " + equip.type);
            }
            else
            {
                EquipmentPage page = binding.page;
                bool saveActive = page.gameObject.activeInHierarchy;
                page.gameObject.SetActive(true);
                page.AddEquipment(equip);
                page.gameObject.SetActive(saveActive);
            }
        }
        EquipmentButton.OnEquipmentSelected += OnEquipmentSelected;
    }

    private void OnEquipmentSelected(EquipmentLayout equip)
    {
        currentEquipment.Layout = equip;
    }
}
