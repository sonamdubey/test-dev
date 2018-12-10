import { desktopBreakPoint } from "../constants";

export function isDesktop() {
  if (window.innerWidth > desktopBreakPoint) {
    return true;
  }

  return false;
}
