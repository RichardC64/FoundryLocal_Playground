using FoundryLocalPlayground;
using Spectre.Console;

var selectedUse = AnsiConsole.Prompt(
    new SelectionPrompt<Uses>()
        .Title("[green]Choisissez le test :[/]")
        .AddChoices( Uses.FoundryOllama));

IUse use = selectedUse switch
{
    Uses.FoundryOllama => new UseFoundryOllama(),
    _ => throw new ArgumentOutOfRangeException(nameof(selectedUse), selectedUse, null)
};

await use.ExecuteAsync();
