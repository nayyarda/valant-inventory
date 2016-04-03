using System;

namespace ValantData
{
	public class Item
	{
		public Item ()
		{
			//expirationNotificationSent = false;
		}

		public int? Id {
			get;
			set;
		}

		public string Description {
			get;
			set;
		}

		public int Label {
			get;
			set;
		}

		public ItemType Type {
			get;
			set;
		}

		public DateTime? EnteredDate {
			get;
			set;
		}

		public DateTime? ExpirationDate {
			get;
			set;
		}

		//private Boolean expirationNotificationSent;
	}
}

