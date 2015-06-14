Feature: FullGameTests

@mytag
Scenario: Full game test
	Given I have the following players and cards
	| Player | CardsInHand                           | CardsFaceUp                          | CardsFaceDown                          |
	| Ed     | ThreeOfClubs, FourOfClubs, AceOfClubs | JackOfClubs, FourOfClubs, EightOfClubs  | QueenOfClubs, TenOfClubs, FiveOfClubs  |
	| Liam   | TwoOfClubs, FourOfClubs, QueenOfClubs | SevenOfClubs, TwoOfClubs, TenOfClubs | EightOfClubs, NineOfClubs, FourOfClubs |
	And the following cards have rules
	| CardValue | Rule  |
	| Two       | Reset |
	And the deck has no more cards
	When 'Ed' plays the 'ThreeOfClubs'
	And 'Liam' plays the 'FourOfClubs'
	And 'Ed' plays the 'FourOfClubs'
	And 'Liam' plays the 'QueenOfClubs'
	And 'Ed' plays the 'AceOfClubs'
	And 'Liam' plays the face up card 'TwoOfClubs'
	And 'Ed' plays the face up card 'FourOfClubs'
	And 'Liam' plays the face up card 'SevenOfClubs'
	And 'Ed' plays the face up card 'EightOfClubs'
	And 'Liam' plays the face up card 'TenOfClubs'
	And 'Ed' plays the face up card 'JackOfClubs'
	And 'Liam' plays the 'TwoOfClubs'
	And 'Ed' plays the face down card 'FiveOfClubs'
	Then this should be allowed
