using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace cliente_borrar_socket
{
    public class Cliente
    {
        IPHostEntry host;
        IPAddress ipAddress;
        IPEndPoint ipEndpoint;

        Socket s_client;
        public Cliente(string ip = "127.0.0.1", int port = 4445)
        {
            ipEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            s_client = new Socket(IPAddress.Parse(ip).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void start()
        {
            s_client.Connect(ipEndpoint);
        }

        public void send_message()
        {
            string parametros = "04TRN01|AMT2500|";
            byte[] byte_msg = Encoding.ASCII.GetBytes(parametros);
            byte LRC = calculateLRC(byte_msg);
            byte_msg = Encoding.ASCII.GetBytes("\x02" + parametros + "\x03" + LRC);


            s_client.Send(byte_msg);
            Console.WriteLine("Mensaje Enviado");
        }

        private byte calculateLRC(byte[] b)
        {
            byte lrc = 0x00;
            for (int i = 0; i < b.Length; i++)
            {
                lrc = (byte)((lrc + b[i]) & 0xFF);
            }
            lrc = (byte)(((lrc ^ 0xff) + 2) & 0xFF);
            return lrc;
        }

        public string receive_message ()
        {
            byte [] buffer_response = new byte[1024];
            s_client.Receive(buffer_response);
            return byte_to_string(buffer_response);
        }

        public string byte_to_string(byte[] buffer)
        {
            string message;
            int endIndex;

            message = Encoding.ASCII.GetString(buffer);
            endIndex = message.IndexOf('\0');
            message = endIndex > 0 ? message.Substring(0, endIndex) : message;
            return message;

        }

    }
}
