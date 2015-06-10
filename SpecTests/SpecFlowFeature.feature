Feature: Playing one card means you receive one card

Scenario: Add two numbers
	Given I have two players, called Ed and Sophie, with three cards each in a game in progress
	When Ed plays a card
	Then Ed should be given a card from the deck
		