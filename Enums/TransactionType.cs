using System;

namespace ValantData
{
	public enum TransactionType
	{
		Removed = 1,
		Expired = 2,
		Inserted = 3,
		FailedToInsert = 4,
		FailedToRemove = 5
	}
}

