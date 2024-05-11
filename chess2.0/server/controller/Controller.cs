using Fleck;
using Newtonsoft.Json;

public class Controller
{
    public static string CreateRoom(IWebSocketConnection client)
    {
        var roomId = Guid.NewGuid().ToString();
        Db.CreateRoom(roomId, client);
        return CreateJsonMessage(MessageType.Create, null, roomId);
    }

    public static string JoinRoom(IWebSocketConnection client, string roomId)
    {
        var gameRoom = Db.JoinRoom(roomId, client);
        if (gameRoom == null)
        {
            return CreateJsonMessage(MessageType.Error, null, roomId);
        }

        return CreateJsonMessage(MessageType.Join, null, roomId);
    }

    public static string Init(string roomId)
    {
        var gameRoom = Db.GetRoomState(roomId);
        var gameRoomDto = new GameRoomDto(gameRoom, FigureColors.WHITE);
        return CreateJsonMessage(MessageType.Init, gameRoomDto, roomId);
    }

    public static (List<Player>, (string firstMessage, string secondMessage)) Start(string roomId)
    {
        var gameRoom = Db.StartGame(roomId);
        var firstGameRoomData = new GameRoomDto(gameRoom, FigureColors.WHITE);
        var secondGameRoomData = new GameRoomDto(gameRoom, FigureColors.BLACK);
        return (gameRoom.Players,
            (CreateJsonMessage(MessageType.Start, firstGameRoomData, roomId),
                CreateJsonMessage(MessageType.Start, secondGameRoomData, roomId)));
    }

    public static (List<Player>, (string firstMessage, string secondMessage)) Move(string roomId, string moveParams)
    {
        var gameRoom = Db.ChangeRoomState(roomId, moveParams);
        var firstGameRoomData = new GameRoomDto(gameRoom, FigureColors.WHITE);
        var secondGameRoomData = new GameRoomDto(gameRoom, FigureColors.BLACK);
        
        if (gameRoom.IsMate)
        {
            return (gameRoom.Players,
                (CreateJsonMessage(MessageType.EndGame, firstGameRoomData, roomId),
                    CreateJsonMessage(MessageType.EndGame, secondGameRoomData, roomId)));
        }
        
        return (gameRoom.Players,
            (CreateJsonMessage(MessageType.NextTurn, firstGameRoomData, roomId),
                CreateJsonMessage(MessageType.NextTurn, secondGameRoomData, roomId)));
    }

    private static string CreateJsonMessage(MessageType type, GameRoomDto? gameRoomDto, string roomId)
    {
        var message = new MessageToClient(type, gameRoomDto, roomId);
        return JsonConvert.SerializeObject(message);
    }
}