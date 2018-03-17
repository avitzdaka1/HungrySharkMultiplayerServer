using System.Collections.Generic;
using System;

using System.Threading;
using Lidgren.Network;


namespace ServerApp
{
    class Program
    {

        public static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();

        }

        
        
    }
}