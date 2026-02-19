using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

class Program
{

    static async Task Main(string[] args)
    {
        using (var pipeClient = new NamedPipeClientStream(".", "TestPipe", PipeDirection.Out))
        {
            Console.WriteLine("Подключение к серверу...");
            await pipeClient.ConnectAsync();
            Console.WriteLine("Подключено к серверу.");
            using (var writer = new BinaryWriter(pipeClient))
            {

                while (true)
                {
                    Console.Write("Введите сообщение: ");
                    string message = Console.ReadLine();
                    writer.Write(message);
                    if (message.Equals("Выйти"))
                    {
                        break;
                    }


                    Console.Write("Введите путь файла: ");
                    string filepath = Console.ReadLine();
                    if (!File.Exists(filepath))
                    {
                        Console.WriteLine("Файл не найден.");
                        continue;
                    }

                    FileInfo fileInfo = new FileInfo(filepath);
                    writer.Write(fileInfo.Length);
                    writer.Write(fileInfo.Name);
                    using (var filestream = File.OpenRead(filepath))
                    {
                        filestream.CopyTo(pipeClient);
                    }
                }
            }
        }
    }
}
