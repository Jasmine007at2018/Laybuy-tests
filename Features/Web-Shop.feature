Feature: Shop


@Web @Shop
Scenario: Search and navigate from the Shop
	Given I am on Laybuy website
	When I search in the Shop with the keyword 'Heart of Oxford'
	Then I should see at least 2 shop directory tiles
	And  Any tile should navigate to the correct merchant wbsite
