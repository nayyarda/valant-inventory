using System;

namespace ValantData
{
	public class Notification
	{
		public Notification (Item item, TransactionType transactionType)
		{
			Item = item;
			TransactionDate = TimeUtility.Now;
			TransactionType = transactionType;
		}

		public Item Item {
			get;
			set;
		}

		public DateTime TransactionDate {
			get;
			set;
		}

		public TransactionType TransactionType {
			get;
			set;
		}
	}
}

