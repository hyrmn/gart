# gart

Welcome to `gart`; a C# application for creating generative art.

It is still very early in development so expect a lot of changes over the coming week.

I am building out a few generative art algorithms for fun and, since I promised to blog about them, I decided it was high time I find a way to procrastinate and do something else. `gart` is the culmination of that effort. But, more helpfully, it's the UI to run the various art generators.

Built with 💖 using [Terminal.Gui](https://github.com/migueldeicaza/gui.cs).

![Early screenshot of the UI](betascreenshot.jpg)

## Building the application

`gart` is a C# .NET 6.0 console application. From the root of the repository (where `gart.sln` lives; along with this readme), you can run `dotnet build -c Release` to build the application. By default, the resulting executable will be buried under `.\src\bin\Release\net6.0`. That's likely not useful so the next step is to publish the project. 

The following command will publish `gart` to `.\dist`. The path assumes, again, that you will run this from the repository root

```powershell
dotnet publish .\src\gart.csproj -o ./dist --no-self-contained
```

Alternately, you can execute `.\build.ps1` or `.\build.sh` to restore any missing Nuget packages, build the application and then publish the application to `.\dist`

## Contributing

It's a bit early for contributions for others. But, I would highly encourage you to fork `gart` and play around. Do your own thing. And, as always, you can message me on twitter [@hyrmn](https://twitter.com/hyrmn).

## The algorithms

- [Simple Colored Boxes](https://hyr.mn/lets-make-art-pt1)
- more to come