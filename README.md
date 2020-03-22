# Welcome to SUnit 
A .NET unit testing framework.

## What is SUnit?
SUnit is a .NET unit testing framework with a highly-fluent syntax.
```
    public Test MyUnitTest() 
    {
        return Assert.That(2 + 2).Is.EqualTo(4);
    }
```
The first thing you'll notice about SUnit tests is that the test methods have a return-type of `Test` rather than returning `void`. The main advantage of this is that it makes it impossible to write an empty unit test that always passes. 
```
    public Test SomeoneForgotToWriteThisTest() 
    {
        //  And it won't compile until they write it. 
    }
```
Tests can also be combined using boolean operations.
```
    public Test ThrowsWhenNull() 
    {
        return Assert.That(list).Is.Not.Null &
            Assert.That(list.First()).Is.EqualTo(77);
    }
```
Both eager and lazy forms of the logical operators are supported.
```
    public Test CleanlyFailsTestWhenNull()
    {
        return Assert.That(list).Is.Not.Null &&
            Assert.That(list.First()).Is.EqualTo(78);
    }
```
If you want to apply multiple assertions to the same value, you can use this fluent syntax as well.
```
    public Test FluentBooleanOperations() 
    {
        return Assert.That(7).Is.LessThan(8).And.Is.GreaterThan(6);
    }
```
SUnit also supports theory-style tests. Simply have the test method return an `IEnumerable<Test>` and SUnit will run them all. 
```
    public IEnumerable<Test> FrobbersDoNotFrib()
    {
        return frobber.Foos
            .Where(foo => foo.IsFrobbingFoo)
            .Select(foo => Assert.That(foo.IsFribbingFoo).Is.False);
    }
```
SUnit also supports async tests. Simply return a `Task<Test>` from your test method.
```
    public async Task<Test> DoesTheLaggyDatabaseWork()
    {
        var result = await laggyDB.GetLaggyFrobbingFoos();
        return Assert.That(result).Is.SetEqualTo("Alpha", "Charlie", "Hotel");
    }
```

## Getting Started
Before writing unit tests in SUnit, you'll need to install the SUnit packages from Nuget. We recommend using our Visual Studio extension. It comes with Visual Studio project templates that take care of importing all the dependancies for you. You can [download the Visual Studio SUnit Extension](https://marketplace.visualstudio.com/items?itemName=NewellClark.SUnitTemplates).

The extension includes project templates for creating SUnit projects. 
