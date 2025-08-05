using System.ClientModel;
using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.AI;
using OllamaSharp;
using OpenAI;
using Spectre.Console;

namespace FoundryLocalPlayground;

public class UseFoundryOllama : IUse
{
    private readonly string _alias = "phi-4";
    private readonly string _ollamaModelId = "mistral";
    private readonly Uri _ollamaUri = new Uri("http://localhost:11434/");
    
    private readonly int[] _iterations = [0]; // [0, 1, 2, 3];
    public async Task ExecuteAsync()
    {
        // choix du client de chat
        var selectedClient = AnsiConsole.Prompt(
            new SelectionPrompt<ChatClientType>()
                .Title("[green]Choisissez le type de client de chat :[/]")
                .AddChoices(Enum.GetValues<ChatClientType>()));

        AnsiConsole.MarkupLine($"[green]Excellent choix ! Vous avez choisi : {selectedClient}[/]");

        var createChatClient = selectedClient switch
        {
            ChatClientType.FoundryLocal => await CreateFoundryLocalChatClientAsync(),
            ChatClientType.Ollama => CreateOllamaChatClient(),
            _ => throw new ArgumentOutOfRangeException(nameof(selectedClient), "ClientType inconnu")
        };

        var topic = "Ecris un poème de 15 alexandrin sur le Pays Basque en français";
        var systemPrompt = "Tu génères un texte basé sur la demande de l'utilisateur. Réponds et français uniquement avec le contenu généré sans commentaires superflus.";
        var userPrompt = "Génère le texte basé sur la demande suivante : " + topic;
        
        AnsiConsole.MarkupLine($"[yellow]Démarrage du test avec le client : {selectedClient} pour {_iterations.Length} itération(s)[/]");
        await Parallel.ForEachAsync(_iterations, async (i, _) =>
            {
                var cts = new CancellationTokenSource();
                using var chatClient = createChatClient();
                await foreach (var messagePart in chatClient.GetStreamingResponseAsync(
                                   [
                                       new ChatMessage(ChatRole.System, systemPrompt),
                                   new ChatMessage(ChatRole.User, userPrompt)
                                   ], new ChatOptions
                                   {
                                       MaxOutputTokens = 1000
                                   },
                                   cts.Token))
                {
                    var color = i switch
                    {
                        0 => "blue",
                        1 => "yellow",
                        2 => "green",
                        _ => "red"
                    };
                    AnsiConsole.Markup($"[{color}]{messagePart}[/]");
                }
            });
    }

    private async Task<Func<IChatClient>> CreateFoundryLocalChatClientAsync()
    {
        AnsiConsole.MarkupLine("[yellow]Démarrage de FoundryLocal[/]");
        var manager = await FoundryLocalManager.StartModelAsync(_alias);

        AnsiConsole.MarkupLine($"[green]FoundryLocal démarré avec succès pour le modèle : {_alias}[/]");
        // Infos sur les modèles du cache
        var cacheFolder = await manager.GetCacheLocationAsync();
        var table = new Table
                { Title = new TableTitle($"Modèles dans le cache ({cacheFolder})", new Style(Color.Green)) }
            .AddColumn(new TableColumn("[yellow]Alias[/]"))
            .AddColumn(new TableColumn("[yellow]DisplayName[/]"))
            .AddColumn(new TableColumn("[yellow]File Size[/]"))
            .AddColumn(new TableColumn("[yellow]Tools[/]"))
            .AddColumn(new TableColumn("[yellow]Licence[/]"));

        var models = await manager.ListCachedModelsAsync();
        foreach (var modelInfo in models)
            table.AddRow(modelInfo.Alias, modelInfo.DisplayName, modelInfo.FileSizeMb.ToString(), modelInfo.SupportsToolCalling.ToString(), modelInfo.License);
        AnsiConsole.Write(table);
        

        // Infos sur les paramètres de l'API
        var model = await manager.GetModelInfoAsync(_alias);
        if (model == null) throw new ArgumentException("Model non trouvé");

        table = new Table
                { Title = new TableTitle("OpenAPI API infos", new Style(Color.Green)) }
            .AddColumn(new TableColumn("[yellow]API Key[/]"))
            .AddColumn(new TableColumn("[yellow]EndPoint[/]"));
        table.AddRow(manager.ApiKey, manager.Endpoint.ToString());
        AnsiConsole.Write(table);

        AnsiConsole.Write(new Rule().RuleStyle("darkgreen"));
        return () => new OpenAIClient(
                new ApiKeyCredential(manager.ApiKey),
                new OpenAIClientOptions { Endpoint = manager.Endpoint })
            .GetChatClient(model.ModelId)
            .AsIChatClient();
    }

    private Func<IChatClient> CreateOllamaChatClient()
    {
        return () => new OllamaApiClient(_ollamaUri, _ollamaModelId);
    }
}