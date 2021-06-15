
# MarketStalker #
Market Stalker is a tool that uses the Warframe.Market API to find the cheapest prices with an optimized, easy to use, and lightweight interface created by me ^_^

I first joined the guild in January 1st of 2019 without knowing C# or really any programming languages and thought 
this would be a great way to show off my knowledge of C#, XAML, and JSON and what I have learned so far.
Thank you for taking your time to open this repository.

This is my submission for the C# guild New Year Competition.
Enjoy.

# Manual: #

![picture alt](https://img.pidgey.software/f/AvailableProblemPopularPart.png)

### Data ###

►  Total Items displays all the items pulled from the WarframeMarket API when the application was started.

► Items Watching shows a total count of the items you  selected to be watched.

### Data Options ###

►  Changing "Minimum Price Decay" will make all items x amount or more show as true in the DataGrid.

► Changing "Maximum Price Decay" will make all items above this number appear as false on the DataGrid.

### Extra Options ###

► Watch All Items will ignore checked items in the Item table and look for every item received by the API.

► Show Non-Matching Items will ignore DataOptions and display items that don't match as False under the "Matches" column.

### Task Options ###

► Recent Listings Parse will go through the most recent listings API results once and display matched results.

► Continuous Search will refresh the most recent listings API every 5 seconds, check for new results and finally display them.

### Items ###

► Column 1 displays checkboxes which you can check to select your wanted items.

► Column 2 displays the item name.

► Column 3 displays the item ID.

### Collected Data ###

Selecting any row and pressing ctrl+c will allow you to copy a clipboard message to send to the seller.

► ID displays digits 5-9 of the order ID.

► Item displays the Item name.

► Price displays the price of the new order.

► 2ND displays the second cheapest listing for that item. This is used to calculate the discount.

► Discount displays the total discount of the item.

► Seller displays the seller name.

► REP displays the reputation of the seller who put up this listing.

► Matches is a True/False that depends on the settings that had been configured in DataOptions.

► Time displays the moment when this listing was put up on Warframe.Market


### Console ###

► Outputs all the found listings with the item, price, and price difference.

# Use Example #

This is how I like to use the application to get the most use out of my time and obtain the best results.

1. Check Watch All Items
2. Click Continuous Search
3. Sort DataGrid by discount
4. Check every couple of minutes to find good deals.
