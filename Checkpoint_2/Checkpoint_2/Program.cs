using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        using (var pipeServer = new NamedPipeServerStream("TestPipe", PipeDirection.In))
        {
            Console.WriteLine("Ожидание подключения клиента...");
            await pipeServer.WaitForConnectionAsync();
            Console.WriteLine("Клиент подключен.");
            using (var reader = new BinaryReader(pipeServer, Encoding.UTF8, leaveOpen: true))
            {
                while (true)
                {
                    try
                    {
                        string message = reader.ReadString();
                        Console.WriteLine($"Получено сообщение: {message}");
                        
                        long fileLength = reader.ReadInt64();
                        string fileName = reader.ReadString();
                        using (var fileStream = File.Create(fileName))
                        {
                            byte[] buffer = new byte[81920];
                            long totalRead = 0;

                            while (totalRead < fileLength)
                            {
                                int toRead = (int)Math.Min(buffer.Length, fileLength - totalRead);
                                int bytesRead = pipeServer.Read(buffer, 0, toRead);
                                fileStream.Write(buffer, 0, bytesRead);
                                totalRead += bytesRead;
                            }
                            
                        }
                        Console.WriteLine("Содержимое файла: " + File.ReadAllText(fileName, Encoding.UTF8));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        break;
                    }
                }

            }
        }
    }
}