Feature: PlayingFaceDownCards

@mytag
Scenario: Cannot play face down card when you have an in hand card
	Given I have the following players and cards
	| Player | CardsInHand  | CardsFaceDown |
	| Ed     | ThreeOfClubs | TwoOfClubs    |
	When 'Ed' plays the face down card 'TwoOfClubs'
	Then this should not be allowed

Scenario: Cannot play face down card when you have a face up card
	Given I have the following players and cards
         | Player | CardsFaceDown | CardsFaceUp |
         | Ed     | TwoOfClubs    | FourOfClubs |
		When 'Ed' plays the face down card 'TwoOfClubs'
		Then this should not be allowed

Scenario: Can play face down card when you have no in hand cards
	Given I have the following players and cards
	| Player  | CardsFaceDown |
	| Ed      | TwoOfClubs    |
	When 'Ed' plays the face down card 'TwoOfClubs'
	Then 'Ed' should have '0' cards face down