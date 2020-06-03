using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentLayout", menuName = "ScriptableObjects/EquipmentList", order = 1)]
public class EquipmentList : ScriptableObject
{
    public List<EquipmentLayout> equipments;
}
