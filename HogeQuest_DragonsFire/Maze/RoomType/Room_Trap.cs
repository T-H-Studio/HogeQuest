using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    /// <summary>
    /// トラップがある部屋のクラス  15.
    /// </summary>
    internal class Room_Trap : RoomBase
    {
        protected override void PlayEvent(PlayerData playerData)
        {
            if (IsEnterd == false)
            {
                ShowMazeMessage("しまった！トラップだ！！1ダメージを受けてしまった。");
                playerData.Life--;

                IsEnterd = true;
            }
            else
            {
                ShowMazeMessage("砕けた罠が散らばっている。", false);
            }
        }
    }
}
