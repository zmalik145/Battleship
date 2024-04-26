using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private int damagePerAttack = 20;

    /*
        The script handles clicks during the player's turn. It
        raycasts to check if other player building is hit. If it hits a building, it deals damage, plays
        a hit sound, and shakes the cell. If it misses, it plays a miss sound and shows a "missed"
        effect. Finally, it triggers a turn switch.
     */
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameManager.Instance.isPlayerTurn || GameManager.Instance.isGameOver)
                return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, buildingLayer);

            if (hit.collider)
            {
                if (hit.collider.TryGetComponent<BattleBuilding>(out var building))
                {
                    if (building && hit.collider.CompareTag("EnemyCell"))
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
}
