using HogeQuest_DragonsFire.Maze.RoomType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze
{
    /// <summary>
    /// 迷宮を総括するクラス
    /// </summary>
    internal class GameController
    {
        private MazeData _mazeData;
        private MazeCreator _mazeCreator = new MazeCreator();

        private PlayerData _playerData = new PlayerData(Config.PLAYER_LIFE);

        public GameController() 
        {
            _mazeData = _mazeCreator.CreateMaze();
        }

        /// <summary>
        /// ゲーム開始！
        /// </summary>
        public void Play()
        {
            //入力カーソルをピコピコさせない
            Console.CursorVisible = false;

            ShowBeginMessage();

            while (true) 
            {
                //迷宮の状況を描画
                DrawMaze();

                //MoveInput()で部屋移動を受付して、移動先の部屋のギミック情報を代入する。移動してなければnull。
                I_Gimmick gimmick = MoveInput();

                //移動が出来ていれば
                if (gimmick != null) 
                {
                    //キー入力後の状況を描画
                    DrawMaze();

                    //ギミックを起動する
                    gimmick.PlayGimmick(_playerData);
                }

                //ループを抜ける条件判定
                if (_playerData.Life <= 0){ break; }
                if (IsGameCleared() == true){ break; }
            }

            //サヨナラ
            if (_playerData.Life <= 0)
            {
                Console.Clear();
                DrawMaze();
                KeyWaitMessage("HPが尽きた...もう立ち上がることができない....");
                Console.WriteLine("   GAME OVER");
            }

            //おめでとう！
            if (IsGameCleared() == true)
            {
                Console.Clear();
                KeyWaitMessage("迷宮に潜む黒龍は倒れ、空を覆っていた暗雲は晴れた。");
                KeyWaitMessage("人々はやがてあなたの偉業を知り、英雄として語り継ぐ事だろう。");
                KeyWaitMessage("願わくばこの平和が、いつまでも続きますように―――");
                Console.WriteLine("    CONGRATULATIONS !");
                Console.WriteLine("YOU SAVE THE Hoge WORLD!");
                Console.WriteLine("黒龍討伐にかかった歩数: " + _playerData.WalkCount);
            }
        }

        /// <summary>
        /// 最初に表示されるチュートリアルメッセージ
        /// </summary>
        private void ShowBeginMessage()
        {
            Console.WriteLine("  Hoge Quest");
            Console.WriteLine("-Dragon's Fire-");
            Console.WriteLine();
            KeyWaitMessage("\"PRESS ENTER KEY\"");
            KeyWaitMessage("ゲーム中にこういった文章を読み進める時はEnterを押してくれ。");
            KeyWaitMessage("ようこそ！Hogeの世界へ！");

            Console.WriteLine("君は今、世界の敵「黒龍」が潜む迷宮にやってきた。");
            Console.WriteLine("とても手ごわいが、きっと勝つ方法はあるはず。");
            KeyWaitMessage("迷宮を探索し、黒龍を討ち果たそう！！");            

            Console.WriteLine("-説明-");
            Console.WriteLine("迷宮の中は「↑←→↓」の矢印キーで移動が出来るぞ。");
            Console.WriteLine("君自身は「〇」で表現されている。");
            Console.WriteLine("HPがあり、0になると死んでしまう...");
            Console.WriteLine();
            Console.WriteLine("迷宮内には他にも色々な記号(#、$、@)があるが、");
            Console.WriteLine("その記号の場所に移動すると色々なことが起こるぞ！");
            KeyWaitMessage("何が起こる記号なのかは、ゲーム中に一覧があるのでそれを見てほしい。");

            Console.WriteLine("それでは迷宮に入ろう。");
            KeyWaitMessage("健闘を祈る！");
        }

        /// <summary>
        /// 迷宮を描画
        /// </summary>
        private void DrawMaze()
        {
            string[] strings = _mazeCreator.CreateConsoleMazeStrings(_mazeData.NowPointX, _mazeData.NowPointY);

            //WriteLineなどのConsole出力関数が書き出す文字の開始位置を一番最初の位置にする。
            //これならConsole.Clear()と違って、画面下のほうに出力された文字は消えずに残り続ける。
            Console.SetCursorPosition(0, 0);

            foreach (string s in strings) 
            {
                Console.WriteLine(s);
            }

            string nowPoint = "座標[" + _mazeData.NowPointX + "," + _mazeData.NowPointY + "]";
            string playerLife = "HP:" + _playerData.Life + "/" + _playerData.MaxLife;
            string howToRoomType = "($:宝  #:敵  @:黒龍)";
            string walk = "歩数:" + _playerData.WalkCount;
            Console.WriteLine(nowPoint + "  " + playerLife + "  " + howToRoomType + "  " + walk);
            Console.WriteLine("--------------------------------------");
        }

        /// <summary>
        /// プレイヤーのキー入力を待ち、それが移動用の矢印キーなら移動をして、
        /// 移動先のRoomの情報をI_Gimmick型に限定して戻す
        /// </summary>
        private I_Gimmick MoveInput()
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.LeftArrow:
                    return _mazeData.MoveRoom(MazeData.Direction.West);

                case ConsoleKey.RightArrow:
                    return _mazeData.MoveRoom(MazeData.Direction.East);

                case ConsoleKey.UpArrow:
                    return _mazeData.MoveRoom(MazeData.Direction.North);

                case ConsoleKey.DownArrow:
                    return _mazeData.MoveRoom(MazeData.Direction.South);

                default:
                    return null;
            }
        }

        /// <summary>
        /// メッセージ表示の後にKey入力を待つタイプのメッセージ
        /// </summary>
        private void KeyWaitMessage(string message, ConsoleKey targetKey = ConsoleKey.Enter)
        {
            Console.WriteLine(message + "▼");

            //設定されたキーを押していない限り、ループし続ける
            while (Console.ReadKey(true).Key != targetKey) { }

            //コンソール画面のクリア
            Console.Clear();
        }

        /// <summary>
        /// ゲームクリア条件を判定する
        /// </summary>
        private bool IsGameCleared()
        {
            bool clear = true;

            RoomData[] rooms = RoomData.GetGroupRooms(RoomBase.GroupID.Boss);

            //1つでも到達(制覇)していないボス部屋があるなら未クリアとする(今回は1つしかない)
            foreach(var room in rooms) 
            {
                if (room.IsEnterd == false)
                {
                    clear = false;
                    break;
                }
            }

            return clear;
        }
    }
}
