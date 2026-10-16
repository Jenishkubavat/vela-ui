# Vela UI

Vela UI is a component library for Blazor that you install as source code.
Instead of referencing a package of sealed components, you run the `vela`
CLI to copy a component's Razor and C# files into your own project. After
that, the code is yours. You can read it, restyle it, and change its
behavior without waiting on a release or working around an API.

The idea comes from [shadcn/ui](https://ui.shadcn.com/). This is an
independent Blazor implementation, not a port.

## Status

Vela UI is in early development and there is nothing to install yet. Button
exists in the component registry (`src/Core`) and is covered by tests, but
the CLI can't copy it into an app yet: it only prints its help and version.

The first milestone is `vela init` and `vela add` for Button, Input, Label,
and Card, with light and dark themes driven by CSS variables. Dialog,
Dropdown Menu, and Select come after that, once keyboard and focus
behavior can be tested in a real browser.

## Building from source

You need the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```bash
dotnet build
dotnet test
dotnet run --project src/Cli -- --help
```

To run one test:

```bash
dotnet test --filter "FullyQualifiedName~ButtonTests.Button_Disabled"
```

The build treats warnings as errors and enforces the code style in
`.editorconfig`, so fix a warning rather than suppressing it.

NuGet package versions are managed centrally in `Directory.Packages.props`.
To add a package, add a `PackageVersion` entry there and a
`PackageReference` without a version in the project file.

## Repository layout

| Path | Contents |
| --- | --- |
| `src/Core/` | The component registry: every component's source, compiled and tested but never published as a package |
| `src/Core/Styles/vela.css` | Theme tokens (Tailwind v4 CSS variables, light and dark) |
| `src/Primitives/` | `Vela.UI.Primitives`, headless behavior with no styles (like Radix for shadcn/ui), published as a NuGet package. Empty until Dialog. |
| `src/Cli/` | The `vela` command-line tool |
| `tests/Core/`, `tests/Cli/` | Tests, mirroring the `src/` folders |
| `Directory.Build.props` | Version number and settings shared by every project |
| `Directory.Packages.props` | NuGet package versions for every project |
| `NuGet.config` | Restricts package restore to nuget.org |

## Writing a component

Each component lives in its own folder under `src/Core/Components/`, so the
CLI can copy one folder into an app. `Button` is the reference:

- `Button.razor` holds the markup and `Button.razor.cs` the parameters and
  logic, as a partial class.
- Variant and size enums sit in the same folder, one type per file.
- Classes are combined with `ClassNames.Join`, which is the only shared helper.
  It appends without resolving conflicts, so each kind of utility (rounding,
  border color, focus ring color, icon size) is set in exactly one place: the
  base, the variant, or the size. A test enforces this for Button.
- Components take a `Class` parameter, pass other attributes through to the
  root element, and set `data-slot` (plus `data-variant`/`data-size` where
  they apply) with shadcn/ui's names.
- Components use Tailwind utilities that refer to theme tokens (`bg-primary`,
  not `bg-neutral-900`), so restyling means editing `vela.css`, not the
  component.
- Tests are bUnit tests written in Razor under `tests/Core/Components/`,
  named `Component_Scenario`.

## Versioning

Vela UI uses [Semantic Versioning](https://semver.org/). The version is set
once, in `Directory.Build.props`. Until 1.0, a minor release may include
breaking changes. Every release gets an entry in [CHANGELOG.md](CHANGELOG.md).

## License

Vela UI is released under the [MIT License](LICENSE). The component design is
adapted from shadcn/ui; see [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md).
