using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtnennaSwitcher
{
   public class ResponseData
   {
       public string AntennaA;
       public string AntennaB;
       public string FilterA;
       public string FilterB;
       public bool TxA;
       public bool TxB;
       public List<int> BlockedA;
       public List<int> BlockedB;
       public bool IsAlarm;

   }
}
