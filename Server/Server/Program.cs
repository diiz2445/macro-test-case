using Server.Server;

ServerConnection server = new ServerConnection("", 8888, 1);
try
{
    server.Start();    
}
finally
{
    server.Stop();
}