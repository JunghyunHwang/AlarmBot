namespace AlarmBot
{
	public class User
	{
		public string ChatID { get; private set; } // I think that we need to messenger info class
        private readonly Messenger mMessenger;

        public User(Messenger messenger, string chatID)
        {
            mMessenger = messenger;
            ChatID = chatID;
        }

        public void SendMessage(ProductInfo p)
        {
            mMessenger.SendMessage(p, this);
        }
    }
}
