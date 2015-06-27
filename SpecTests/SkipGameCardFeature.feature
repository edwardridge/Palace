Feature: SkipGameCardFeature

@bugtest
Scenario: Playing a skip card with non skip cards in the play pile should only skip one player
	Given I have the following players and cards
    | Player | CardsInHand               |
    | Ed     | ThreeOfClubs, FiveOfClubs |
    | Liam   | FourOfClubs               |
    | Dave   | FourOfClubs               |
	And it is 'Liam' turn
	And the following cards have rules
	| CardValue   | Rule |
	| Five | SkipPlayer |
	When 'Liam' plays the 'FourOfClubs'
	And 'Dave' plays the 'FourOfClubs'
	And 'Ed' plays the 'FiveOfClubs'
	Then it should be 'Dave' turn

@bugtest
Scenario: Playing two skip cards with non skip cards in play pile should skip two players
	Given I have the following players and cards
    | Player | CardsInHand               |
    | Ed     | ThreeOfClubs, FiveOfClubs, FiveOfClubs |
    | Liam   | FourOfClubs               |
    | Dave   | FourOfClubs               |
	
	And the following cards have rules
	| CardValue   | Rule |
	| Five | SkipPlayer |
	And it is 'Liam' turn
	When 'Liam' plays the 'FourOfClubs'
	And 'Dave' plays the 'FourOfClubs'
	And 'Ed' plays the 'FiveOfClubs, FiveOfClubs'
	Then it should be 'Ed' turn

Scenario: PLaying one skip card after a previous player plays a skip card should only skip one player
Given I have the following players and cards
    | Player | CardsInHand               |
    | Ed     | FiveOfClubs |
    | Liam   | FourOfClubs               |
    | Dave   | FiveOfClubs               |
	And the following cards have rules
	| CardValue | Rule       |
	| Five      | SkipPlayer |
	And it is 'Ed' turn
	When 'Ed' plays the 'FiveOfClubs'
	And 'Dave' plays the 'FiveOfClubs'
	Then it should be 'Liam' turn