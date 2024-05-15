using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    /// <summary>
    /// 竜の剣がある部屋のクラス  12.
    /// </summary>
    internal class Room_Tresure_DragonSword : RoomBase
    {
        public Room_Tresure_DragonSword()
        { 
            IsHidden = true;
            _groupRooms[GroupID.GoddessAwakeDragonSword].Add(this);
        }

        protected override void PlayEvent(PlayerData playerData)
        {
            if (IsEnterd == false)
            {
                ShowMazeMessage("神々しい光が辺りを包み込んでいる。");
                ShowMazeMessage("光が集まり、１本の剣の形を成してゆく。");
                ShowMazeMessage("秘宝「竜の剣」を手に入れた！！！");

                playerData.HasDragonSword = true;
                IsEnterd = true;
            }
            else
            {
                ShowMazeMessage("この部屋にはもう何もないようだ。", false);
            }
        }
    }
}
