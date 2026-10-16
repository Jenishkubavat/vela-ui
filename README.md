# Vela UI

Vela UI is a component library for Blazor that you install as source code.
Instead of referencing a package of sealed components, you run the `vela`
CLI to copy a component's Razor and C# files into your own project. After
that, the code is yours. You can read it, restyle it, and change its
behavior without waiting on a release or working around an API.

The idea comes from [shadcn/ui](https://ui.shadcn.com/). This is an
independent Blazor implementation, not a port.

## Status

Vela UI is in early development and there is nothing to install yet. The
CLI currently prints its help and version, and nothing more.

The first milestone is `vela init` and `vela add` for Button, Input, Label,
and Card, with light and dark themes driven by CSS variables. Dialog,
Dropdown Menu, and Select come after that, once keyboard and focus
behavior can be tested in a real browser.

## Building from source

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```bash
dotnet build
dotnet test
dotnet run --project src/Vela.UI.Cli -- --help
```

To run one test:

```bash
dotnet test --filter "FullyQualifiedName~VelaCliTests.Version_is_a_semantic_version_without_build_metadata"
```

The build treats warnings as errors and enforces the code style in
`.editorconfig`, so fix a warning rather than suppressing it.

NuGet package versions are managed centrally in `Directory.Packages.props`.
To add a package, add a `PackageVersion` entry there and a
`PackageReference` without a version in the project file.

## Repository layout

| Path | Contents |
| --- | --- |
| `src/Vela.UI.Cli/` | The `vela` command-line tool |
| `tests/Vela.UI.Cli.Tests/` | Tests for the CLI |
| `Directory.Build.props` | Version number and settings shared by every project |
| `Directory.Packages.props` | NuGet package versions for every project |
| `NuGet.config` | Restricts package restore to nuget.org |

## Versioning

Vela UI uses [Semantic Versioning](https://semver.org/). The version is set
once, in `Directory.Build.props`. Until 1.0, a minor release may include
breaking changes. Every release gets an entry in [CHANGELOG.md](CHANGELOG.md).

## License

Vela UI is released under the [MIT License](LICENSE).
