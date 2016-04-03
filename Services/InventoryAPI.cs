using System;
using System.Collections.Generic;
using System.Linq;
using ValantData;

namespace ValantApi
{
	public class InventoryApi : IInventoryApi
	{
		public InventoryApi ()
		{
			existingInventory = new List<Item> ();
		}

		private List<Item> existingInventory;

		public Notification InsertItem (Item item) {
			TransactionType transactionType = TransactionType.FailedToInsert;

			if (ValidateItem(item)) {
				item.Id = GetNextId ();
				existingInventory.Add(item);
				transactionType = TransactionType.Inserted ;
			}

			Notification insertNotification = new Notification (item, transactionType);	

			return 	insertNotification;
		}

		public Notification RemoveByLabel (int label) {
			Item itemToRemove = existingInventory.FirstOrDefault (item => item.Label == label);
			TransactionType transactionType = (itemToRemove != null ? TransactionType.Removed : TransactionType.FailedToRemove);
			Notification notification = new Notification (itemToRemove, transactionType);

			existingInventory = existingInventory.Where(item => item.Label != label).ToList();
		
			return notification;
		}

		public IEnumerable<Notification> GetExpiredItemNotifications () {
			IEnumerable<Item> expiredItems = existingInventory.Where(item => 
				item.ExpirationDate.HasValue &&
				item.ExpirationDate.Value < TimeUtility.Now).ToList();

			List<Notification> expirationNotifications = new List<Notification> ();

			foreach(Item item in expiredItems){
				expirationNotifications.Add (
					new Notification(item, TransactionType.Expired)
				);
				existingInventory.Remove (item);
			}

			return expirationNotifications;
		}

		private bool ValidateItem(Item item) {
			bool isValidItem = true;

			if (item == null) {
				isValidItem = false;
			} else if (item.Label <= 0 ) {
				isValidItem = false;
			} else if (item.Description == string.Empty) {
				isValidItem = false;
			} else if (existingInventory.Any(x => x.Label == item.Label)) {
				isValidItem = false;
			} else if (item.ExpirationDate.HasValue && item.ExpirationDate.Value < TimeUtility.Now) {
				isValidItem = false;				
			}

			return isValidItem;
		}

		private int GetNextId() {
			int nextId = (existingInventory.Max (item => item.Id) ?? 0) + 1;

			return nextId;
		}
	}
}

