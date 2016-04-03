using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ValantApi;
using ValantData;

namespace ValantUnitTests
{
	[TestFixture ()]
	public class InventoryApitTests
	{
		Item item1;
		Item item2;
		IInventoryApi inventoryService;
		
		[SetUp]
		public void ItemCreationAndInventorySetup()
		{
			inventoryService = new InventoryApi ();

			item1 = new Item () {
				Id = null,
				Label = 100001,
				Description = "Item1",
				EnteredDate = null,
				ExpirationDate = null
			};

			item2 = new Item () {
				Id = null,
				Label = 100002,
				Description = "Item2",
				EnteredDate = null,
				ExpirationDate = null
			};

			TimeUtility.ResetDateTimeNow ();
		}
			
		[Test ()]
		public void InsertItem_NotificationReturnsOriginalItem ()
		{
			Notification insertNotification  = inventoryService.InsertItem (item1);

			Assert.AreSame (item1, insertNotification.Item);
			Assert.AreNotSame (item2, insertNotification.Item);
		}

		[Test ()]
		public void InsertItem_NotificationReturnsOriginalItemWithId ()
		{
			Notification insertNotification  = inventoryService.InsertItem (item1);

			Assert.Greater(insertNotification.Item.Id, 0);
		}

		[Test ()]
		public void InsertItem_NotificationReturnsWithInsertedTransactionType ()
		{
			Notification insertNotification  = inventoryService.InsertItem (item1);

			Assert.AreEqual(TransactionType.Inserted, insertNotification.TransactionType);
		}

		[Test ()]
		public void InsertItem_NotificationReturnsWithInsertedDateOfToday ()
		{
			Notification insertNotification  = inventoryService.InsertItem (item1);

			Assert.AreEqual (System.DateTime.Now.Year, insertNotification.TransactionDate.Year);
			Assert.AreEqual (System.DateTime.Now.Month, insertNotification.TransactionDate.Month);
			Assert.AreEqual (System.DateTime.Now.Day, insertNotification.TransactionDate.Day);
		}

		[Test ()]
		public void InsertExpiredItem_NotificationReturnsWithInsertFailedTransactionType ()
		{
			item1.ExpirationDate = System.DateTime.Now.AddDays(-1);
			Notification insertNotification  = inventoryService.InsertItem (item1);

			Assert.AreEqual(TransactionType.FailedToInsert, insertNotification.TransactionType);
		}

		[Test ()]
		public void InsertItemSecondTime_NotificationReturnsWithInsertFailedTransactionType ()
		{
			inventoryService.InsertItem (item1);
			Notification insertNotification  = inventoryService.InsertItem (item1);

			Assert.AreEqual(TransactionType.FailedToInsert, insertNotification.TransactionType);
		}

		[Test ()]
		public void InsertUnlabeledItem_NotificationReturnsWithInsertFailedTransactionType ()
		{
			item1.Label = 0;
			Notification insertNotification  = inventoryService.InsertItem (item1);

			Assert.AreEqual(TransactionType.FailedToInsert, insertNotification.TransactionType);
		}

		[Test ()]
		public void InsertNoDescriptionItem_NotificationReturnsWithInsertFailedTransactionType ()
		{
			item1.Description = string.Empty;
			Notification insertNotification  = inventoryService.InsertItem (item1);

			Assert.AreEqual(TransactionType.FailedToInsert, insertNotification.TransactionType);
		}

		[Test ()]
		public void RemoveItem_ReturnedItemMatchesByLabel ()
		{
			inventoryService.InsertItem (item1);
			inventoryService.InsertItem (item2);

			Notification removeNotification = inventoryService.RemoveByLabel (item1.Label);

			Assert.AreEqual (item1.Label, removeNotification.Item.Label);
			Assert.AreSame (item1, removeNotification.Item);
			Assert.AreNotSame (item2, removeNotification.Item);
		}

		[Test ()]
		public void RemoveItem_CanOnlyRemoveItemOnce ()
		{
			inventoryService.InsertItem (item1);

			Notification removeNotification1 = inventoryService.RemoveByLabel (item1.Label);
			Notification removeNotification2 = inventoryService.RemoveByLabel (item1.Label);

			Assert.AreSame (item1, removeNotification1.Item);
			Assert.AreNotSame (item1, removeNotification2.Item);
		}

		[Test ()]
		public void RemoveItem_ReturnedNotificationHasTransactionDateOfToday ()
		{
			inventoryService.InsertItem (item1);

			Notification removeNotification = inventoryService.RemoveByLabel (item1.Label);

			Assert.AreEqual (System.DateTime.Now.Year, removeNotification.TransactionDate.Year);
			Assert.AreEqual (System.DateTime.Now.Month, removeNotification.TransactionDate.Month);
			Assert.AreEqual (System.DateTime.Now.Day, removeNotification.TransactionDate.Day);
		}

		[Test ()]
		public void RemoveItem_ReturnedNotificationHasTransactionTypeOfRemoved ()
		{
			inventoryService.InsertItem (item1);

			Notification removeNotification = inventoryService.RemoveByLabel (item1.Label);

			Assert.AreEqual (TransactionType.Removed, removeNotification.TransactionType);
		}

		[Test ()]
		public void GetExpiredItem_ReturnsEmptyListWhenThereAreNoExpiredItems()
		{
			inventoryService.InsertItem (item1);

			var notifications = inventoryService.GetExpiredItemNotifications ().ToList();

			Assert.AreEqual (notifications, new List<Notification>());
		}

		[Test ()]
		public void GetExpiredItem_ReturnsExpiredItemWhenThereIsAnExpiredItem()
		{
			item1.ExpirationDate = System.DateTime.Now.AddDays (1);
			inventoryService.InsertItem (item1);
			TimeUtility.SetDateTimeNow(System.DateTime.Now.AddDays(7));

			var notifications = inventoryService.GetExpiredItemNotifications ().ToList ();

			var listWithOneNotificationForExpiredItem1 = new List<Notification>();
			var notificationForExpiredItem1 = new Notification(item1, TransactionType.Expired);
			listWithOneNotificationForExpiredItem1.Add(notificationForExpiredItem1);

			Assert.AreEqual (notifications.Count, listWithOneNotificationForExpiredItem1.Count);
			Assert.AreSame (notifications [0].Item, listWithOneNotificationForExpiredItem1 [0].Item);
		}

		[Test ()]
		public void GetExpiredItem_ReturnsNotificationWithExpirationType()
		{
			item1.ExpirationDate = System.DateTime.Now.AddDays (1);
			inventoryService.InsertItem (item1);
			TimeUtility.SetDateTimeNow(System.DateTime.Now.AddDays(7));

			var notifications = inventoryService.GetExpiredItemNotifications ().ToList ();

			Assert.AreEqual (notifications[0].TransactionType, TransactionType.Expired);
		}

		[Test ()]
		public void GetExpiredItem_ReturnsNotificationOnlyOnce()
		{
			item1.ExpirationDate = System.DateTime.Now.AddDays (1);
			inventoryService.InsertItem (item1);
			TimeUtility.SetDateTimeNow(System.DateTime.Now.AddDays(7));

			var notifications = inventoryService.GetExpiredItemNotifications ().ToList ();
			var emptyNotifications = inventoryService.GetExpiredItemNotifications ().ToList ();

			Assert.AreEqual (notifications.Count, 1);
			Assert.AreEqual (emptyNotifications, new List<Notification>());
		}
	}
}

