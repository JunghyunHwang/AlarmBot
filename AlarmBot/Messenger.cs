using System;

namespace AlarmBot
{
	public abstract class Messenger
	{
        public abstract void SetNotification(List<ProductInfo> drawProducts);

        protected abstract void SendMessageToAllUsers(System.Timers.Timer timer, ProductInfo product);
    }
}
