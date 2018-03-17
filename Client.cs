using System.Net.Configuration;
using Lidgren.Network;

namespace ServerApp
{
    public class Client
    {
        private static int counter = 0;
        private int id;
        private string name;
        private NetConnection address;
        private NetConnectionStatus status;

        private double y;
        private double x;

        public double X
        {
            get => x;
            set => x = value;
        }

        public double Y
        {
            get => y;
            set => y = value;
        }

        

        

        public Client(string name, NetConnection address, NetConnectionStatus status)
        {
            id = counter++;
            this.name = name;
            this.address = address;
            x=0;
            y=0;
            this.status = status;
        }

        public int Id => id;

        public string Name => name;

        public NetConnection Address => address;


        public NetConnectionStatus Status => status;





    }
}