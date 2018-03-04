using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoItX3Lib;
namespace AutoCentral
{
    public class Auto
    {
        public static AutoItX3 au3;
        static Auto()
        {
            au3 = new AutoItX3();
        }
        
        public static void play(string title,int x, int y,string value)
        {
            //au3.ControlClick("[TITLE:Đua Thú - Game kiếm tiền triệu phú - Google Chrome; CLASS:Chrome_WidgetWin_1]", "", "", "LEFT", 2, 195, 602);
            au3.ControlClick(title, "", "", "LEFT", 2, x, y);
            au3.ControlSend(title, "", "",value, 0);
        }
    }
}
