namespace GridField
{
    public class Cell
    {
        public Cell(CellPosition position, CellInfo info)
        {
            _position = position;
            _info = info;
            _obj = null;
        }

        public CellObject GetCellObject() { return _obj; }
        public void SetCellObject(CellObject obj) { _obj = obj; }
        public CellInfo GetInfo() { return _info; }
        public CellPosition GetPosition() { return _position; }

        private CellObject _obj;
        private CellInfo _info;
        private CellPosition _position;
    }

    public class CellObject
    {

    }
    public class CellInfo
    {

    }

    public struct CellPosition
    {
        public CellPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;
    }
}
