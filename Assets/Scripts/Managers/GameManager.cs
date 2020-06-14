using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Unknown, Building, Fighting }

public class GameManager : MonoBehaviour
{
    private GameState _currentState;
    public static GameManager Instance { get; private set; }
    public GameState CurrentState
    {
        get { return _currentState; }
        set
        {
            if(_currentState != value)
            {
                _currentState = value;
                OnGameStateChanged?.Invoke(_currentState);
            }
        }
    }

    public delegate void GameStateChanged(GameState state);
    public GameStateChanged OnGameStateChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CurrentState = GameState.Building;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleStates()
    {
        if(CurrentState == GameState.Building)
        {
            CurrentState = GameState.Fighting;
        } else
        {
            CurrentState = GameState.Building;
        }
    }
}
