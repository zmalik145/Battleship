using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public int gridSizeX, gridSizeY;
    public float cellSpacing;
    public Vector2 playerCellPos, enemyCellPos;

    void Start()
    {
        CreateGrid(playerCellPos, "PlayerCell", GameManager.Instance.playerCells);
        CreateGrid(enemyCellPos, "EnemyCell", GameManager.Instance.enemyCells);
        GameManager.SelectBuilding?.Invoke();
    }

    /* The GridManager class is responsible for creating the grid layout for the game. It
        instantiates cells (game objects) based on the specified dimensions (gridSizeX and
        gridSizeY) and spacing (cellSpacing). It creates two grids, one for the player and one for
        the enemy, by calling the CreateGrid method twice with different starting positions and
        cell lists.
        Inside the CreateGrid method, it iterates over the grid dimensions and instantiates cells
        at calculated positions. Each cell is given a unique name and tagged based on whether
        it belongs to the player or the enemy. The instantiated cells are added to the respective
        cell lists (playerCells or enemyCells) for further use. Finally, it invokes the SelectBuilding
        event, triggering the selection of buildings for both players.
    */
    void CreateGrid(Vector2 startingPos, string cellNamePrefix, List<BattleBuilding> cellList)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 cellPosition = startingPos + new Vector2(x * cellSpacing, y * cellSpacing);
                GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                BattleBuilding building = cell.GetComponent<BattleBuilding>();
                cellList.Add(building);
                cell.name = $"{cellNamePrefix} ({x},{y})";
                cell.tag = cellNamePrefix;
            }
        }
    }
}
