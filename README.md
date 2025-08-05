# FoundryLocal-Playground
Ceci est un exemple d'utilisation de FoundryLocal.
Pour plus de renseignements, rendez-vous sur le site [devdevdev.net](https://devdevdev.net) et la vid�o consacr�e sur Youtube.

[![Miniature YouTube](https://img.youtube.com/vi/jKUDknKGPqc/0.jpg)](https://youtu.be/jKUDknKGPqc)

[Regarder la vid�o sur YouTube](https://youtu.be/jKUDknKGPqc)

# Pour installer FoundryLocal : 
```bash
winget install Microsoft.FoundryLocal
````
# Comparaison avec Ollama
Si vous avez Ollama d'installer, vous pouvez faire la comparaison � l'ex�cution.
Dans le fichier UseFoundryOllama, vous pouvez param�trer les mod�les que vous souhaitez utiliser.

```csharp
private readonly string _foundryLocalModel = "phi-4";
private readonly string _ollamaModel = "mistral";
private readonly Uri _ollamaUri = new Uri("http://localhost:11434/");
```
Si vous voulez simuler plusieurs appels en m�me temps, modifiez le param�tres _iterations :
```csharp
private readonly int[] _iterations = [0]; // [0, 1, 2, 3];
```
Enjoy!

