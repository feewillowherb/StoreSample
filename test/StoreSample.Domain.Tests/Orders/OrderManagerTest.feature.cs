﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace StoreSample.Orders
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("OrderManagerTest")]
    [NUnit.Framework.FixtureLifeCycleAttribute(NUnit.Framework.LifeCycle.InstancePerTestCase)]
    public partial class OrderManagerTestFeature
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Orders", "OrderManagerTest", null, global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
#line 1 "OrderManagerTest.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public static async System.Threading.Tasks.Task FeatureSetupAsync()
        {
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(featureHint: featureInfo);
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Equals(featureInfo) == false)))
            {
                await testRunner.OnFeatureEndAsync();
            }
            if ((testRunner.FeatureContext == null))
            {
                await testRunner.OnFeatureStartAsync(featureInfo);
            }
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
            global::Reqnroll.TestRunnerManager.ReleaseTestRunner(testRunner);
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        public virtual async System.Threading.Tasks.Task FeatureBackgroundAsync()
        {
#line 3
    #line hidden
#line 4
        await testRunner.GivenAsync("Now is 2023-10-01", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 5
        await testRunner.GivenAsync("Throwing an exception", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
            global::Reqnroll.Table table1 = new global::Reqnroll.Table(new string[] {
                        "Name",
                        "Cost"});
            table1.AddRow(new string[] {
                        "product1",
                        "20"});
            table1.AddRow(new string[] {
                        "product2",
                        "12"});
#line 6
        await testRunner.AndAsync("Products as below", ((string)(null)), table1, "And ");
#line hidden
            global::Reqnroll.Table table2 = new global::Reqnroll.Table(new string[] {
                        "UserName",
                        "Value"});
            table2.AddRow(new string[] {
                        "user1",
                        "50"});
            table2.AddRow(new string[] {
                        "user2",
                        "80"});
#line 10
        await testRunner.AndAsync("Assets as below", ((string)(null)), table2, "And ");
#line hidden
#line 14
        await testRunner.AndAsync("Start harness", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Create order")]
        public async System.Threading.Tasks.Task CreateOrder()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Create order", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 16
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
    await this.FeatureBackgroundAsync();
#line hidden
                global::Reqnroll.Table table3 = new global::Reqnroll.Table(new string[] {
                            "UserName",
                            "ProductName",
                            "Quantity"});
                table3.AddRow(new string[] {
                            "user1",
                            "product1",
                            "2"});
#line 17
        await testRunner.WhenAsync("Create order as below", ((string)(null)), table3, "When ");
#line hidden
                global::Reqnroll.Table table4 = new global::Reqnroll.Table(new string[] {
                            "UserName",
                            "Amount",
                            "Status"});
                table4.AddRow(new string[] {
                            "user1",
                            "40",
                            "Created"});
#line 20
        await testRunner.ThenAsync("Order as below", ((string)(null)), table4, "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Order pending")]
        public async System.Threading.Tasks.Task OrderPending()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Order pending", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 24
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
    await this.FeatureBackgroundAsync();
#line hidden
                global::Reqnroll.Table table5 = new global::Reqnroll.Table(new string[] {
                            "UserName",
                            "ProductName",
                            "Quantity"});
                table5.AddRow(new string[] {
                            "user1",
                            "product1",
                            "2"});
#line 25
        await testRunner.WhenAsync("Create order as below", ((string)(null)), table5, "When ");
#line hidden
                global::Reqnroll.Table table6 = new global::Reqnroll.Table(new string[] {
                            "UserName"});
                table6.AddRow(new string[] {
                            "user1"});
#line 28
        await testRunner.WhenAsync("Create payment as below", ((string)(null)), table6, "When ");
#line hidden
                global::Reqnroll.Table table7 = new global::Reqnroll.Table(new string[] {
                            "UserName",
                            "Amount",
                            "Status"});
                table7.AddRow(new string[] {
                            "user1",
                            "40",
                            "Pending"});
#line 31
        await testRunner.ThenAsync("Order as below", ((string)(null)), table7, "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Order paid")]
        public async System.Threading.Tasks.Task OrderPaid()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Order paid", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 35
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
    await this.FeatureBackgroundAsync();
#line hidden
                global::Reqnroll.Table table8 = new global::Reqnroll.Table(new string[] {
                            "UserName",
                            "ProductName",
                            "Quantity"});
                table8.AddRow(new string[] {
                            "user1",
                            "product1",
                            "2"});
#line 36
        await testRunner.WhenAsync("Create order as below", ((string)(null)), table8, "When ");
#line hidden
                global::Reqnroll.Table table9 = new global::Reqnroll.Table(new string[] {
                            "UserName"});
                table9.AddRow(new string[] {
                            "user1"});
#line 39
        await testRunner.WhenAsync("Create payment as below", ((string)(null)), table9, "When ");
#line hidden
#line 42
        await testRunner.WhenAsync("Run Consumer", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
                global::Reqnroll.Table table10 = new global::Reqnroll.Table(new string[] {
                            "UserName",
                            "Value"});
                table10.AddRow(new string[] {
                            "user1",
                            "10"});
#line 43
        await testRunner.ThenAsync("Asset as below", ((string)(null)), table10, "Then ");
#line hidden
#line 46
        await testRunner.WhenAsync("Run Consumer", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
                global::Reqnroll.Table table11 = new global::Reqnroll.Table(new string[] {
                            "UserName",
                            "Amount",
                            "Status"});
                table11.AddRow(new string[] {
                            "user1",
                            "40",
                            "Paid"});
#line 47
        await testRunner.ThenAsync("Order as below", ((string)(null)), table11, "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Insufficient balance")]
        public async System.Threading.Tasks.Task InsufficientBalance()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Insufficient balance", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 51
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 3
    await this.FeatureBackgroundAsync();
#line hidden
#line 52
        await testRunner.GivenAsync("Not throwing an exception", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
                global::Reqnroll.Table table12 = new global::Reqnroll.Table(new string[] {
                            "UserName",
                            "ProductName",
                            "Quantity"});
                table12.AddRow(new string[] {
                            "user1",
                            "product1",
                            "3"});
#line 53
        await testRunner.WhenAsync("Create order as below", ((string)(null)), table12, "When ");
#line hidden
                global::Reqnroll.Table table13 = new global::Reqnroll.Table(new string[] {
                            "UserName"});
                table13.AddRow(new string[] {
                            "user1"});
#line 56
        await testRunner.WhenAsync("Create payment as below", ((string)(null)), table13, "When ");
#line hidden
#line 59
        await testRunner.ThenAsync("Exception is \"InsufficientBalance\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
