// Opens and closes a <dialog> for DialogPrimitiveContent. Blazor can't call these
// methods from markup, and a dialog opened with showModal() only leaves the top
// layer through close(), not by removing its open attribute.

export function show(dialog) {
  if (!dialog.open) {
    dialog.showModal();
  }
}

export function close(dialog) {
  if (dialog.open) {
    dialog.close();
  }
}
