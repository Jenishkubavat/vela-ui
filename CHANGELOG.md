# Changelog

Notable changes to Vela UI are recorded here. The format follows
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and versions
follow [Semantic Versioning](https://semver.org/).

## [Unreleased]

### Added

- `vela` CLI with `--help` and `--version`.
- Button component matching shadcn/ui: six variants, eight sizes, icon spacing
  through `data-icon`, `aria-invalid` and `aria-expanded` styles, and
  `Button.GetClasses` for styling links as buttons.
- Input component built on Blazor's `InputBase`: works with `@bind-Value`,
  `EditForm` validation (error styles through `aria-invalid`), and static SSR
  form posts. `Immediate` updates on every keystroke. `Input.GetClasses`
  styles `InputNumber`, `InputDate`, and `InputFile` to match.
- Label component that links to a control through `for`.
- Theme tokens for Tailwind CSS v4 with light and dark values.
