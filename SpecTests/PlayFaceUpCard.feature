Feature: PlayFaceUpCard

Scenario: Cannot play face up cards when player has 3 in hand cards
	Given I have the following players and cards
		 | Player | CardsInHand								 | CardsFaceUp |
         | Ed     | TwoOfClubs, ThreeOfClubs, FourOfClubs    | FiveOfClubs |
	When 'Ed' plays the face up card 'FiveOfClubs'
	Then this should not be allowed

Scenario: Cann play face up cards when player has 2 in hand cards
	Given I have the following players and cards
		 | Player | CardsInHand					| CardsFaceUp |
         | Ed     | TwoOfClubs, ThreeOfClubs    | FiveOfClubs |
	When 'Ed' plays the face up card 'FiveOfClubs'
	Then this should be allowed
	And 'Ed' should have '0' cards face up
