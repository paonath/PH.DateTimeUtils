# PH.DateTimeUtils - Utility related to Date and Times

## Week 

Work with weeks.

### Calculate Start and End DateTime from a week number

```csharp
var firstWeek = PH.DateTimeUtils.Weeks.GetStartAndEndDateTimeFromWeekNumber(2023, 1);

Console.WriteLine(firstWeek.Start); // the output is 2023-01-02
Console.WriteLine(firstWeek.End);   // the output is 2023-01-08

```
