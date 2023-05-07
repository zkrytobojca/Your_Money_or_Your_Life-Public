using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTarget : MonoBehaviour, IDamageable
{
    public void TakeDamage(int amount, RaycastHit hit)
    {
        GameStateManager.instance.SetGameState(GameStateManager.GameState.InProgress);
    }
}
