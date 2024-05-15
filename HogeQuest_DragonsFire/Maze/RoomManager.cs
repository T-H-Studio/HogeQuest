using HogeQuest_DragonsFire.Maze.RoomType;
using System;
using System.Reflection;

namespace HogeQuest_DragonsFire.Maze
{
    //迷宮の部屋たちを管理するクラス  2.
    internal class RoomManager
    {
        RoomBase[][] _mazeRooms;//部屋データ一覧

        public RoomManager(RoomBase[][] roomDatas) 
        {
            _mazeRooms = roomDatas;
        }

        public RoomData GetRoomData(int pointX, int pointY) 
        {
            if(_mazeRooms.Length > pointY) 
            {
                if (_mazeRooms[pointY] != null && _mazeRooms[pointY].Length > pointX)
                {
                    return _mazeRooms[pointY][pointX];
                }
            }

            Console.WriteLine("存在しない部屋座標が指定されました！: X " + pointX + ", Y " + pointY);
            return null;
        }
        public I_PlayerAccess GetRoomEvent(int pointX, int pointY)
        { 
            RoomBase rd = (RoomBase)GetRoomData(pointX, pointY);//ダウンキャスト

            return rd;//アップキャスト
        }

        public void GetStartPoint(out int pointX, out int pointY) 
        {
            for (int y = 0; y < _mazeRooms.Length; y++)
            {
                for (int x = 0; x < _mazeRooms[y].Length; x++)
                {
                    //クラスがRoom_StartPoint型のものが入っているかをチェックする記述
                    //_mazeRoomsにRoomData型としてアップキャストされ入れられていても、
                    //GetType()は本来の型であるRoom_StartPointを検知出来る。
                    if (_mazeRooms[y][x].GetType() == typeof(Room_StartPoint))
                    {
                        pointX = x;
                        pointY = y;

                        return;
                    }
                }
            }

            //Room_StartPointが無かったら0,0を返す
            pointX = 0;
            pointY = 0;
        }
    }
}
