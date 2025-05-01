using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze.RoomType
{
    /// <summary>
    /// 迷宮を構成する部屋を表す基底(基本)クラス
    /// </summary>
    internal abstract class RoomBase : RoomData, I_Gimmick
    {
        /// <summary>
        /// コンソール画面で、迷宮内のセリフを表示し始めるコンソール画面上の行位置
        /// </summary>
        private const int DEF_CURSOR_INDEX = 15;

        //インターフェースのメソッド
        public void PlayGimmick(PlayerData playerData)
        {
            //歩数は全ギミック共通でカウント
            playerData.WalkCount++;

            ClearMazeMessage();

            if (IsHidden == false)
            {
                PlayRoomGimmick(playerData);
            }
            else
            {
                ShowMazeMessage("この部屋には何もないようだ。", false);
            }
        }

        /// <summary>
        /// 部屋に入ると起こるギミック
        /// </summary>
        protected abstract void PlayRoomGimmick(PlayerData playerData);


        /// <summary>
        /// 迷宮のセリフを出力(1行のみの上書き方式)
        /// waitをtrueにすれば、Enterを押さないと進めないように出来る。
        /// </summary>
        protected void ShowMazeMessage(string message, bool wait = true)
        {//引数の「bool wait = true」は、呼び出し時に値がtrueでよければ引数省略出来る書き方

            ClearMazeMessage();

            string msg = wait == true ? message + " ▼" : message;
            Console.SetCursorPosition(0, DEF_CURSOR_INDEX);
            Console.WriteLine(msg);

            if (wait)
            {
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                ClearMazeMessage();
            }
        }

        /// <summary>
        /// 迷宮のセリフをクリア
        /// </summary>
        protected void ClearMazeMessage()
        {
            int beforeIndex = Console.CursorTop;
            Console.SetCursorPosition(0, DEF_CURSOR_INDEX);
            Console.Write(new string(' ', Console.BufferWidth));//無を書き込んでクリアとみなす
            Console.SetCursorPosition(0, beforeIndex);
        }
    }
}
