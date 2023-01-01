using System;

namespace AlarmBot
{
	public abstract class Bot
	{
        public abstract void SetNotification(List<ProductInfo> drawProducts);

        protected abstract void SendMessageToAllUsers(ProductInfo product);
    }
}
