using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public DynamicGrid grid;
    public EquipmentSelector equipSelector;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGameStateChanged(GameState state)
    {
        switch(state)
        {
            case GameState.Unknown:
                break;
            case GameState.Building:
                grid.SetVisibility(true);
                equipSelector.transform.DOScaleX(1.0f, 1.0f);
                break;
            case GameState.Fighting:
                grid.SetVisibility(false);
                equipSelector.transform.DOScaleX(0.0f, 1.0f);
                break;
        }
    }
}
