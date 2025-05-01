using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    /// <summary>
    /// 特殊なギミックが起きる女神部屋のクラス
    /// </summary>
    internal class Room_Goddess : RoomBase
    {
        protected override void PlayRoomGimmick(PlayerData playerData)
        {
            //全ての雑魚敵が倒されていればdefeatAllをtrue
            bool defeatAll = true;
            foreach (var room in _groupRooms[GroupID.AllEnemy])
            {
                if (room.IsEnterd == false)//部屋に入っている≒倒しているなので、入ってない部屋があるなら倒せてない
                {
                    defeatAll = false;
                    break;
                }
            }

            if (defeatAll == true)
            {
                if (IsEnterd == false)
                {
                    ShowMazeMessage("壁に掛けてある女性の絵が話しかけてくる...");
                    ShowMazeMessage("「ああ...すべての悪鬼を退けてくださったのですね...」");
                    ShowMazeMessage("「私はこの迷宮の女神。」");
                    ShowMazeMessage("「黒龍の手先である悪鬼達によって、この絵の中に封じられていました。」");
                    ShowMazeMessage("「私が開放されたことで、迷宮の秘宝の封印もきっと解かれていることでしょう。」");
                    ShowMazeMessage("「探してください、迷宮の秘宝を。倒してください、かの黒龍を...」");

                    //剣が置かれたroomの隠ぺいを解く
                    foreach (var room in _groupRooms[GroupID.GoddessAwakeDragonSword])
                    {
                        room.IsHidden = false;
                    }

                    IsEnterd = true;
                }
                else
                {
                    if (playerData.HasDragonSword == true)
                    {
                        ShowMazeMessage("「まぁ、秘宝を手に入れることができたのですね。」");
                        ShowMazeMessage("「どうかこの世界を守ってください。勇者よ。」");
                    }
                    else
                    {
                        ShowMazeMessage("「秘宝の詳しい場所は私も覚えていないのです。ごめんなさい...」");
                    }
                }
            }
            else
            {
                Console.WriteLine("苦しそうな顔をした女性の絵が飾ってある。");
            }
        }
    }
}
