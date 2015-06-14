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
    [NUnit.Framework.DescriptionAttribute("Who should start")]
    public partial class WhoShouldStartFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "WhoStarts.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Who should start", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Player with lowest card starts - lowest card two of clubs")]
        public virtual void PlayerWithLowestCardStarts_LowestCardTwoOfClubs()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Player with lowest card starts - lowest card two of clubs", ((string[])(null)));
#line 3
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Player",
                        "CardsInHand"});
            table1.AddRow(new string[] {
                        "Ed",
                        "TenOfClubs, FourOfClubs, AceOfClubs"});
            table1.AddRow(new string[] {
                        "Liam",
                        "TwoOfClubs, ThreeOfClubs, QueenOfClubs"});
#line 4
 testRunner.Given("I have the following players and cards", ((string)(null)), table1, "Given ");
#line 8
 testRunner.When("The game starts", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 9
 testRunner.Then("it should be \'Liam\' turn", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Player with lowest card starts - lowest card three of clubs")]
        public virtual void PlayerWithLowestCardStarts_LowestCardThreeOfClubs()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Player with lowest card starts - lowest card three of clubs", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Player",
                        "CardsInHand"});
            table2.AddRow(new string[] {
                        "Ed",
                        "ThreeOfClubs, FourOfClubs, AceOfClubs"});
            table2.AddRow(new string[] {
                        "Liam",
                        "FourOfClubs, FiveOfClubs, QueenOfClubs"});
#line 12
 testRunner.Given("I have the following players and cards", ((string)(null)), table2, "Given ");
#line 16
 testRunner.When("The game starts", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
 testRunner.Then("it should be \'Ed\' turn", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Player with lowest card starts - lowest card seven of clubs")]
        public virtual void PlayerWithLowestCardStarts_LowestCardSevenOfClubs()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Player with lowest card starts - lowest card seven of clubs", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Player",
                        "CardsInHand"});
            table3.AddRow(new string[] {
                        "Ed",
                        "EightOfClubs, NineOfClubs, AceOfClubs"});
            table3.AddRow(new string[] {
                        "Liam",
                        "SevenOfClubs, TenOfClubs, QueenOfClubs"});
#line 20
 testRunner.Given("I have the following players and cards", ((string)(null)), table3, "Given ");
#line 24
 testRunner.When("The game starts", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
 testRunner.Then("it should be \'Liam\' turn", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Player with lowest card starts - with three players, lowest card three of clubs")]
        public virtual void PlayerWithLowestCardStarts_WithThreePlayersLowestCardThreeOfClubs()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Player with lowest card starts - with three players, lowest card three of clubs", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Player",
                        "CardsInHand"});
            table4.AddRow(new string[] {
                        "Ed",
                        "EightOfClubs, NineOfClubs, AceOfClubs"});
            table4.AddRow(new string[] {
                        "Liam",
                        "SevenOfClubs, TenOfClubs, QueenOfClubs"});
            table4.AddRow(new string[] {
                        "David",
                        "ThreeOfClubs, KingOfClubs, QueenOfClubs"});
#line 28
 testRunner.Given("I have the following players and cards", ((string)(null)), table4, "Given ");
#line 33
 testRunner.When("The game starts", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 34
 testRunner.Then("it should be \'David\' turn", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
