Feature: Who should start

Scenario: Player with lowest card starts
	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | TenOfClubs, FourOfClubs, AceOfClubs |
	| Liam   | TwoOfClubs, ThreeOfClubs, QueenOfClubs |
	When The game starts
	Then it should be 'Liam' turn
