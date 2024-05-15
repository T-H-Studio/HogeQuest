using HogeQuest_DragonsFire.Maze.RoomType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HogeQuest_DragonsFire.Maze
{
    /// <summary>難読クラス
    /// ゴール到達不能な迷宮が出来るのを回避するためにアルゴリズムを作るのが面倒なので
    /// 手打ちで迷宮を作るためのクラス。MAZE_SIZEが変わったら、
    /// 書いた迷宮も大きさに合わせて書き換えないといけない点に注意    4.
    /// </summary>
    internal class MazeCreator
    {
        /// <summary>
        /// 文字から部屋を作る時の、番号対応表enum
        /// </summary>
        private enum RoomTypeNum
        {
            Empty = 0,
            T_Heal,
            T_DSword,
            Enemy,
            Boss,
            Trap,
            Goddess,// 6
            StartP = 9,
        };

        private string[] _mazeMap = new string[Config.MAZE_SIZE * 2 + 1]//行情報は迷宮サイズ*2+1で決まる
            {//「―」は横壁　「｜」は縦壁　「□」は壁無し（通路）
             //「0」は空部屋　「1」は宝：回復薬　「2」は宝：竜の剣　「3」は雑魚敵
             //「4」はドラゴン「5」はトラップ    「6」は女神の絵  　「9」は開始地点
                "　―　―　―　―　―　―　",
                "｜ 2｜ 0□ 0□ 0□ 0□ 4｜",
                "　□　□　―　―　―　―　",
                "｜ 0｜ 0□ 0□ 0｜ 3□ 1｜",
                "　□　―　―　□　□　□　",
                "｜ 3□ 0｜ 0｜ 0□ 0｜ 1｜",
                "　―　□　□　□　―　―　",
                "｜ 0□ 0｜ 0□ 0□ 3｜ 6｜",
                "　□　□　□　□　□　□　",
                "｜ 0｜ 0｜ 0｜ 0｜ 0｜ 5｜",
                "　□　□　□　□　□　□　",
                "｜ 9｜ 0□ 3｜ 1｜ 5□ 5｜",
                "　―　―　―　―　―　―　",
            };

        private WallManager _wallManager;
        private List<bool[]> _rowWalls;
        private List<bool[]> _columnWalls;

        private RoomManager _roomManager;
        private List<RoomBase[]> _rooms;

        private string[] _consoleMazeMap = null;

        /// <summary>
        /// 迷宮データを作成する
        /// </summary>
        /// <returns></returns>
        public MazeData CreateMaze()
        {
            //壁データの作成
            _wallManager = CreateWall();

            //部屋データの作成
            _roomManager = CreateRoom();

            //迷宮データの作成
            MazeData mazeData = new MazeData(_wallManager, _roomManager);
            return mazeData;
        }

        /// <summary>
        /// プレイヤーが見る事になる、コンソール用の迷宮文字データを生成して戻す
        /// </summary>
        /// <returns></returns>
        public string[] CreateConsoleMazeStrings(int nowPointX, int nowPointY)
        {
            //はじめて_consoleMazeMapを作る場合は_mazeMapを見ながらしっかり作る
            //全角の文字が存在すると迷宮が崩れて表示されるので注意すること
            if (_consoleMazeMap == null)
            {
                _consoleMazeMap = new string[_mazeMap.Length];

                for (int i = 0; i < _mazeMap.Length; i++)
                {
                    string allString = _mazeMap[i];
                    string halfRepRow = allString.Replace("―", "--");//全角の半角化
                    string halfRepCol = halfRepRow.Replace("｜", "||");

                    string rowReplace = Regex.Replace(halfRepCol, @"\s", "++");//空白文字を++に変換する
                    string columnReplace = Regex.Replace(halfRepCol, @"\s", "");//空白文字を消す
                    string nonSpace = (i % 2 == 0) ? rowReplace : columnReplace;
                    string consoleRow = nonSpace.Replace("□", "  ");//壁無し記号を半角スペース2つに変換する

                    //部屋情報がある文字列の行はさらに編集する
                    if (i % 2 == 1)
                    {
                        string emptyReplace = consoleRow.Replace(Convert.ToString((int)RoomTypeNum.Empty), "  ");
                        string tresureReplace = emptyReplace.Replace(Convert.ToString((int)RoomTypeNum.T_Heal), " $");
                        string tresureReplace2 = tresureReplace.Replace(Convert.ToString((int)RoomTypeNum.T_DSword), " $");
                        string enemyReplace = tresureReplace2.Replace(Convert.ToString((int)RoomTypeNum.Enemy), " #");
                        string bossReplace = enemyReplace.Replace(Convert.ToString((int)RoomTypeNum.Boss), " @");
                        string trapReplace = bossReplace.Replace(Convert.ToString((int)RoomTypeNum.Trap), "  ");
                        string goddessReplace = trapReplace.Replace(Convert.ToString((int)RoomTypeNum.Goddess), "  ");
                        consoleRow = goddessReplace.Replace(Convert.ToString((int)RoomTypeNum.StartP), "  ");
                    }

                    _consoleMazeMap[i] = consoleRow;
                }
            }

            //_consoleMazeMapを元に、完成形を作る
            string[] playerInMazeMap = UpdateConsoleMazeStrings(nowPointX, nowPointY);
            return playerInMazeMap;
        }

        /// <summary>
        /// IsEnterdがtrueの部屋情報や、プレイヤーの位置情報を書き込んだMapを作成して戻す
        /// </summary>
        /// <param name="nowPointX"></param>
        /// <param name="nowPointY"></param>
        /// <returns></returns>
        private string[] UpdateConsoleMazeStrings(int nowPointX, int nowPointY)
        {
            string[] playerInMazeMap = new string[_consoleMazeMap.Length];
            List<string> roomExistMazeMap = new List<string>();//ルーム情報の行のみのリスト
            for (int i = 0; i < _consoleMazeMap.Length; i++)
            {
                playerInMazeMap[i] = _consoleMazeMap[i];

                if (i % 2 == 1)
                {
                    roomExistMazeMap.Add(playerInMazeMap[i]);
                }
            }
            
            //各部屋の情報を元に上書き
            for (int x = 0; x < Config.MAZE_SIZE; x++)
            {
                for (int y = 0; y < Config.MAZE_SIZE; y++)
                {
                    RoomData room = _roomManager.GetRoomData(x, y);
                    if (room.IsEnterd == true || room.IsHidden == true)
                    {
                        roomExistMazeMap[y] = CustomReplace(roomExistMazeMap[y], x, 2, "  ");
                    }
                }
            }

            //プレイヤー位置を元に上書き
            roomExistMazeMap[nowPointY] = CustomReplace(roomExistMazeMap[nowPointY], nowPointX, 2, "〇");

            //roomExistMazeMapの情報をplayerInMazeMapに反映
            for (int i = 0; i < playerInMazeMap.Length; i++)
            {
                if (i % 2 == 1)
                {
                    playerInMazeMap[i] = roomExistMazeMap.First();
                    roomExistMazeMap.RemoveAt(0);
                }
            }

            return playerInMazeMap;
        }
        
        /// <summary>
        /// 与えられたsource文字列のpointX座標に該当する文字数目から、length文字数分の文字をtarget文字に書き換えて戻す
        /// </summary>
        private string CustomReplace(string source, int pointX, int length, string target)
        {
            int startIndex = GetTruePointIndex(pointX);
            string beginStrings = source.Substring(0, startIndex);
            string endStrings = source.Substring(startIndex + length);

            string resultString = beginStrings + target + endStrings;
            return resultString;

            /// <summary>
            /// 座標として示される数字(引数point)から、文字配列の実際のIndex位置を計算して戻す
            /// </summary>
            int GetTruePointIndex(int point)
            {
                int leftWallOffsetCount = 2;//壁は半角２文字で表現されているから2
                int roomOffSet = point * 2;//部屋は半角２文字で表現されているから2
                int wallOffSet = point * 2;//壁

                return leftWallOffsetCount + roomOffSet + wallOffSet;
            }
        }

        /// <summary>
        /// 文字列を認識して壁の有無をbool型配列で作成し、それを元にWallManagerクラスを作成して戻す
        /// </summary>
        private WallManager CreateWall()
        {
            _rowWalls = new List<bool[]>();
            _columnWalls = new List<bool[]>();

            for (int i = 0; i < _mazeMap.Length; i++)
            {
                string allString = _mazeMap[i];//現在見ている行を格納
                string nonSpace = Regex.Replace(allString, @"\s", "");//空白文字をすべて消す
                string wallOnly = Regex.Replace(nonSpace, @"[0-9]", "");//数字をすべて消す
                char checkWord = (i % 2 == 0) ? '―' : '｜';//偶数行なら"―"、奇数行なら"｜"を壁として認識する

                //1文字ずつ調べて、その文字が壁か否かをexistWallに格納していく
                List<bool> existWall = new List<bool>();
                foreach (char cha in wallOnly)
                {
                    bool isWall = (cha == checkWord);
                    existWall.Add(isWall);
                }

                if (i % 2 == 0)
                {
                    _rowWalls.Add(existWall.ToArray());
                }
                else
                {
                    _columnWalls.Add(existWall.ToArray());
                }
            }

            WallManager wallManager = new WallManager(_rowWalls.ToArray(), _columnWalls.ToArray());
            return wallManager;
        }

        /// <summary>
        /// 文字列を認識して部屋の番号を元にRoomDataクラスを作成し、
        /// 作成したすべてのRoomDataを元にRoomManagerクラスを作成して戻す
        /// </summary>
        private RoomManager CreateRoom()
        {
            RoomBase.InitGroupRooms();
            _rooms = new List<RoomBase[]>();

            for (int i = 0; i < _mazeMap.Length; i++)
            {
                if (i % 2 == 0) { continue; }//奇数行は生成対象外

                string allString = _mazeMap[i];//現在見ている行を格納
                string numberOnly = Regex.Replace(allString, @"[^0-9]", "");//数字"以外"をすべて消す

                //1文字ずつ調べて、その文字に対応しているRoomクラスを生成＆格納していく
                List<RoomBase> rowRooms = new List<RoomBase>();
                foreach (char cha in numberOnly)
                {
                    //chaのデータをintに変換できたら
                    if (int.TryParse(cha.ToString(), out int roomTypeNumber) == true)
                    {
                        RoomTypeNum roomType = (RoomTypeNum)roomTypeNumber;
                        switch(roomType) 
                        {
                            case RoomTypeNum.Empty:
                                rowRooms.Add(new Room_Empty());
                                break;

                            case RoomTypeNum.T_Heal:
                                rowRooms.Add(new Room_Tresure_Heal());
                                break;

                            case RoomTypeNum.T_DSword:
                                rowRooms.Add(new Room_Tresure_DragonSword());
                                break;

                            case RoomTypeNum.Enemy:
                                rowRooms.Add(new Room_Enemy());
                                break;

                            case RoomTypeNum.Boss:
                                rowRooms.Add(new Room_Boss());
                                break;

                            case RoomTypeNum.Trap:
                                rowRooms.Add(new Room_Trap());
                                break;

                            case RoomTypeNum.Goddess:
                                rowRooms.Add(new Room_Goddess());
                                break;

                            case RoomTypeNum.StartP:
                                rowRooms.Add(new Room_StartPoint());
                                break;

                            default:
                                Console.WriteLine("想定外の数値がここには居ます！！：" + roomTypeNumber);
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("想定外の何者かがここには居ます！！："+ cha);
                    }
                }

                _rooms.Add(rowRooms.ToArray());
            }

            RoomManager roomManager = new RoomManager(_rooms.ToArray());
            return roomManager;
        }
    }
}
