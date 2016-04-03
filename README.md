# valant-inventory
A generic inventory API
By David Nayyar 2016

	Overview: 
		The Valant Inventory API is a simple, generic inventory system. Items can be added or removed one at a
	time. Each action in the API returns a Notification containing the relevant item, a transaction type, and a
	transaction date. Items can have an expiration date, and the API can be polled for notifications about expired
	items. 

	How to run application:
		At present, there is no UI component for the API. There is a suite of NUnit tests to test the API’s
	functionality. The API compiles as a class library and could easily be integrated into any UI or hosted web
	service.

	Design:
		The API consists of a single class named “InventoryApi” which implements an interface called IInventoryApi.
	The interface could be reference by future UIs to allow for dependency injection and makes the service mockable for
	unit tests.
		The methods of this service return a Notification object, or a collection of Notification objects. Each
	Notification contains an Item object. These classes are defined in a library called “ValantData” in a separate
	namespace from the API. This library would need to be shared with any UI, or these classes would need to be mapped
	to equivalent classes in the UI.
		In lieu of any persistent data storage, the existing items are maintained in an IEnumerable collection of
	Item objects, which is a private member of the InventoryApi class.
		There is an additional class in the ValantData namespace called “TimeUtility” to aid in unit testing
	functionality around System.DateTime.Now.

	Assumptions:
		- Items contain an optional Id that acts as a unique identifier. This value is set when an item is first
	entered.
		- An item contains a Label, which is an integer value. An item cannot be entered into Inventory it shares a
	Label with an existing item.
		- Items contain an optional ExpirationDate. If an item’s ExpirationDate is past the current date, it cannot
	reinserted.
		- When the method GetExpiredItemNotifications() is called, it will remove all expired items from the
	inventory and return a collection of Notifications with the expired items in it.
		- The Api doesn’t have a true “push” capability. So any implementations will have call to
	GetExpiredItemNotifications() routinely. This could be tied to a library such as SignalR to simulate a “push”
	architecture. 
