using HogeQuest_DragonsFire.Maze.RoomType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze
{
    /// <summary>
    /// 迷宮全体の情報クラス 6.
    /// </summary>
    internal class MazeData
    {
        /// <summary>
        /// 移動方角のenum
        /// </summary>
        public enum Direction
        {
            North = 0,
            East,
            West,
            South,
        }

        private WallManager _wallManager;
        private RoomManager _roomManager;

        public int NowPointX { get; private set; }//迷宮の左上端が0
        public int NowPointY { get; private set; }//迷宮の左上端が0

        public MazeData(WallManager wallManager, RoomManager roomManager)
        {
            _wallManager = wallManager;
            _roomManager = roomManager;

            //初期位置を設定
            _roomManager.GetStartPoint(out int x, out int y);
            NowPointX = x;
            NowPointY = y;
        }

        /// <summary>
        /// 別の部屋に移動し、
        /// 現在プレイヤーがいる事になる部屋のイベント実行メソッドを持ったI_PlayerAccess型を返す
        /// </summary>
        public I_PlayerAccess MoveRoom(Direction direction)
        {
            //壁にぶつかっていなかったら現在座標を変更
            if (_wallManager.IsExistWall(NowPointX, NowPointY, direction) == false)
            {
                //北か南方向ならY座標を調整
                if (direction == Direction.North || direction == Direction.South) 
                {
                    NowPointY = (direction == Direction.North) ? --NowPointY : ++NowPointY;
                }
                else//西か東方向ならX座標を調整
                {
                    NowPointX = (direction == Direction.West) ? --NowPointX : ++NowPointX;
                }
            }

            return _roomManager.GetRoomEvent(NowPointX, NowPointY);
        }
    }
}
