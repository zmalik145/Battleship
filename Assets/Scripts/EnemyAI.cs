using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private int damagePerAttack = 3;

    /*
        The script handles clicks during the enemy's turn. It
        raycasts to check if other player building is hit. If it hits a building, it deals damage, plays
        a hit sound, and shakes the cell. If it misses, it plays a miss sound and shows a "missed"
        effect. Finally, it triggers a turn switch.
     */
    public void AttackBuilding()
    {
        if (GameManager.Instance.isPlayerTurn || GameManager.Instance.isGameOver)
            return;

        int randomIndex = Random.Range(0, GameManager.Instance.playerCells.Count);
        Vector3 randomCellPosition = GameManager.Instance.playerCells[randomIndex].transform.position;

        RaycastHit2D hit = Physics2D.Raycast(Vector2.zero, randomCellPosition, Mathf.Infinity, buildingLayer);

        if (hit.collider)
        {
            if (hit.collider.TryGetComponent<BattleBuilding>(out var building))
            {
                if (hit.collider.gameObject.CompareTag("PlayerCell"))
                {
                    if (building.HasBuilding && !building.IsDead)
                    {
                        building.TakeDamage(damagePerAttack);
                        GameManager.Instance.BuildingHit();
                        SoundManager.Instance.PlayHitTarget();
                    }
                    else
                    {
                        SoundManager.Instance.PlayMissFire();
                        if (!building.IsDead) building.HitMissed();
                    }
                    building.Shake();
                    GameManager.SwapTurn?.Invoke();
                }
            }
        }
    }
}
