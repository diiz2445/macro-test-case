using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

public class Client
{
    private TcpClient _client;
    private const int Port = 8888; // Порт сервера
    private const string ServerIp = "127.0.0.1"; // IP-адрес сервера

    public Client()
    {
        _client = new TcpClient();
    }

    public async Task StartAsync()
    {
        try
        {
            await _client.ConnectAsync(ServerIp, Port);
            Console.WriteLine("Подключение к серверу установлено.");

            using var stream = _client.GetStream();
            string message;

            Console.WriteLine("Введите сообщение для отправки (или 'exit' для выхода):");
            //while ((message = Console.ReadLine()) != "exit")
            {
                string path = "input1.txt";
                message = GetStringFromFile(path);
                // Отправка сообщения на сервер
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine($"Отправлено: {message}");

                // Получение ответа от сервера
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine("aa");
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Ответ от сервера: {response}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            _client.Close();
            Console.WriteLine("Соединение закрыто.");
        }
    }
    public static string GetStringFromFile(string path)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            StreamReader sr = File.OpenText(path);
            string s = sr.ReadToEnd();
            sb.Insert(0, s);

        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        return sb.ToString();
    }
}

