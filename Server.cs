using System.Collections.Generic;
using Lidgren.Network;
using System;
using System.Threading;
using System.Timers;
using System.Xml.Serialization;

namespace ServerApp
{
    public class Server
    {
       private Logger logger = new Logger();
       private DataLogger logData = new DataLogger();

        private List<Client> clients;
        private NetPeerConfiguration config;
        private NetServer server;
        private Random rnd;
        private List<Fruits> apples;
        private int maxFruits = 10;

        public Server()
        {
           clients = new List<Client>();
           config = new NetPeerConfiguration("Sharks") { Port = 15000};
           config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
           server = new NetServer(config);
           rnd = new Random();
             apples = new List<Fruits>();
        }

        public void Run()
        {
            server.Start();
            Console.WriteLine("Server started....");
            
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            timer.Interval = 1000;
            timer.Enabled = true;
                            
                            
                            
            GC.KeepAlive(timer);
            
            while (true)
            {
                NetIncomingMessage inc;
                if ((inc = server.ReadMessage()) == null)
                    continue;

      
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        Console.WriteLine("Incoming Connection Request...");
                        var data = inc.ReadByte();
                        if (data == (byte) PacketType.Login)
                        {
                            NetworkLoginInformation logingInfo = new NetworkLoginInformation();
                            inc.ReadAllProperties(logingInfo);
                            Console.WriteLine("Connection Accepted From " + inc.SenderConnection.RemoteEndPoint.ToString());
                            inc.SenderConnection.Approve();

                            logger.Log(logingInfo.Name + ": " + inc.SenderConnection.ToString());
                            
                            var client = new Client(logingInfo.Name,inc.SenderConnection,NetConnectionStatus.Connected);
                            clients.Add(client);
                            var outmsg = server.CreateMessage();
                            
                            outmsg.Write((byte) PacketType.Login);
                            outmsg.Write(client.Id);
                            outmsg.Write(clients.Count);
                            for (int i = 0; i < clients.Count; i++)
                            {
                                outmsg.Write(clients[i].Id);
                                outmsg.Write(clients[i].Name);
                                outmsg.Write(clients[i].X);
                                outmsg.Write((clients[i].Y));
                            }

                            Thread.Sleep(20);
                            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.Unreliable, 0);
                            SendNewPlayer(client,inc);

                            
















                        }
                        else
                        {
                            inc.SenderConnection.Deny(("Invalid info"));
                        }

                        break;

                    case NetIncomingMessageType.Data:
                        Data(inc);
                        break;
                    case NetIncomingMessageType.Receipt:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                    
                         
                }
               
                
                
                
                
                
                
                
            }
        }

        public void Data(NetIncomingMessage inc)
        {
            var packetType = (PacketType) inc.ReadByte();
            switch (packetType)
            {
                case PacketType.Input:

                    var tid = inc.ReadInt32();
                    var x = inc.ReadDouble();
                    var y = inc.ReadDouble();

                    clients[tid].X = x;
                    clients[tid].Y = y;
                    
                    Console.WriteLine(clients[tid].Name + " " + clients[tid].Id + " " + clients[tid].X + " " + clients[tid].Y);
                    SendNewPlayer(clients[tid],inc);
                    break;
                case PacketType.Eat:
                    var appID = inc.ReadInt32();
                    logData.Log(appID.ToString());
                    apples.Remove(apples[appID]);
                    break;
            }
            
        }
        
        public void SendNewPlayer(Client client, NetIncomingMessage inc)
        {
            var msg = server.CreateMessage();
            msg.Write((byte)PacketType.NewPlayer);
            msg.Write(client.Id);
            msg.Write(client.Name);
            msg.Write(client.X);
            msg.Write(client.Y);
                                
            server.SendToAll(msg,inc.SenderConnection,NetDeliveryMethod.ReliableOrdered,0);
            
            var appleMessage = server.CreateMessage();
            appleMessage.Write((byte)PacketType.Fruit);
            appleMessage.Write(apples.Count);
            for (int i = 0 ; i < apples.Count ; i ++)
            {
                appleMessage.Write(i);
                appleMessage.Write(apples[i].getX());
                appleMessage.Write((apples[i].getY()));
            }
            server.SendToAll(appleMessage,NetDeliveryMethod.ReliableOrdered);
            
        }
        
        private void SendFullPlyerList()
        {
            Console.WriteLine("Sending all players...");
            var outmessage = server.CreateMessage();
            outmessage.Write((byte)PacketType.AllPlayers);
            outmessage.Write(clients.Count);
            foreach (var cl in clients)
            {
                outmessage.Write(cl.Id);
                outmessage.Write(cl.Name);
                outmessage.Write(cl.X);
                outmessage.Write(cl.Y);
            }
            server.SendToAll(outmessage,NetDeliveryMethod.ReliableOrdered);

            

        }


        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
           if(apples.Count < maxFruits)
            apples.Add(new Fruits(rnd.Next(0,3000),rnd.Next(0,2000)));
   
        }
        
        
        }
        
   
    
        
    }
