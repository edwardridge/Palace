Feature: Playing one card means you receive one card

Scenario: Add two numbers
	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | TenOfClubs, FourOfClubs, AceOfClubs |
	When 'Ed' plays the 'TenOfClubs'
	Then 'Ed' should be have three cards
		