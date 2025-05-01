using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    /// <summary>
    /// 何もない部屋のクラス
    /// </summary>
    internal class Room_Empty : RoomBase
    {
        protected override void PlayRoomGimmick(PlayerData playerData)
        {
            ShowMazeMessage("この部屋には何もないようだ。", false);
        }
    }
}
