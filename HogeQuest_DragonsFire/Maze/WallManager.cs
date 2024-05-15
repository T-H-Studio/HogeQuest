using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze
{
    /// <summary>
    /// 部屋の間に存在する壁を管理するクラス  3.
    /// </summary>
    internal class WallManager
    {
        bool[][] _rowWalls;//横向きの壁リスト　「－－－－－－」
        bool[][] _columnWalls;//縦向きの壁リスト　「｜｜｜｜｜｜｜」

        //引数に入れられた迷宮の大きさを元に、壁を生成
        public WallManager(bool[][] rowWalls, bool[][] columnWalls)
        {
            _rowWalls = rowWalls;
            _columnWalls = columnWalls;
        }

        public bool IsExistWall(int pointX, int pointY, MazeData.Direction direction)
        {
            //上下移動
            if (direction == MazeData.Direction.North || direction == MazeData.Direction.South)
            {
                if(direction == MazeData.Direction.South) { pointY++; }

                return _rowWalls[pointY][pointX];
            }
            //左右移動
            else
            {
                if(direction == MazeData.Direction.East) { pointX++; }

                return _columnWalls[pointY][pointX];
            }
        }
    }
}
