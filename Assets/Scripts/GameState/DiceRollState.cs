using UnityEngine;

public class DiceRollState : BaseState
{
    private GameManagerScript manager;

    public DiceRollState(GameManagerScript gameManager)
    {
        manager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("lancement de dés");
    }

    public void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int dice1 = Random.Range(1, 7);
            int dice2 = Random.Range(1, 7);
            int total = dice1 + dice2;
            Debug.Log($"Résultat du lancer : {total}");

          
        }
    }

    public void Exit() { }
}