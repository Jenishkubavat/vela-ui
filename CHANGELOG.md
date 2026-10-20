# Changelog

Notable changes to Vela UI are recorded here. The format follows
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and versions
follow [Semantic Versioning](https://semver.org/).

## [Unreleased]

### Added

- `vela` CLI with `--help` and `--version`.
- Button component with shadcn/ui's API (six variants, eight sizes, icon
  spacing through `data-icon`, `Button.GetClasses` for styling links as
  buttons) and the Vela look: only the default variant uses the flame.
- Input component built on Blazor's `InputBase`: works with `@bind-Value`,
  `EditForm` validation (error styles through `aria-invalid`), and static SSR
  form posts. `Immediate` updates on every keystroke. `Input.GetClasses`
  styles `InputNumber`, `InputDate`, and `InputFile` to match.
- Label component that links to a control through `for`.
- Card component with Header, Title, Description, Action, Content, and Footer
  parts, a small size, and spacing driven by the `--card-spacing` variable.
- Kbd and KbdGroup for showing keyboard shortcuts, and Spinner for loading
  states.
- Field component with shadcn/ui's parts (Field, FieldLabel, FieldTitle,
  FieldDescription, FieldError, FieldContent, FieldGroup, FieldSet,
  FieldLegend, FieldSeparator) and vertical, horizontal, and responsive
  layouts. FieldError shows EditForm validation messages through `For`.
- Alert, AspectRatio, Badge, Breadcrumb, Bubble, DirectionProvider, Empty,
  Marker, Message, Separator, and Skeleton, completing shadcn/ui's simplest
  components in the Vela look.
- The Vela design system as a Tailwind CSS v4 theme (`vela.css`): Night and
  Day tokens, type roles, machined radii, hairline-ring shadows, and the
  Host Grotesk and Spline Sans Mono fonts. Night is the default.
- Optional motion layer (`vela-light.js`): a focus ring that travels between
  controls, the ember flare and its flight to a progress wick, rolling
  figures, and overlays that open out of their trigger.
