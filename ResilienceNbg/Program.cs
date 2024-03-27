// See https://aka.ms/new-console-template for more information
using ResilienceNbg.Services;

Console.WriteLine("Hello, World!");


var typicalConnection = new TypicalConnection();
await typicalConnection.DoSomethingAsync();
