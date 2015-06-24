Feature: GameOverTests

Scenario: When player plays their last card they win
	Given I have the following players and cards
	| Player | CardsFaceDown |
	| Ed     | TwoOfClubs    |
	| Liam   | ThreeOfClubs  |
	And it is 'Ed' turn
	When 'Ed' plays the face down card 'TwoOfClubs'
	Then the game is over
	And 'Ed' has won


Scenario: When the game is over you and a player plays a card you are notified the game is over
	Given I have the following players and cards
	| Player | CardsFaceDown |
	| Ed     | TwoOfClubs    |
	| Liam   | ThreeOfClubs, FourOfClubs  |
	And it is 'Ed' turn
	When 'Ed' plays the face down card 'TwoOfClubs'
	And 'Liam' plays the face down card 'ThreeOfClubs'
	Then you are notified the game is over
	And 'Ed' has won

Scenario: When the game is over and a player plays a card that card isn't removed
	Given I have the following players and cards
	| Player | CardsFaceDown |
	| Ed     | TwoOfClubs    |
	| Liam   | ThreeOfClubs  |
	And it is 'Ed' turn
	When 'Ed' plays the face down card 'TwoOfClubs'
	And 'Liam' plays the face down card 'ThreeOfClubs'
	Then 'Liam' should have '1' cards face down
