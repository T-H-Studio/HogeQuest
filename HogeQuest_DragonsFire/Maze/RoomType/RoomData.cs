using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    internal abstract class RoomData
    {
        /// <summary>
        /// _groupRoomsのキー値として使用される、部屋カテゴリー作成の為のenum
        /// </summary>
        public enum GroupID
        {
            /// <summary>
            /// 女神によって活性化する者たち
            /// </summary>
            GoddessAwakeDragonSword = 0,
            /// <summary>
            /// すべての雑魚敵たち
            /// </summary>
            AllEnemy = 1,
            /// <summary>
            /// 迷宮のラスボス
            /// </summary>
            Boss = 2,
        }

        /// <summary>
        /// 訪れたことがあるか
        /// </summary>
        public bool IsEnterd { get; set; } = false;

        /// <summary>
        /// trueである限り、空部屋として認識させる
        /// </summary>
        public bool IsHidden { get; set; } = false;

        /// <summary>
        /// RoomData系クラス共有で使用する、特定目的のためにグループ化された部屋を認識するための配列。
        /// HashSetは重複を許さないList
        /// </summary>
        static protected Dictionary<GroupID, HashSet<RoomData>> _groupRooms =
                                            new Dictionary<GroupID, HashSet<RoomData>>();

        /// <summary>
        /// 全RoomData系クラス共有で扱う変数を初期化
        /// </summary>
        public static void InitGroupRooms()
        {
            var enums = Enum.GetValues(typeof(GroupID));
            foreach (GroupID id in enums)
            {
                _groupRooms[id] = new HashSet<RoomData>();
            }
        }
        public static RoomData[] GetGroupRooms(GroupID groupID)
        {
            return _groupRooms[groupID].ToArray();
        }
    }
}
