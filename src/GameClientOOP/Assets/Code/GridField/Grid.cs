using System.Collections.Generic;
using UnityEngine;

namespace GridField
{
    public class GridField
    {
        public GridField(float cellSize, int gridSideLength)
        {
            for (int x = 0; x < gridSideLength; x++)
            {
                for (int y = 0; y < gridSideLength; y++)
                {
                    var cell = new Cell(new CellPosition(x, y), new CellInfo());
                    _cells[x].Add(cell);
                }
            }
        }

        public void AddObjToGrid(CellObject obj, Cell cell)
        {
            if (cell.GetCellObject() != null)
            {
                Debug.Assert(false, "Cell is already occupied.");
                Debug.Break();
                return;
            }

            cell.SetCellObject(obj);   
        }

        public Cell FindCellByPosition(CellPosition position)
        {
            return _cells[position.x][position.y];
        }

        private float _cellSize;
        private List<List<Cell>> _cells;

        private GameObject _visual;
    }
}