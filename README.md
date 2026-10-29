# Vela UI

Vela UI is a component library for Blazor that you install as source code. Instead of referencing a package of sealed components, you run the `vela` CLI to copy component source directly into your application. Once installed, the code belongs to you: you can read it, restyle it, and change its behavior without waiting on a release or working around an API.

The idea comes from [shadcn/ui](https://ui.shadcn.com/). This is an independent Blazor implementation, not a port.

## Two Ways to Use It

You can pick either layer, or mix them:

- **Styled components.** `vela add button card` copies ready-to-use components, styled with Tailwind CSS v4 and the theme tokens in `vela.css`. This is the shadcn/ui experience.
- **Headless primitives.** The `Vela.UI.Primitives` NuGet package provides behavior alone: focus management, keyboard navigation, ARIA, and open/close state. It renders no CSS classes, so you can style it with or without Tailwind CSS. This is the Radix experience.

Styled components are built on the primitives, so both behave the same way. Every component that Radix provides a primitive for has one here too (such as `LabelPrimitive` under styled `Label`). Components with no separate behavioral state (such as `Button`, `Card`, and `Badge`) exist directly in the styled layer.

## The Look

The styled components use the Vela design system: one flame on a neutral scale.
- **Graphite scale:** A twelve-step warm-graphite scale carries all surfaces, hairlines, and text.
- **Budgeted flame:** A single orange flame (`#ff9d3d`) is reserved for the primary action (`Button` with default variant), keyboard focus rings, and progress indicators. Secondary actions, active states, and checked toggles use neutral tones so the primary action stands out.
- **Hairlines and rings:** Surfaces and controls use subtle box-shadow rings (`shadow-control`, `shadow-hairline`) rather than borders.
- **Typography:** Bundled Latin subsets of [Host Grotesk](https://github.com/Element-Type/HostGrotesk) for interface text and [Spline Sans Mono](https://github.com/SorkinType/SplineSansMono) for figures, keys, and code.
- **Themes:** Night (dark mode) is the default. Add `data-theme="light"` to `<html>` for Day.
- **Optional motion layer:** Including `vela-light.js` adds a focus ring that travels between controls, rolling figures (`data-roll`), and ember flares upon action completion (`data-ember`). If omitted, all controls remain fully functional with standard CSS focus outlines.

## Status

Vela UI is in early development (version 0.1.0) and is not published to NuGet yet. The `vela` CLI can set up a Blazor project and copy components from the registry (`src/Core`) into it.

Because `Vela.UI.Primitives` is not yet on nuget.org, components that depend on primitives add a package reference that cannot be restored from public feeds alone. Until packages are published:
- Run the CLI from a clone of this repository.
- Either pack `Vela.UI.Primitives` locally and register the local folder in your `NuGet.config`, or replace the `<PackageReference Include="Vela.UI.Primitives" ... />` that `vela add` adds with a `<ProjectReference Include=".../src/Primitives/Vela.UI.Primitives.csproj" />`.

Dialog is the first overlay, with its keyboard and focus behavior tested in a real browser. Dropdown Menu and Select come next. To see every MVP component, run the examples app with `dotnet run --project examples/Demo` (Node.js 20 or later builds its styles).

## Using the CLI

Run it in your Blazor project folder (.NET 10 or later):

```bash
dotnet run --project path/to/vela-ui/src/Cli -- init
dotnet run --project path/to/vela-ui/src/Cli -- add button card
```

`vela init` writes `vela.json`, the theme (`Styles/vela.css`), a Tailwind input file (`Styles/app.css`), the fonts, `vela-light.js`, and `ClassNames.cs`. It then prints the remaining steps: building the CSS with Tailwind CSS v4, linking it in `App.razor`, and adding a `@using` for the components' namespace.

`vela add` copies each component's folder into `Components/Ui/`, in your project's namespace, along with the components it uses. It never replaces a file you have changed without being asked: if a file differs, it stops and lists it. `--overwrite` replaces it and keeps the old version in `.vela/backups/`. Components built on a primitive also add a reference to the `Vela.UI.Primitives` package. Paths and the namespace can be changed in `vela.json` before adding components.

## Quickstart

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later.
- [Node.js](https://nodejs.org/) (version 20 or later) to run the Tailwind CSS v4 CLI.

### 1. Initialize your Blazor project

In your Blazor project folder (where your `.csproj` lives):

```bash
dotnet run --project path/to/vela-ui/src/Cli -- init
```

### 2. Build Tailwind CSS v4

Install Tailwind CSS v4 and the CLI tool in your project folder, then start the watcher:

```bash
npm install tailwindcss @tailwindcss/cli
npx @tailwindcss/cli -i Styles/app.css -o wwwroot/tailwind.css --watch
```

Keep this process running in a terminal while developing.

### 3. Wire your layout and imports

In `Components/App.razor`, add the compiled stylesheet to `<head>` and optionally include `vela-light.js` before `</body>`:

```html
<head>
    <!-- ... other tags ... -->
    <link rel="stylesheet" href="tailwind.css" />
</head>
<body>
    <!-- ... routes ... -->
    <script src="vela-light.js"></script>
</body>
```

In `Components/_Imports.razor`, import the generated UI namespace (replace `MyApp` with your project's root namespace):

```razor
@using MyApp.Components.Ui
```

### 4. Add components

Copy the components you need into your project:

```bash
dotnet run --project path/to/vela-ui/src/Cli -- add button card input field
```

> **Note on package restore:** Adding components built on primitives (such as `field`, `label`, `checkbox`, or `switch`) adds a package reference to `Vela.UI.Primitives 0.1.0`. Until published to NuGet, build the package locally:
> ```bash
> dotnet pack path/to/vela-ui/src/Primitives -o ./packages
> ```
> and add `<add key="local" value="./packages" />` under `<packageSources>` in your `NuGet.config`.

### 5. Use components in your Razor pages

```razor
@page "/profile"

<Card Class="max-w-md">
    <CardHeader>
        <CardTitle>Profile Settings</CardTitle>
        <CardDescription>Manage how your account appears to teammates.</CardDescription>
    </CardHeader>

    <CardContent Class="space-y-4">
        <Field>
            <FieldLabel for="username">Display Name</FieldLabel>
            <Input id="username" @bind-Value="model.DisplayName" placeholder="e.g. Alex Chen" />
            <FieldDescription>Visible on your team dashboard and comments.</FieldDescription>
        </Field>

        <Field>
            <FieldLabel for="email">Work Email</FieldLabel>
            <Input id="email" type="email" @bind-Value="model.Email" />
        </Field>

        <div class="flex items-center space-x-2 pt-2">
            <Checkbox id="notifications" @bind-Value="model.EmailNotifications" />
            <Label For="notifications">Receive weekly digest</Label>
        </div>
    </CardContent>

    <CardFooter Class="flex justify-end space-x-2">
        <Button Variant="ButtonVariant.Outline" OnClick="Cancel">Cancel</Button>
        <Button Variant="ButtonVariant.Default" OnClick="SaveProfile">Save Changes</Button>
    </CardFooter>
</Card>

@code {
    private ProfileModel model = new();

    private void SaveProfile()
    {
    }

    private void Cancel()
    {
    }

    private class ProfileModel
    {
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public bool EmailNotifications { get; set; } = true;
    }
}
```

## Component Registry

Components available in the registry:

### Actions and Controls

| Component | Description |
| --- | --- |
| `button` | Native `<button>` with 6 variants (`Default`, `Destructive`, `Outline`, `Secondary`, `Ghost`, `Link`) and 8 sizes. `Button.GetClasses()` styles navigation links. Only `Default` uses the flame. |
| `button-group` | Groups connected buttons horizontally or vertically with coordinated borders and rounding. |
| `toggle` | Two-state pressable button built on `TogglePrimitive`, supporting `@bind-Pressed`. |

### Form and Input Controls

| Component | Description |
| --- | --- |
| `input` | Sunken single-line text input inheriting `InputBase<string?>`. Works with `@bind-Value`, `EditForm` validation, and static SSR form posts. `Input.GetClasses()` styles `InputNumber`, `InputDate`, and `InputFile`. |
| `textarea` | Multi-line text field inheriting `InputBase<string?>` with proportional vertical sizing. |
| `native-select` | Styled native HTML `<select>` supporting option groups, single selections, and static SSR forms. |
| `checkbox` | Sunken square checkbox built on `CheckboxPrimitive`. Uses native input under the hood for static SSR compatibility and `:has(:checked)` styling. |
| `switch` | Animated sliding toggle switch built on `SwitchPrimitive` for settings taking immediate effect. |
| `radio-group` | Mutually exclusive radio buttons with arrow-key keyboard navigation, built on `RadioGroupPrimitive`. |
| `input-group` | Compounds inputs with inline button addons, icons, or text prefixes and suffixes. |
| `field` | Accessible form composition: `Field`, `FieldLabel`, `FieldDescription`, `FieldError`, `FieldContent`, `FieldGroup`, `FieldSet`, `FieldLegend`, and `FieldSeparator`. `FieldError` automatically binds to `EditContext` validation messages via `For="() => Model.Property"`. |
| `label` | Accessible form label built on `LabelPrimitive`, targeting controls via `For`. |

### Layout and Structure

| Component | Description |
| --- | --- |
| `card` | Container surface composed of `Card`, `CardHeader`, `CardTitle`, `CardDescription`, `CardAction`, `CardContent`, and `CardFooter`. Padding is driven by `--card-spacing`. |
| `table` | Accessible data tables: `Table`, `TableHeader`, `TableBody`, `TableFooter`, `TableRow`, `TableHead`, `TableCell`, and `TableCaption`. |
| `separator` | Semantic (`role="separator"`) or decorative divider, horizontal or vertical. |
| `aspect-ratio` | Ratio-preserving box (`16/9`, `4/3`, `1/1`) built on `AspectRatioPrimitive`. |
| `item` | Structured media item layout: `Item`, `ItemMedia`, `ItemContent`, `ItemTitle`, `ItemDescription`, `ItemActions`, and `ItemSeparator`. |
| `collapsible` | Section that a trigger shows and hides (`Collapsible`, `CollapsibleTrigger`, `CollapsibleContent`), built on `CollapsiblePrimitive`. |
| `direction` | `DirectionProvider` cascading text direction (`LeftToRight` or `RightToLeft`) to nested components, built on `DirectionPrimitive`. |

### Feedback and Status

| Component | Description |
| --- | --- |
| `alert` | Non-modal notification box (`Alert`, `AlertTitle`, `AlertDescription`) with `Default` and `Destructive` variants. |
| `badge` | Small status label (`Default`, `Secondary`, `Destructive`, `Outline`, `Ghost`, `Link`). Never filled with the flame. |
| `progress` | Progress indicator styled as a candle wick with a flame leading edge, built on `ProgressPrimitive`. |
| `spinner` | Animated SVG loading indicator, for buttons and busy states. |
| `skeleton` | Loading placeholder shape using high-contrast surface fills that render consistently across Day and Night. |
| `empty` | Empty state layout with icon well, title, description, and action button slots. |
| `bubble` | Chat message bubble (`Bubble`, `BubbleGroup`, `BubbleContent`, `BubbleReactions`), aligned to either side. |
| `message` | Conversation or event log message layout. |

### Navigation and Media

| Component | Description |
| --- | --- |
| `avatar` | User avatar built on `AvatarPrimitive`. Displays image with automatic fallback initials; fallback renders under static SSR while image loads. |
| `breadcrumb` | Hierarchical navigation trail: `BreadcrumbList`, `BreadcrumbItem`, `BreadcrumbLink`, `BreadcrumbPage`, `BreadcrumbSeparator`, and `BreadcrumbEllipsis`. |
| `pagination` | Pagination navigation controls: `PaginationContent`, `PaginationItem`, `PaginationLink`, `PaginationPrevious`, `PaginationNext`, and `PaginationEllipsis`. |
| `attachment` | File attachment card with name, file size metadata, and removal action. |
| `kbd` | Keyboard key indicator (`Kbd`, `KbdGroup`) using Spline Sans Mono typography. |
| `marker` | A quiet line of context in a conversation or feed, such as "Today" or "Ada joined". |

### Overlays

| Component | Description |
| --- | --- |
| `dialog` | Modal dialog (`Dialog`, `DialogTrigger`, `DialogContent`, `DialogHeader`, `DialogTitle`, `DialogDescription`, `DialogFooter`, `DialogClose`) built on `DialogPrimitive` and the browser's `<dialog>` element. The page behind becomes inert, Escape and a click on the backdrop close it, and focus returns to the trigger. `OnConfirm` adds Ctrl+Enter (⌘+Enter on a Mac). |

## Headless Primitives Layer

For developers who prefer writing custom CSS or do not use Tailwind CSS, Vela UI provides unstyled headless primitives in the `Vela.UI.Primitives` namespace.

```razor
@using Vela.UI.Primitives

<TogglePrimitive @bind-Pressed="isMuted" class="custom-mute-btn">
    @(isMuted ? "Unmute" : "Mute")
</TogglePrimitive>
```

- **No CSS dependencies:** Primitives render zero utility classes and include no bundled stylesheets.
- **State hooks:** Component state is communicated via standard HTML and `data-*` attributes (`data-state="checked|unchecked"`, `data-orientation="horizontal|vertical"`, `data-disabled`).
- **SSR compatibility for form controls:** Form controls (`CheckboxPrimitive`, `SwitchPrimitive`, `RadioGroupPrimitive`) render hidden native inputs via `CheckedInputPrimitiveBase`. Static server-rendered forms submit values without JavaScript, and can be styled with `:has(:checked)`.
- **Validation integration:** Form primitives inherit from Blazor's `InputBase<T>` and participate in `EditContext` validation, while remaining functional unbound.

See [`src/Primitives/README.md`](src/Primitives/README.md) for full primitive API documentation.

## Render Modes

Components render under every Blazor render mode, but some features need an interactive one (Server, WebAssembly, or Auto). Under static server rendering, nothing runs in the browser after the page loads.

| Works under static server rendering | Needs an interactive render mode |
| --- | --- |
| Everything that only displays: Alert, Badge, Card, Table, Progress, Skeleton, and the rest | `OnClick` on `Button`, `InputGroupButton`, and `Attachment` (a `type="submit"` button still submits a static form) |
| Form posts from `Input`, `Textarea`, `NativeSelect`, `Checkbox`, `Switch`, and `RadioGroup`, which are native inputs | Pressing a `Toggle` |
| Arrow keys in a `RadioGroup` and Space on a `Checkbox` or `Switch`, which the browser handles | Opening and closing a `Collapsible` or a `Dialog` |
| Pagination and breadcrumb links | Removing an `Avatar` photo that failed to load (the initials show either way) |

Under static rendering, `data-state` doesn't update when a form control changes, so the styled controls draw their checked look from the native input (`:has(:checked)`).

With the **WebAssembly** or **Auto** render mode, interactive components have to live in the `.Client` project. Run `vela init` and `vela add` there (`--cwd MyApp.Client`). `App.razor` and its stylesheet link stay in the server project; the server serves the client project's `wwwroot`, so the link works as written.

## Accessibility

Each interactive component uses the native element the browser already makes accessible, and adds ARIA only where the element doesn't carry the meaning itself.

| Component | Keyboard and screen readers |
| --- | --- |
| `Button`, `Toggle` | Enter and Space activate them. `Toggle` announces "pressed" or "not pressed" through `aria-pressed`. |
| `Checkbox`, `Switch` | Space toggles them. `Switch` is announced as a switch (`role="switch"`). Clicking a linked `Label` toggles them too. |
| `RadioGroup` | Tab lands on the selected option, and the arrow keys move between options and select them. |
| `Collapsible` | Enter and Space on the trigger open and close it; `aria-expanded` and `aria-controls` tell screen readers what it controls. |
| `Dialog` | Focus moves into the dialog when it opens and can't leave it; the page behind is inert. Escape closes it, and focus returns to the trigger. It's named by its title and described by its description. |
| `Input`, `Textarea`, `NativeSelect` | Link a `Label` or `FieldLabel` with `for`. With `EditForm` validation, an invalid field gets `aria-invalid`; point `aria-describedby` at its `FieldError`. |

Keyboard and focus behavior for Collapsible, RadioGroup, and Dialog is covered by browser tests in Chromium. Automated tests don't replace testing with a screen reader, and Vela UI doesn't claim WCAG conformance.

## Known Limitations

- `Vela.UI.Primitives` isn't on NuGet yet, and the CLI runs from a clone of this repository (see [Status](#status)).
- Browser tests run in Chromium only. Firefox and Safari aren't tested automatically yet.
- There's no manual screen-reader audit yet.
- There's no `vela update` or diff. To take a newer version of a component you've installed, run `vela add <name> --overwrite`; your previous files go to `.vela/backups/`.
- `vela init` sets up one project. With WebAssembly or Auto, run it in the `.Client` project (see [Render Modes](#render-modes)).
- `vela.css` reserves the scrollbar's space on every page (`scrollbar-gutter: stable`), so the page doesn't shift when a dialog hides the scrollbar.
- The Dialog's exit animation only plays in browsers that support `transition-behavior: allow-discrete`; elsewhere it closes without animating.

## Versioning

Vela UI follows [Semantic Versioning](https://semver.org/). Before 1.0, a minor release (0.2, 0.3) can include breaking changes; [CHANGELOG.md](CHANGELOG.md) lists them under each release.

The CLI, the component registry it bundles, and the `Vela.UI.Primitives` package share one version number. `vela add` references the Primitives version that matches the CLI it was run with.

Components you've installed are your own code: updating the CLI never changes them. Read the changelog, then re-add a component with `--overwrite` if you want its newer version.

## CLI Reference

### Commands

#### `vela init`

Initializes Vela UI in the current Blazor project:
- Verifies the project targets `.NET 10.0` or later.
- Creates `vela.json`.
- Installs `Styles/vela.css`, `Styles/app.css`, `ClassNames.cs`, fonts, and `vela-light.js`.

#### `vela add <component>...`

Copies one or more components from the registry into your application:
- Resolves and copies dependent components automatically (e.g. adding `field` also adds `label`).
- Skips re-copying existing dependencies if their folders already exist, preserving local edits.
- If a target component file exists and its content differs, installation stops and lists the conflicting files.
- Adds a package reference to `Vela.UI.Primitives` in your `.csproj` if any added component requires it.

### Options

| Option | Description |
| --- | --- |
| `--cwd <folder>` | Runs the command in the specified folder rather than the current working directory. |
| `--overwrite` | Replaces differing files. Replaced files are backed up in `.vela/backups/<timestamp>/`. |
| `-h, --help` | Displays command usage and available components. |
| `--version` | Displays the current version of the CLI and bundled registry. |

### Configuration (`vela.json`)

`vela init` creates a configuration file in your project root:

```json
{
  "project": "MyApp.csproj",
  "namespace": "MyApp.Components.Ui",
  "components": "Components/Ui",
  "styles": "Styles",
  "assets": "wwwroot",
  "registry": "bundled",
  "version": "0.1.0"
}
```

You can customize `namespace`, `components`, `styles`, or `assets` paths in `vela.json` prior to running `vela add`.

### Exit Codes

- `0`: Success.
- `1`: Cannot run (not a Blazor project, missing `vela.json`, or unknown component name).
- `2`: Bad arguments.
- `3`: Files would be overwritten (run with `--overwrite` to replace with automatic backup).

## Design System Tokens & Theming

Vela UI's design system tokens are declared in `Styles/vela.css` and mapped to Tailwind utilities.

### Surface and Foreground Tokens

| Token | Utility | Night (Default) | Day (`data-theme="light"`) | Purpose |
| --- | --- | --- | --- | --- |
| `--vela-bg` | `bg-room` | `#111010` | `#f6f5f2` | Application background / room |
| `--vela-surface` | `bg-surface` | `#171615` | `#ffffff` | Primary content panels |
| `--vela-surface-raised` | `bg-surface-raised` | `#1f1e1c` | `#ffffff` | Raised cards, popovers, keys |
| `--vela-surface-sunken` | `bg-surface-sunken` | `#0b0a09` | `#eeede8` | Wells, inputs, recessed tracks |
| `--vela-text` | `text-fg` | `#f0ece5` | `#1b1a18` | Primary body text |
| `--vela-text-muted` | `text-fg-muted` | `#aba59c` | `#5d5852` | Secondary labels and helper text |
| `--vela-text-faint` | `text-fg-faint` | `#938d84` | `#6b665f` | Placeholders and disabled text |
| `--vela-flame` | `bg-flame` | `#ff9d3d` | `#f7922b` | Primary action accent |

### Switching to Day Theme

Add `data-theme="light"` to your `<html>` element in `App.razor`:

```html
<html lang="en" data-theme="light">
```

Or toggle the attribute dynamically via JavaScript to support user theme switching.

## Building and Testing the Repository

To build the Vela UI repository itself:

```bash
# Build all projects
dotnet build

# Run the test suite
dotnet test

# Verify code formatting against .editorconfig
dotnet format --verify-no-changes
```

The browser tests in `tests/Browser` drive Chromium with Playwright. After the first build, install it once:

```bash
pwsh tests/Browser/bin/Debug/net10.0/playwright.ps1 install chromium
```

Building the examples app (`examples/Demo`) also builds its CSS with Tailwind CSS, which needs Node.js 20 or later.

### Repository Layout

| Path | Contents |
| --- | --- |
| `src/Core/` | The component registry: every component's source, compiled and tested but never published as a package |
| `src/Core/Styles/vela.css` | The Vela theme for Tailwind v4: design-system tokens (Night and Day), type roles, radii, shadows, and the motion layer's CSS |
| `src/Core/Styles/fonts/` | Host Grotesk and Spline Sans Mono (Latin subsets, SIL OFL 1.1) |
| `src/Core/Scripts/vela-light.js` | The optional motion layer |
| `src/Primitives/` | `Vela.UI.Primitives`, headless behaviour with no styles (like Radix for shadcn/ui), published as a NuGet package |
| `src/Cli/` | The `vela` command-line tool |
| `tests/Core/`, `tests/Primitives/`, `tests/Cli/` | Tests, mirroring the `src/` folders; the primitive tests don't reference `src/Core` |
| `tests/Browser/` | Keyboard and focus tests in Chromium, run with Playwright against the examples app |
| `examples/Demo/` | The examples app: a minimal and a realistic example for each component, using `src/Core` directly |
| `Directory.Build.props` | Version number and settings shared by every project |
| `Directory.Packages.props` | NuGet package versions for every project |
| `NuGet.config` | Restricts package restore to nuget.org |

## License

Vela UI is open source under the [MIT License](LICENSE). Component APIs are adapted from [shadcn/ui](https://github.com/shadcn-ui/ui) (MIT). Bundled Latin subsets of Host Grotesk and Spline Sans Mono are licensed under the [SIL Open Font License 1.1](THIRD-PARTY-NOTICES.md).
