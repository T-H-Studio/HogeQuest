using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    /// <summary>
    /// 開始地点となる部屋のクラス  16.
    /// </summary>
    internal class Room_StartPoint : RoomBase
    {
        protected override void PlayEvent(PlayerData playerData)
        {
            Console.WriteLine("ここは迷宮の入口だ");
        }
    }
}
