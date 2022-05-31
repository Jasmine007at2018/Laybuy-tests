Feature: Users
	In order to get the user list
	As a tester
	I want to be able to send a Get request to the Users API


@API @Users
Scenario: Successfully get the user list	
	When I call the GET method from Users api
	Then I should get a response with Status code 200
	And  I should get 10 records returned
	And  The user 'Mrs. Dennis Schulist' is contained in the list
	And  I should be able to retrieve data for any single user by valid userId between 1 and 10
	And  I should be able to retrieve data for certain user by valid userId
		| validUserId | userName       |
		| 1			  | Bret           |
		| 5			  | Kamren         |
		| 10		  | Moriah.Stanton |			
	And I should get a response with Status code 404 by invalid userId
		| invalidUserId | 		
		| 11			| 
		| 0			    | 
		| -1			| 
	
 
	
