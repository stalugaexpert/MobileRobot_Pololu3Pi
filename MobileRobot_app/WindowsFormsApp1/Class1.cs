using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace robotApp
{
    class Menago
    {
        /* Pola określające stan zarządcy */
        private StanZarzadcy AktualnyStanZarzadcy = StanZarzadcy.Niepolaczony;

        /* Gniazdo zarządcy */
        private TcpClient Gniazdo;

        #region StanZarzadcy
        enum StanZarzadcy
        {
            Niepolaczony = 1,
            Polaczony = 2
        }
        #endregion
       

        public void Connectt(string Address, int port)
        {

            Disconnect();
            //TcpClient client = new TcpClient("127.0.0.1", 1302);
            Gniazdo = new TcpClient();
            Gniazdo.Connect(Address, port);
            
            Thread.Sleep(300);
            AktualnyStanZarzadcy = StanZarzadcy.Polaczony;
        }

        public void Disconnect()
        {


            /* - Czynności - */
            if (this.AktualnyStanZarzadcy != StanZarzadcy.Niepolaczony)
            {
                NetworkStream Strumien;
                byte[] BajtowaZawartoscStrumienia = new byte[] { 0 };
                // Próba wysłania informacji kończącej połączenie do serwera
                try
                {
                    Strumien = Gniazdo.GetStream();
                    Strumien.Write(BajtowaZawartoscStrumienia, 0, 1);
                    Strumien.Flush();
                }
                catch
                {
                }

                Gniazdo.Close();

                AktualnyStanZarzadcy = StanZarzadcy.Niepolaczony;

            }

        }


    }
}

