# FoundryLocal-Playground
Ceci est un exemple d'utilisation de FoundryLocal.
Pour plus de renseignements, rendez-vous sur le site [devdevdev.net](https://devdevdev.net) et la vidéo consacrée sur Youtube.

[![Miniature YouTube](https://img.youtube.com/vi/jKUDknKGPqc/0.jpg)](https://youtu.be/jKUDknKGPqc)

[Regarder la vidéo sur YouTube](https://youtu.be/jKUDknKGPqc)

# Pour installer FoundryLocal : 
```bash
winget install Microsoft.FoundryLocal
````
# Comparaison avec Ollama
Si vous avez Ollama d'installer, vous pouvez faire la comparaison à l'exécution.
Dans le fichier UseFoundryOllama, vous pouvez paramètrer les modèles que vous souhaitez utiliser.

```csharp
private readonly string _foundryLocalModel = "phi-4";
private readonly string _ollamaModel = "mistral";
private readonly Uri _ollamaUri = new Uri("http://localhost:11434/");
```
Si vous voulez simuler plusieurs appels en même temps, modifiez le paramètres _iterations :
```csharp
private readonly int[] _iterations = [0]; // [0, 1, 2, 3];
```
Enjoy!

