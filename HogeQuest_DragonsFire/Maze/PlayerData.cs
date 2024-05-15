using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire.Maze
{
    /// <summary>
    /// プレイヤーの各種パラメータ  8.
    /// </summary>
    internal class PlayerData
    {
        public int WalkCount;//歩数
        public bool HasDragonSword;//竜の剣を所持しているか

        public int MaxLife { get; private set; }
        public int Life//体力
        {
            get
            {
                return _life;
            }
            set
            {
                _life = (value >= 0) ? value : 0;//_lifeが0未満にならないようにする
                _life = (_life >= MaxLife) ? MaxLife : _life;//_lifeがMaxLifeを超えないようにする
            }
        }

        int _life;

        public PlayerData(int life)
        {
            MaxLife = life;
            _life = MaxLife;
        }
    }
}
