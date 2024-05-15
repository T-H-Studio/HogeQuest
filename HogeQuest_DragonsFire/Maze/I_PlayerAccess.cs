using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze
{
    /// <summary>
    /// 何かプレイヤーデータに作用するイベントを作る際に必ず実装して欲しいインターフェース  9.
    /// </summary>
    internal interface I_PlayerAccess
    {
        /// <summary>
        /// プレイヤーのパラメータを変更する処理(何かしらのイベント)を書くメソッド
        /// </summary>
        void ChangeParameter(PlayerData playerData);
    }
}
