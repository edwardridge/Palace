Feature: GameOverTests

Scenario: When player plays their last card they win
	Given I have the following players and cards
	| Player | CardsFaceDown |
	| Ed     | TwoOfClubs    |
	| Liam   | ThreeOfClubs  |
	And it is 'Ed' turn
	When 'Ed' plays the face down card 'TwoOfClubs'
	Then the game is over
