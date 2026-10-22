// Opens and closes a <dialog> for DialogPrimitiveContent. Blazor can't call these
// methods from markup, and a dialog opened with showModal() only leaves the top
// layer through close(), not by removing its open attribute.

const settings = new WeakMap();

// A click on the backdrop reaches the <dialog> itself, at a point outside its box.
// Clicks on its padding, border, or scrollbar also target the <dialog>, but inside the
// box. The box is rounded outward because, when the page is zoomed, pointer positions
// can be whole pixels while the box's edges aren't.
function isOutside(dialog, event) {
  if (event.target !== dialog) {
    return false;
  }

  const box = dialog.getBoundingClientRect();
  return event.clientX < Math.floor(box.left) || event.clientX > Math.ceil(box.right)
    || event.clientY < Math.floor(box.top) || event.clientY > Math.ceil(box.bottom);
}

function listen(dialog) {
  const state = { closeOnClickOutside: true, pressedOutside: false };
  settings.set(dialog, state);

  dialog.addEventListener("pointerdown", event => {
    state.pressedOutside = isOutside(dialog, event);
  });

  // Close on the click, not the press, so the rest of the click can't land on the
  // page under the backdrop. A press that starts inside, such as selecting text and
  // dragging out, never closes it.
  dialog.addEventListener("click", event => {
    if (state.closeOnClickOutside && state.pressedOutside && isOutside(dialog, event)) {
      dialog.close();
    }

    state.pressedOutside = false;
  });

  return state;
}

export function show(dialog, closeOnClickOutside) {
  const state = settings.get(dialog) ?? listen(dialog);
  state.closeOnClickOutside = closeOnClickOutside;

  if (!dialog.open) {
    dialog.showModal();
  }
}

export function close(dialog) {
  if (dialog.open) {
    dialog.close();
  }
}
