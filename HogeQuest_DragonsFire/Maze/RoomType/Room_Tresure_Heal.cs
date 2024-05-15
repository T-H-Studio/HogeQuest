using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    /// <summary>
    /// 回復薬がある部屋のクラス  10.
    /// </summary>
    internal class Room_Tresure_Heal : RoomBase
    {
        protected override void PlayEvent(PlayerData playerData)
        {
            if (IsEnterd == false)//初めて訪れる時に実行
            {
                if (playerData.Life >= playerData.MaxLife)
                {
                    ShowMazeMessage("宝箱を見つけた。中には回復薬があるが、今は体調が万全だ。");
                }
                else
                {
                    ShowMazeMessage("宝箱を見つけた。中にあった回復薬を使って、体力が1回復した！");
                    playerData.Life++;

                    IsEnterd = true;//回復薬を使用した場合だけ訪れ済みにする
                }
            }
            else
            {
                ShowMazeMessage("この部屋にはもう何もないようだ。", false);
            }
        }
    }
}
