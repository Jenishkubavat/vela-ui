# Contributing to Vela UI

Instructions for building, testing, and contributing components to Vela UI.

Everyone taking part follows the [code of conduct](CODE_OF_CONDUCT.md). To report a bug or ask for a component, open an issue and pick the matching form. How versions are numbered is described in the README's [Versioning](README.md#versioning) section.

## Development Setup

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later.
- [Node.js](https://nodejs.org/) (version 20 or later) for compiling Tailwind CSS during development.
- Git.

### Clone and Verify Build

```bash
git clone https://github.com/Jenishkubavat/vela-ui.git
cd vela-ui

# Restore packages and build all projects
dotnet build

# Run the test suite (CLI tests, Primitives tests, and Core component tests)
dotnet test

# Verify code formatting and style
dotnet format --verify-no-changes
```

The repository enables `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`. All builds and pull requests must compile without warnings.

## Repository Architecture

The repository is structured into two main layers plus the CLI:

- `src/Core/`: The component registry (`Vela.UI.Components.csproj`). These are the styled components copied into applications via `vela add`. Compiled and tested, but never published as a package (`IsPackable=false`).
- `src/Primitives/`: The headless primitives library (`Vela.UI.Primitives.csproj`). Provides unstyled behaviors, ARIA, focus, and keyboard management. Published as a standalone NuGet package.
- `src/Cli/`: The command-line tool (`Vela.UI.Cli.csproj`). Bundles registry files and executes `vela init` and `vela add`.
- `tests/`: Unit and component test suites mirroring the `src/` layout (`tests/Core/`, `tests/Primitives/`, `tests/Cli/`).

## Component Guidelines

When creating or modifying components, follow these conventions:

### 1. Folder Structure and File Naming
- Each component lives in its own folder under `src/Core/Components/<Name>/` so the CLI can copy a single folder into user applications.
- Split components into `Name.razor` (markup and `@namespace Vela.UI.Components`) and `Name.razor.cs` (logic and parameters in a partial class).
- Exception: simple parts that only wrap elements with styling (such as `CardTitle.razor`) can stay as a single `.razor` file with a concise `@code` block.
- Keep variant and size enums in the same folder, one type per file (e.g. `ButtonVariant.cs`, `ButtonSize.cs`).

### 2. Self-Contained Files
- Every `.razor` file must declare all `@using` directives it requires.
- `src/Core` deliberately contains no `_Imports.razor`. Components must build cleanly using only the files copied by the CLI.

### 3. Headless Primitives First
- If Radix UI provides a primitive for the component (such as Dialog, DropdownMenu, Popover, Tooltip), build the unstyled primitive in `src/Primitives/` first, test it standalone in `tests/Primitives/`, and then create the styled component in `src/Core/` wrapping it.
- Primitives must never reference `src/Core` and must not render any CSS classes.
- Public members in `Vela.UI.Primitives` require XML documentation summaries.

### 4. Form Controls and SSR Support
- Form controls must inherit from Blazor's `InputBase<T>` or `CheckedInputPrimitiveBase` to support `@bind-Value`, `EditForm` validation (`aria-invalid`), and static server-rendered (SSR) form posts.
- For controls that use native inputs (Checkbox, Switch, RadioGroup), use the hidden native input pattern so state styling works with `:has(:checked)` under static SSR without JavaScript execution.

### 5. Styling Conventions
- Write inline Tailwind CSS utilities using semantic Vela design tokens from `Styles/vela.css` (`bg-surface`, `text-fg`, `shadow-control`, `ease-vela`). Do not use raw colours.
- Group Tailwind utilities into clear private string constants by purpose (`LayoutClasses`, `ColorClasses`, `FocusClasses`, `DisabledClasses`, `InvalidClasses`) so application developers can easily inspect and edit copied files.
- Combine utility strings using `ClassNames.Join`. Do not introduce runtime class-merging libraries.
- The flame (`bg-flame`, `text-flame-text`) is strictly budgeted: use it only for primary actions (`ButtonVariant.Default`), keyboard focus rings, and progress indicators. Checked, active, or selected states use text or neutral surface colors.
- Use box-shadow rings (`shadow-control`, `shadow-hairline`) instead of borders.

### 6. Updating the Registry
When adding a new component or introducing a dependency between components:
- Register the component in `src/Core/registry.json`.
- If component A depends on component B (e.g. `attachment` uses `button`), declare `"dependencies": ["button"]` in `registry.json`.
- The CLI test suite validates `registry.json` against actual file references.

## Testing Standards

- Every component and primitive must have automated tests in `tests/Core/` or `tests/Primitives/`.
- Component tests use [bUnit](https://bunit.dev/) with Razor syntax (`Component_Scenario`), inheriting `BunitContext`.
- Use explicit assertions (`Assert.Equal`, `Assert.Contains`) rather than snapshot files.
- Cover all variants, sizes, disabled states, default parameters, event callbacks, and attribute passthrough (`AdditionalAttributes`).
- Keyboard, focus, and dismissal behavior is tested in a real browser: add a page to `examples/Demo` and Playwright tests to `tests/Browser/`. bUnit can't prove focus or key handling.

## Git and Commit Workflow

- The main production branch is `master`.
- Create short-lived feature branches for your changes:
  ```bash
  git checkout -b feat/my-component
  ```
- Commit messages follow [Conventional Commits](https://www.conventionalcommits.org/):
  - Subject line: 50 characters or fewer, imperative tense, capitalized, no trailing period (e.g. `feat: Add Breadcrumb component`, `fix: Correct switch focus outline`).
  - Body: Explain the context, motivation, and user-facing behavior. Wrap lines at 72 characters.
- Ensure all tests pass and formatting is clean before opening a Pull Request:
  ```bash
  dotnet test
  dotnet format --verify-no-changes
  ```
