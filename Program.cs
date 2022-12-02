using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie_SK_2
{
    class Program
    {
        public static int m1 = 4; 
        public static int m2 = 3; 
        public static int sd1 = 2; 
        public static int sd2 = 1;
        private static List<ServerModel> servers = new List<ServerModel>();
        
        
        static int GetHttpRequestTime()
        {
            Random rnd = new Random();
            return rnd.Next(m1-sd1, sd1+m1 + 1);
        }

        static int GetDNSRequestTime()
        {
            Random rnd = new Random();
            return rnd.Next(m2-sd2, m2+sd2 + 1);
        }
        public static void Shuffle<T>(IList<T> values)
        {
            Random rand = new Random();
            for (int i = values.Count - 1; i > 0; i--) {
                int k = rand.Next(i + 1);
                T value = values[k];
                values[k] = values[i];
                values[i] = value;
            }
        }

        public static List<ServerModel> GetServerList()
        {
            List<ServerModel> returnList = new List<ServerModel>();
            returnList.Add( new ServerModel
            {
                IsDNS = true,
                Id = 1,
                RequestsQueue = new List<RequestModel>()
            });
            returnList.Add(  new ServerModel
            {
                IsDNS = true,
                Id = 2,
                RequestsQueue = new List<RequestModel>()
            });
            returnList.Add(  new ServerModel
            {
                IsDNS = true,
                Id = 3,
                RequestsQueue = new List<RequestModel>()
            });
            returnList.Add(  new ServerModel
            {
                IsDNS = false,
                Id = 4,
                RequestsQueue = new List<RequestModel>()
            });
            returnList.Add(  new ServerModel
            {
                IsDNS = false,
                Id = 5,
                RequestsQueue = new List<RequestModel>()
            });
            return returnList;
        }
        
        static async Task Main(string[] args)
        {
            
            var rnd = new Random();
            List<RequestModel> requests = new List<RequestModel>();
            servers = GetServerList();
            
            for (int i =1; i <= 100; i++)
            {
             requests.Add(new RequestModel
             {
                 Id = i,
                 IsDns = true 
             });
             requests.Add(new RequestModel
             {
                 Id = i++,
                 IsDns = false, 
                 
             });
            }
            Shuffle(requests);
            
            var tasks = new Task[]
            {
                ServerDNS1(),
                ServerDNS2(),
                ServerDNS3(),
                ServerHTTP1(),
                ServerHTTP2()
            };
            
            
            foreach (var request in requests)
            {
                Thread.Sleep(1);
                var server = LoadBalancer(request);
                server.RequestsQueue.Add(request);
                await Task.WhenAll(tasks);
            }

        }

        public static ServerModel LoadBalancer(RequestModel request)
        {
            if (request.IsDns)
            {
                return servers.Where(x => x.IsDNS && x.RequestsQueue.Count() == servers.Min(y => y.RequestsQueue.Count())).FirstOrDefault();
            }
            return servers.Where(x => x.RequestsQueue.Count() == servers.Min(y => y.RequestsQueue.Count())).FirstOrDefault();
        }

        public static async Task ServerDNS1()
        {
            var server = servers.Where(x => x.Id == 1).FirstOrDefault();
            while (server.RequestsQueue.Count() != 0)
            {
                foreach (var request in server.RequestsQueue)
                {
                    Task.Delay(GetDNSRequestTime());
                    server.RequestsQueue.Remove(request);   
                }
            }
        }
        public static async Task ServerDNS2()
        {
            var server = servers.Where(x => x.Id == 2).FirstOrDefault();
            while (server.RequestsQueue.Count() != 0)
            {
                foreach (var request in server.RequestsQueue)
                {
                    Task.Delay(GetDNSRequestTime());
                    server.RequestsQueue.Remove(request);   
                }
            }
        }
        public static async Task ServerDNS3()
        {
            var server = servers.Where(x => x.Id == 3).FirstOrDefault();
            while (server.RequestsQueue.Count() != 0)
            {
                foreach (var request in server.RequestsQueue)
                {
                    Task.Delay(GetDNSRequestTime());
                    server.RequestsQueue.Remove(request);   
                }
            }
        }
        public static async Task ServerHTTP1()
        {
            var server = servers.Where(x => x.Id == 4).FirstOrDefault();
            while (server.RequestsQueue.Count() != 0)
            {
                foreach (var request in server.RequestsQueue)
                {
                    Task.Delay(GetHttpRequestTime());
                    server.RequestsQueue.Remove(request);   
                }
            }
        }
        public static async Task ServerHTTP2()
        {
            var server = servers.Where(x => x.Id == 5).FirstOrDefault();
            while (server.RequestsQueue.Count() != 0)
            {
                foreach (var request in server.RequestsQueue)
                {
                    Task.Delay(GetHttpRequestTime());
                    server.RequestsQueue.Remove(request);   
                }
            }
        }
        
        public static async Task ProcessRequest(ServerModel server)
        {
            
        }
    }
}