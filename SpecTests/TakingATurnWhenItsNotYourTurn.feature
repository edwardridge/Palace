Feature: TakingATurnWhenItsNotYourTurn
	Trying to take a turn when it isn't your turn

@mytag
Scenario: Trying to take a turn when it isn't your turn
	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | TenOfClubs, FourOfClubs, AceOfClubs |
	| Liam   | TwoOfClubs, ThreeOfClubs, QueenOfClubs |
	And it is 'Ed' turn
	When 'Liam' plays the 'TwoOfClubs'
	Then this should not be allowed
