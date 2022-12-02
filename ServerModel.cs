using System.Collections.Generic;
using System.Reflection;

namespace Zadanie_SK_2
{
    public class ServerModel
    {
        public bool IsDNS { get; set; }
        public int Id { get; set; }
        public decimal Time { get; set; }
        public List<RequestModel> RequestsQueue { get; set; } 
    }
}