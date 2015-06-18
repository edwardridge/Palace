﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SpecTests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("PlayingFaceDownCards")]
    public partial class PlayingFaceDownCardsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PlayingFaceDownCards.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "PlayingFaceDownCards", "", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot play face down card when you have an in hand card")]
        [NUnit.Framework.CategoryAttribute("mytag")]
        public virtual void CannotPlayFaceDownCardWhenYouHaveAnInHandCard()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot play face down card when you have an in hand card", new string[] {
                        "mytag"});
#line 4
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Player",
                        "CardsInHand",
                        "CardsFaceDown"});
            table1.AddRow(new string[] {
                        "Ed",
                        "ThreeOfClubs",
                        "TwoOfClubs"});
#line 5
 testRunner.Given("I have the following players and cards", ((string)(null)), table1, "Given ");
#line 8
 testRunner.When("\'Ed\' plays the face down card \'TwoOfClubs\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 9
 testRunner.Then("this should not be allowed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot play face down card when you have a face up card")]
        public virtual void CannotPlayFaceDownCardWhenYouHaveAFaceUpCard()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot play face down card when you have a face up card", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Player",
                        "CardsFaceDown",
                        "CardsFaceUp"});
            table2.AddRow(new string[] {
                        "Ed",
                        "TwoOfClubs",
                        "FourOfClubs"});
#line 12
 testRunner.Given("I have the following players and cards", ((string)(null)), table2, "Given ");
#line 15
  testRunner.When("\'Ed\' plays the face down card \'TwoOfClubs\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 16
  testRunner.Then("this should not be allowed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can play face down card when you have no in hand cards")]
        public virtual void CanPlayFaceDownCardWhenYouHaveNoInHandCards()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can play face down card when you have no in hand cards", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Player",
                        "CardsFaceDown"});
            table3.AddRow(new string[] {
                        "Ed",
                        "TwoOfClubs"});
#line 19
 testRunner.Given("I have the following players and cards", ((string)(null)), table3, "Given ");
#line 22
 testRunner.When("\'Ed\' plays the face down card \'TwoOfClubs\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then("\'Ed\' should have \'0\' cards face down", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
