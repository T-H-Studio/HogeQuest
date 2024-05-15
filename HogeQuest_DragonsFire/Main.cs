using HogeQuest_DragonsFire.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogeQuest_DragonsFire
{
    /// <summary>
    /// これがないと動かないクラス  0.
    /// </summary>
    internal class MainClass
    {
        static void Main()
        {
            try
            {
                //ゲーム開始
                GameController controller = new GameController();
                controller.Play();
            }
            catch (Exception e)
            {
                Console.WriteLine("なんかエラーになりました。スマン。");
                Console.WriteLine(e.TargetSite + " : " + e.Message);
            }
            finally 
            {
                Console.Write("    また来てね");
                Console.ReadLine();
            }
        }
    }
}
