using System;
using System.Collections.Generic;
using ValantData;

namespace ValantApi
{
	public interface IInventoryApi
	{
		Notification InsertItem (Item item);
		Notification RemoveByLabel (int label);
		IEnumerable<Notification> GetExpiredItemNotifications ();
	}
}
