using cliente_borrar_socket;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace deleteConsoleApp1
{
    class Program
    {
        static void Main(string [] args)
        {
            Cliente cliente = new Cliente();
            cliente.start();
            while(true){
                cliente.send_message();
                Console.Write(cliente.receive_message());
            }
            Console.ReadKey();
        }
    }
}
