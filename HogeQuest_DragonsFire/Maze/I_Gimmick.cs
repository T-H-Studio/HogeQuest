using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze
{
    /// <summary>
    /// 何かギミックを作る際に必ず実装して欲しいインターフェース
    /// </summary>
    internal interface I_Gimmick
    {
        /// <summary>
        /// 何かしらのギミックを書くメソッド。
        /// </summary>
        void PlayGimmick(PlayerData playerData);
    }
}
