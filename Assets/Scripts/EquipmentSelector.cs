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
    public bool isDraggingEquipment = false;

#pragma warning disable 0649
    [SerializeField] private EquipmentVisual _currentEquipment;
#pragma warning restore 0649

    private DynamicGrid _grid;

    // Start is called before the first frame update
    void Start()
    {
        //currentEquipment.ShouldCatchRaycast = false; ;
        _grid = FindObjectOfType<DynamicGrid>();
        PopulatePages();
    }

    private void Update()
    {
        if (isDraggingEquipment)
        {
            _currentEquipment.transform.position = Input.mousePosition;
            _grid.ManageDrag(_currentEquipment);
            ManageClick();
        }
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
        _currentEquipment.gameObject.SetActive(true);
        _currentEquipment.Layout = equip;
        _currentEquipment.dragging = true;
        isDraggingEquipment = true;
    }

    public void DeselectEquipment()
    {
        isDraggingEquipment = false;
        _currentEquipment.dragging = false;
        _currentEquipment.gameObject.SetActive(false);
    }

    private void ManageClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            DeselectEquipment();
            _grid.PutEquipment(_currentEquipment);
        }
    }
}
