# Vela.UI.Primitives

Unstyled building blocks for accessible Blazor components. A primitive
handles behaviour and accessibility only: roles, ARIA attributes,
keyboard and focus handling, and state. It renders no CSS classes, so you
can style it with Tailwind, plain CSS, or anything else, using the `class`
attribute and the `data-*` attributes it exposes.

[Vela UI](https://github.com/Jenishkubavat/vela-ui) components are built
on these primitives, in the way shadcn/ui is built on Radix. The names
follow shadcn's convention for Radix (`LabelPrimitive`), so they never
clash with the styled components in your app (`Label`).

| Primitive | What it handles |
| --- | --- |
| `LabelPrimitive` | A native `<label>` linked to its control through `For` |
| `SeparatorPrimitive` | `role` and `aria-orientation`, plus `data-orientation` for styling |
| `AspectRatioPrimitive` | A culture-safe CSS `aspect-ratio` that keeps your own `style` |
| `DirectionPrimitive` | Cascades `TextDirection` so keyboard navigation follows right-to-left layouts |
| `CheckboxPrimitive` | A native checkbox over a box, with `Indeterminate` and an indicator part |
| `SwitchPrimitive` | A native checkbox with `role="switch"` over a track, with a thumb part |
| `RadioGroupPrimitive` | Native radios sharing a name, so arrow keys work, with item and indicator parts |
| `TogglePrimitive` | A `<button>` with `aria-pressed` and `@bind-Pressed` |

Checkbox, Switch, and RadioGroup bind with `@bind-Value` and take part in
`EditForm` validation, but also work unbound. Because they're native inputs,
style their checked look with `:has(:checked)` to make it work in static
server-rendered forms, where `data-state` isn't updated.

```razor
@using Vela.UI.Primitives

<SeparatorPrimitive Orientation="Orientation.Vertical"
                    Decorative="false"
                    class="my-divider" />
```

```css
.my-divider[data-orientation="vertical"] { width: 1px; background: #ccc; }
```
