# Vela UI

Vela UI is a component library for Blazor that you install as source code.
Instead of referencing a package of sealed components, you run the `vela`
CLI to copy a component's Razor and C# files into your own project. After
that, the code is yours. You can read it, restyle it, and change its
behavior without waiting on a release or working around an API.

The idea comes from [shadcn/ui](https://ui.shadcn.com/). This is an
independent Blazor implementation, not a port.

## Two ways to use it

You can pick either layer, or mix them:

- **Styled components.** `vela add dialog` copies a ready-to-use component,
  styled with Tailwind CSS and the theme tokens in `vela.css`. This is the
  shadcn/ui experience.
- **Headless primitives.** The `Vela.UI.Primitives` NuGet package provides
  the behavior alone: focus management, keyboard navigation, ARIA, and
  open/close state. It renders no CSS classes, so you style it however you
  like, with or without Tailwind. This is the Radix experience.

The styled components are built on the primitives, so both behave the
same way. Simple components such as Button have no behavior worth sharing
and exist only in the styled layer; the primitives start with Dialog.

## The look

The styled components use the Vela design system: one flame on a neutral
scale. A twelve-step warm-graphite scale carries the surfaces, hairlines,
and text, and a single orange flame is budgeted to the primary action
(`Button` with the default variant), keyboard focus, and progress. Night
(dark) is the default theme; add `data-theme="light"` to `<html>` for Day.
Text uses Host Grotesk at two weights, and key caps and figures use Spline
Sans Mono, both bundled.

The component APIs follow shadcn/ui, so the variant, size, and part names
will be familiar. Only the styling is Vela's.

An optional motion layer, `vela-light.js`, adds a focus ring that travels
between controls, the ember flare when someone finishes something
(`data-ember`), and figures that roll in place when they change
(`data-roll`). Without it, every state still shows. The headless primitives
are not styled by any of this.

## Status

Vela UI is in early development and there is nothing to install yet.
Button, Input, Label, Field, Card, Kbd, and Spinner exist in the component registry
(`src/Core`) and are covered by tests, but the CLI can't copy them into an
app yet: it only prints its help and version.

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
| `src/Core/Styles/vela.css` | The Vela theme for Tailwind v4: design-system tokens (Night and Day), type roles, radii, shadows, and the motion layer's CSS |
| `src/Core/Styles/fonts/` | Host Grotesk and Spline Sans Mono (Latin subsets, SIL OFL 1.1) |
| `src/Core/Scripts/vela-light.js` | The optional motion layer |
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
  logic, as a partial class. Each `.razor` file declares its own `@using`
  lines; `src/Core` has no shared `_Imports.razor`, because it wouldn't be
  copied into apps.
- A component with logic gets a `.razor.cs` file. Simple parts that only wrap
  an element with classes (such as `CardTitle`) are a single `.razor` file
  with a short `@code` block.
- Form controls such as `Input` inherit Blazor's `InputBase<T>` so binding,
  validation, and static form posts work the way they do for `InputText`.
- Variant and size enums sit in the same folder, one type per file.
- Classes stay inline as Tailwind utilities, grouped into named constants by
  purpose (`LayoutClasses`, `FocusClasses`, `InvalidClasses`, `DisabledClasses`,
  ...) so you can find the line to change.
- Classes are combined with `ClassNames.Join`, which is the only shared helper.
  It appends without resolving conflicts, so each kind of utility (rounding,
  background, edge, icon size) is set in exactly one place: the base, the
  variant, or the size. A test enforces this for Button.
- Components take a `Class` parameter, pass other attributes through to the
  root element, and set `data-slot` (plus `data-variant`/`data-size` where
  they apply) with shadcn/ui's names.
- Components use Tailwind utilities that refer to theme tokens (`bg-surface`,
  `text-fg-muted`, `shadow-control`, never raw colours), so restyling means
  editing `vela.css`, not the component. Edges are box-shadow rings, not
  borders, as the design system asks.
- The flame (`bg-flame`, `text-flame-text`) belongs only to the primary
  action, focus, and progress. Checked, active, and selected states use the
  text colour. A test keeps every Button variant except the default off it.
- Tests are bUnit tests written in Razor under `tests/Core/Components/`,
  named `Component_Scenario`.

## Versioning

Vela UI uses [Semantic Versioning](https://semver.org/). The version is set
once, in `Directory.Build.props`. Until 1.0, a minor release may include
breaking changes. Every release gets an entry in [CHANGELOG.md](CHANGELOG.md).

## License

Vela UI is released under the [MIT License](LICENSE). The component APIs are
adapted from shadcn/ui, and the bundled fonts are under the SIL Open Font
License; see [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md).
