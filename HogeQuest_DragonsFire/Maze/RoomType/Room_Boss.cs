using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    /// <summary>
    /// ボスがいる部屋のクラス  14.
    /// </summary>
    internal class Room_Boss : RoomBase
    {
        public Room_Boss()
        {
            _groupRooms[GroupID.Boss].Add(this);
        }

        protected override void PlayEvent(PlayerData playerData)
        {
            if (IsEnterd == false)
            {
                ShowMazeMessage("とてつもない威圧感が辺りに漂っている...");
                ShowMazeMessage("あなたが来るのを待っていたかのように、暗闇の中で赤い3つの目が光る。");
                ShowMazeMessage("黒龍が現れた！！！");
                ShowMazeMessage("黒龍の攻撃！");
                ShowMazeMessage("間一髪のところで回避した！");
                ShowMazeMessage("あなたの攻撃！");
                ShowMazeMessage("硬い鱗に阻まれ、一切のダメージを与えられない...");
                ShowMazeMessage("黒龍の「黒炎」攻撃！");
                ShowMazeMessage("辺り一面が炎に包まれ、避けることができない...");
                ShowMazeMessage("99ダメージを受けた...");

                if (playerData.HasDragonSword == true)
                {
                    ShowMazeMessage("竜の剣が光り輝いている...！");
                    ShowMazeMessage("広がっていた黒炎が、剣にすべて吸い込まれてゆく！");
                    ShowMazeMessage("体に力がみなぎってきた。");
                    ShowMazeMessage("あなたの攻撃！");
                    ShowMazeMessage("黒炎をまとった竜の剣が、黒龍の体を両断した！");
                    ShowMazeMessage("黒龍を倒した！");
                    IsEnterd = true;
                }
                else
                {
                    playerData.Life -= 99;
                }
            }
        }
    }
}
