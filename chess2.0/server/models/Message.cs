using Newtonsoft.Json;

public class Message
{
    [JsonProperty("type")]
    public MessageType Type;
    [JsonProperty("params")]
    public GameRoomDto? Params;
    [JsonProperty("roomId")]
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
    Init,
    Start,
    Leave,
    Move,
    EndGame,
    ChangeFigure,
    Restart,
    Error,
}