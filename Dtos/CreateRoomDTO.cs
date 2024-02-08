using ChatBody;

namespace CreateDTO
{
    public class CreateRoomDTO
    {
        public string ChatID {get;set;}
        public string ChatName {get;set;}
        public int OnlineUser { get; set; } = 10; 

        //user.id
        public string Owner {get;set;}
    }
}