using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct TabPair
{
    public TabButton button;
    public GameObject page;
}

public class TabGroup : MonoBehaviour
{
    private List<TabButton> _buttons;
    private TabButton _selectedButton;

    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _hoverSprite;
    [SerializeField] private Sprite _selectedSprite;

    public List<TabPair> tabs;

    // Start is called before the first frame update
    void Start()
    {
        foreach(TabPair tab in tabs)
        {
            Register(tab.button);
        }
    }

    public void Register(TabButton button)
    {
        if(_buttons == null)
        {
            _buttons = new List<TabButton>();
        }
        if(!_buttons.Contains(button))
        {
            _buttons.Add(button);
            button.Clicked += OnClickedButton;
            button.PointerEnter += OnPointerEnterButton;
            button.PointerExit += OnPointerExitButton;
        }
    }

    public void Unregister(TabButton button)
    {
        if(_buttons.Contains(button))
        {
            _buttons.Remove(button);
            button.Clicked -= OnClickedButton;
            button.PointerEnter -= OnPointerEnterButton;
            button.PointerExit -= OnPointerExitButton;
        }
    }

    private void ResetButtons()
    {
        foreach(TabButton button in _buttons)
        {
            if(button != _selectedButton)
            {
                button.background.sprite = _idleSprite;
            }
        }
    }

    private void DisplaySelectedPanel()
    {
        foreach(TabPair tab in tabs)
        {
            tab.page.SetActive(tab.button == _selectedButton);
        }
    }

    private void OnClickedButton(TabButton button)
    {
        _selectedButton = button;
        ResetButtons();
        button.background.sprite = _selectedSprite;
        DisplaySelectedPanel();
    }

    private void OnPointerEnterButton(TabButton button)
    {
        ResetButtons();
        if (button != _selectedButton)
        {
            button.background.sprite = _hoverSprite;
        }
    }

    private void OnPointerExitButton(TabButton button)
    {
        ResetButtons();
    }

}
