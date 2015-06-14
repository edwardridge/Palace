Feature: PlayingFaceDownCards

@mytag
Scenario: Cannot play face down card when you have an in hand card
	Given I have the following players and cards
	| Player | CardsInHand  | CardsFaceDown |
	| Ed     | ThreeOfClubs | TwoOfClubs    |
	When 'Ed' plays the face down card 'TwoOfClubs'
	Then this should not be allowed

	Given I have the following players and cards
	| Player | CardsFaceDown |
	| Ed      | TwoOfClubs    |
	When 'Ed' plays the face down card 'TwoOfClubs'
	Then this should be allowed
