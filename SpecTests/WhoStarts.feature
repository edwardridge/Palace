Feature: Who should start

Scenario: Player with lowest card starts - lowest card two of clubs
	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | TenOfClubs, FourOfClubs, AceOfClubs |
	| Liam   | TwoOfClubs, ThreeOfClubs, QueenOfClubs |
	When The game starts
	Then it should be 'Liam' turn

Scenario: Player with lowest card starts - lowest card three of clubs
	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | ThreeOfClubs, FourOfClubs, AceOfClubs |
	| Liam   | FourOfClubs, FiveOfClubs, QueenOfClubs |
	When The game starts
	Then it should be 'Ed' turn

Scenario: Player with lowest card starts - lowest card seven of clubs
	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | EightOfClubs, NineOfClubs, AceOfClubs |
	| Liam   | SevenOfClubs, TenOfClubs, QueenOfClubs |
	When The game starts
	Then it should be 'Liam' turn

Scenario: Player with lowest card starts - with three players, lowest card three of clubs
	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | EightOfClubs, NineOfClubs, AceOfClubs |
	| Liam   | SevenOfClubs, TenOfClubs, QueenOfClubs |
	| David   | ThreeOfClubs, KingOfClubs, QueenOfClubs |
	When The game starts
	Then it should be 'David' turn
