Feature: GameplayTests

Scenario: Playing an in hand card adds that card to the play pile, change whose turn it is and the player should get a card
	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | TwoOfClubs, FourOfClubs, AceOfClubs |
	| Liam   | ThreeOfClubs, FourOfClubs, QueenOfClubs |
	And it is 'Ed' turn
	When 'Ed' plays the 'TwoOfClubs'
	Then the number of cards in the play pile should be '1'
	And it should be 'Liam' turn
	And 'Ed' should have '3' cards in hand

Scenario: Playing two in hand card adds those cards to the play pile, changes whose turn it is and the players should get a card

	Given I have the following players and cards
	| Player | CardsInHand                         |
	| Ed     | TwoOfClubs, FourOfClubs, AceOfClubs |
	| Liam   | ThreeOfClubs, FourOfClubs, QueenOfClubs |
	And it is 'Ed' turn
	When 'Ed' plays the 'TwoOfClubs'
	And 'Liam' plays the 'ThreeOfClubs'
	Then the number of cards in the play pile should be '2'
	And it should be 'Ed' turn
	And 'Liam' should have '3' cards in hand

Scenario: When four cards of the same value are played in different turns, the play pile is burnt and the last player gets another turn
Given I have the following players and cards
| Player | CardsInHand            |
| Ed     | TwoOfClubs, TwoOfClubs, ThreeOfClubs |
| Liam   | TwoOfClubs, TwoOfClubs, ThreeOfClubs |
And it is 'Ed' turn
When 'Ed' plays the 'TwoOfClubs'
And 'Liam' plays the 'TwoOfClubs'
And 'Ed' plays the 'TwoOfClubs'
And 'Liam' plays the 'TwoOfClubs'
Then this should be allowed
And it should be 'Liam' turn