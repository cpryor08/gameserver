using System;
using System.Text;
namespace AuthServer
{
    public class Calculations
    {
        public static string DateTimeToString(DateTime DT)
        {
            StringBuilder SB = new StringBuilder();
            SB.Append(DT.Year);
            SB.Append("-");
            SB.Append(DT.Month);
            SB.Append("-");
            SB.Append(DT.Day);
            SB.Append(" ");
            SB.Append(DT.Hour);
            SB.Append(":");
            SB.Append(DT.Minute);
            SB.Append(":");
            SB.Append(DT.Second);
            return SB.ToString();
        }
    }
}
