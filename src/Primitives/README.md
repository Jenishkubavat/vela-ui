# Vela.UI.Primitives

`Vela.UI.Primitives` provides unstyled, accessible, headless building blocks for Blazor applications.

Inspired by the design of Radix UI, primitives handle behavior, keyboard navigation, focus management, ARIA roles, and state transitions without imposing any CSS or styling opinions. They render standard semantic HTML elements, expose state via `data-*` attributes, and pass custom classes and attributes directly through to the underlying markup.

You can use `Vela.UI.Primitives` as the headless foundation for your own custom design system, or style them using vanilla CSS, CSS Modules, or Tailwind CSS.

## Key Architectural Principles

### 1. Behavior Only, Zero Styles
Primitives render no CSS classes and include no bundled stylesheets. State is communicated to the DOM through standard HTML attributes (`disabled`, `aria-*`) and `data-*` attributes (`data-state="checked|unchecked|indeterminate"`, `data-orientation="horizontal|vertical"`, `data-disabled`).

### 2. Native Inputs for SSR and Form Support
Interactive form controls (`CheckboxPrimitive`, `SwitchPrimitive`, `RadioGroupPrimitive`) are built on `CheckedInputPrimitiveBase`. An invisible native `<input>` element is layered over the presentation element:
- **Static SSR Compatibility:** Form submissions function under static server rendering without client-side Blazor or JavaScript.
- **CSS State Styling:** Because the native input reflects the checked state in the DOM, controls can be styled using `:has(:checked)`, `:has(:focus-visible)`, and `:has(:disabled)` even when Blazor has not re-rendered the component.
- **Label & Accessibility:** Browser-native accessibility features (screen reader interaction, keyboard shortcuts, label association) work automatically.

### 3. Native Blazor Form Integration
Form primitives inherit from Blazor's `InputBase<T>` and participate in `EditContext` validation, but do not require an `EditForm` to function. When unbound, they maintain their own internal state.

---

## Component Reference

### 1. Checkbox

A two-state or three-state checkbox control with an indicator slot.

```razor
@using Vela.UI.Primitives

<CheckboxPrimitive @bind-Value="isAgreed"
                   Indeterminate="isIndeterminate"
                   class="checkbox-box"
                   id="terms">
    <CheckboxPrimitiveIndicator class="checkbox-check">
        <svg viewBox="0 0 16 16"><path d="M12 4L6 10l-2-2" /></svg>
    </CheckboxPrimitiveIndicator>
</CheckboxPrimitive>
<LabelPrimitive For="terms">I accept the terms and conditions</LabelPrimitive>
```

- **Parameters:** `@bind-Value` (bool), `Indeterminate` (bool), `Disabled` (bool). Set the form field name with the `name` attribute.
- **DOM States:** `data-state="checked|unchecked|indeterminate"`, `data-disabled`.

### 2. Switch

A toggle switch with `role="switch"` and an animated thumb slot.

```razor
<SwitchPrimitive @bind-Value="airplaneMode" class="switch-track" id="airplane">
    <SwitchPrimitiveThumb class="switch-thumb" />
</SwitchPrimitive>
<LabelPrimitive For="airplane">Airplane Mode</LabelPrimitive>
```

- **Parameters:** `@bind-Value` (bool), `Disabled` (bool). Set the form field name with the `name` attribute.
- **DOM States:** `data-state="checked|unchecked"`, `data-disabled`.

### 3. RadioGroup

A mutually exclusive group of radio buttons with arrow-key keyboard navigation.

```razor
<RadioGroupPrimitive @bind-Value="selectedTier" class="radio-group">
    <RadioGroupPrimitiveItem Value="@("free")" class="radio-item" id="tier-free">
        <RadioGroupPrimitiveIndicator class="radio-dot" />
    </RadioGroupPrimitiveItem>
    <LabelPrimitive For="tier-free">Free Plan</LabelPrimitive>

    <RadioGroupPrimitiveItem Value="@("pro")" class="radio-item" id="tier-pro">
        <RadioGroupPrimitiveIndicator class="radio-dot" />
    </RadioGroupPrimitiveItem>
    <LabelPrimitive For="tier-pro">Pro Plan</LabelPrimitive>
</RadioGroupPrimitive>
```

- **Parameters:** `@bind-Value` (string), `Disabled` (bool), `Required` (bool), `Orientation` (`Orientation?`). Items take `Value` (string) and `Disabled` (bool). Set the form field name with the `name` attribute.
- **Keyboard:** Up/Left moves to previous item; Down/Right moves to next item.
- **DOM States:** `data-state="checked|unchecked"`, `data-disabled`.

### 4. Toggle

A two-state button with `aria-pressed`.

```razor
<TogglePrimitive @bind-Pressed="isMuted" class="toggle-btn">
    @(isMuted ? "Muted" : "Unmuted")
</TogglePrimitive>
```

- **Parameters:** `@bind-Pressed` (bool), `Disabled` (bool).
- **DOM States:** `aria-pressed="true|false"`, `data-state="on|off"`, `data-disabled`.

### 5. Collapsible

A section that a trigger shows and hides. The content isn't rendered while closed.

```razor
<CollapsiblePrimitive @bind-Open="isOpen" class="collapsible">
    <CollapsiblePrimitiveTrigger class="collapsible-trigger">
        Toggle Details
    </CollapsiblePrimitiveTrigger>
    <CollapsiblePrimitiveContent class="collapsible-content">
        <p>Hidden details revealed when open.</p>
    </CollapsiblePrimitiveContent>
</CollapsiblePrimitive>
```

- **Parameters:** `@bind-Open` (bool), `Disabled` (bool). On the content, `ForceMount` (bool) keeps it in the page, hidden, while closed.
- **Accessibility:** Trigger manages `aria-expanded` and references content via `aria-controls`.
- **DOM States:** `data-state="open|closed"`, `data-disabled`.

### 6. Progress

Accessible progress indicator with `role="progressbar"`.

```razor
<ProgressPrimitive Value="progressValue" Max="100" class="progress-bar">
    <ProgressPrimitiveIndicator class="progress-fill" style="width: @(progressValue)%;" />
</ProgressPrimitive>
```

- **Parameters:** `Value` (double?, null represents indeterminate), `Max` (double, default 100), `GetValueLabel` (Func<double, double, string>?).
- **DOM States:** `data-state="indeterminate|loading|complete"`, `aria-valuenow`, `aria-valuemin`, `aria-valuemax`.

### 7. Avatar

User avatar image with automatic fallback display. Shows initials immediately while the image loads or if the image fails to load.

```razor
<AvatarPrimitive class="avatar">
    <AvatarPrimitiveImage Src="/user.jpg" alt="Alex Chen" class="avatar-img" />
    <AvatarPrimitiveFallback class="avatar-fallback">AC</AvatarPrimitiveFallback>
</AvatarPrimitive>
```

- **Parameters:** `Src` (string) on the image. Position the image over the fallback (for example `position: absolute`), because the fallback stays rendered until the image reports that it loaded.
- **Image Callbacks:** `OnLoadingStatusChange` (`AvatarImageStatus.Loading|Loaded|Error`).

### 8. Separator

Horizontal or vertical divider with accessible semantics.

```razor
<SeparatorPrimitive Orientation="Orientation.Horizontal" Decorative="false" class="separator" />
```

- **Parameters:** `Orientation` (`Orientation.Horizontal` or `Orientation.Vertical`), `Decorative` (bool, default true).
- **DOM States:** `role="separator"` (or `role="none"` if decorative), `data-orientation="horizontal|vertical"`.

### 9. AspectRatio

Maintains a fixed width-to-height ratio with culture-safe CSS generation.

```razor
<AspectRatioPrimitive Ratio="16.0 / 9.0" class="aspect-box">
    <img src="/banner.jpg" alt="Banner" class="w-full h-full object-cover" />
</AspectRatioPrimitive>
```

- **Parameters:** `Ratio` (double, default 1.0).

### 10. Direction

Cascades text direction (`LeftToRight` or `RightToLeft`) to nested components. It renders no element, so also set `dir="rtl"` on `<html>` or a wrapper for the browser's own layout.

```razor
<DirectionPrimitive Direction="TextDirection.RightToLeft">
    <!-- Components here adjust layout and keyboard navigation -->
</DirectionPrimitive>
```

- **Parameters:** `Direction` (`TextDirection.LeftToRight`, the default, or `TextDirection.RightToLeft`).

### 11. Label

A native `<label>` associated with a form control.

```razor
<LabelPrimitive For="user-email" class="form-label">Email Address</LabelPrimitive>
<input id="user-email" type="email" />
```

- **Parameters:** `For` (string).

---

## Styling Without Tailwind (Vanilla CSS Example)

Because primitives expose semantic attributes and `data-*` states, you can style them with pure CSS:

```css
/* Checkbox base */
.checkbox-box {
    display: inline-grid;
    place-items: center;
    width: 1rem;
    height: 1rem;
    border: 1px solid #71717a;
    border-radius: 2px;
    background: transparent;
    cursor: pointer;
}

/* Checked state using native pseudo-selector */
.checkbox-box:has(:checked) {
    background: #18181b;
    border-color: #18181b;
}

/* Indicator visibility */
.checkbox-box .checkbox-check {
    display: none;
}
.checkbox-box:has(:checked) .checkbox-check {
    display: block;
    color: #ffffff;
}

/* Focus ring */
.checkbox-box:has(input:focus-visible) {
    outline: 2px solid #ff9d3d;
    outline-offset: 2px;
}
```

## License

`Vela.UI.Primitives` is licensed under the [MIT License](https://github.com/Jenishkubavat/vela-ui/blob/master/LICENSE).
