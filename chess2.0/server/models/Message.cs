public class Message
{
    public MessageType Type;
    public GameRoomDto? Params;
    public string RoomId;

    public Message(MessageType type, GameRoomDto? messParams, string roomId)
    {
        Type = type;
        Params = messParams;
        RoomId = roomId;
    }
}

public enum MessageType
{
    Create,
    Join,
    Start,
    Leave,
    Move,
    EndGame,
    Error,
    ChangeFigure,
    Restart
}