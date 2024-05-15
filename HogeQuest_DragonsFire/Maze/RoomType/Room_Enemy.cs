using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    /// <summary>
    /// 雑魚敵がいる部屋のクラス  13.
    /// </summary>
    internal class Room_Enemy : RoomBase
    {
        public Room_Enemy() 
        {
            _groupRooms[GroupID.AllEnemy].Add(this);
        }

        protected override void PlayEvent(PlayerData playerData)
        {
            if (IsEnterd == false)
            {
                ShowMazeMessage("悪鬼が現れた！");
                ShowMazeMessage("1ダメージを受けたが、悪鬼を倒すことができた！");

                playerData.Life--;

                IsEnterd = true;
            }
            else
            {
                ShowMazeMessage("悪鬼の死骸が転がっている。", false);
            }
        }
    }
}
