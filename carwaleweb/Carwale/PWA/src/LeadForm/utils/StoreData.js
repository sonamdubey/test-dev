/**
 * Receives a state object and returns an object with
 * required keys and values
 * @param {*} state
 */
export function getStateFromStore(state) {
  return {
    NC: state.NC,
    location: state.location,
    page: state.leadClickSource.page.page,
    platform: state.leadClickSource.page.platform
  };
}
