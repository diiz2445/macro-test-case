using Server.Server;

ServerConnection server = new ServerConnection("127.0.0.1", 8888, 1);
try
{
    server.Start();    
}
finally
{
    server.Stop();
}