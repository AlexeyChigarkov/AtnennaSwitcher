using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtnennaSwitcher
{
    public class ResponseHelper
    {
        private List<int> ParseBlocked(string message)
        {
            var b = Convert.ToInt16(message);
            var blocked = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                var bit = (b & (1 << i - 1)) != 0;
                if (!bit)
                {
                    blocked.Add(i);
                }
            }
            return blocked;
        }

        public ResponseData ParseResponse(string message)
        {
            var data = new ResponseData();

            data.AntennaA = "buttonSA" + message.Substring(1, 2);
            data.FilterA = "buttonFA" + message.Substring(3, 2);
            data.AntennaB = "buttonSB" + (message.Substring(12, 2));
            data.FilterB = "buttonFB" + message.Substring(14, 2);
            data.BlockedA = ParseBlocked(message.Substring(7, 4));
            data.BlockedB = ParseBlocked(message.Substring(18, 4));
            data.TxA = Convert.ToInt32(message.Substring(23, 1)) == 1 ? true : false;
            data.TxB = Convert.ToInt32(message.Substring(24, 1)) == 1 ? true : false;
            data.IsAlarm = Convert.ToInt32(message.Substring(25, 1)) == 1;
            return data;
        }
    }
}
