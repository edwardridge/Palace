Feature: Playing one card means you receive one card

Scenario: When you have three cards and play a card you get one card
	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | TenOfClubs, FourOfClubs, AceOfClubs |
	| Liam   | TwoOfClubs, ThreeOfClubs, QueenOfClubs |
	When 'Ed' plays the 'TenOfClubs'
	Then 'Ed' should be have three cards