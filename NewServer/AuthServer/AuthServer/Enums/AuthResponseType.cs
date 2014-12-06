using System;
namespace AuthServer.Enums
{
    public enum AuthResponseType : byte
    {
        WrongPass = 1,
        Successful = 255,
        ServerFull = 20,
        Banned = 0
    }
}
