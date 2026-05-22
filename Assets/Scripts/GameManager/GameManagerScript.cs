using UnityEngine;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour
{
    private StateMachine stateMachine;

    [Header("Map Configuration")]
    public GameObject mapContainer;
    public List<GameObject> tilePrefabs;
    public int xSize = 15;
    public int ySize = 15;
    public float offsetX = 0.865f;
    public float offsetZ = 0.5f;

    public BoardGenerationState BoardGenState { get; private set; }
    public DiceRollState DiceRollState { get; private set; }


    private void Awake()
    {
        stateMachine = new StateMachine();

        BoardGenState = new BoardGenerationState(this, mapContainer, tilePrefabs, xSize, ySize, offsetX, offsetZ);
        DiceRollState = new DiceRollState(this);

    }

    private void Start()
    {
        stateMachine.Initialize(BoardGenState);
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public void ChangeState(BaseState newState)
    {
        stateMachine.ChangeState(newState);
    }
}