
/**
 * Utility method to make promises cancelable
 * See {@link https://reactjs.org/blog/2015/12/16/ismounted-antipattern.html isMounted-Antipattern} for more details.
 * @param {Promise<any>} promise
 * @returns Object containing the wrapped promise as promise and a cancel method
 */
export const makeCancelable = (promise) => {
    let hasCanceled_ = false;

    const wrappedPromise = new Promise((resolve, reject) => {
      promise.then(
        val => hasCanceled_ ? reject({isCanceled: true}) : resolve(val),
        error => hasCanceled_ ? reject({isCanceled: true}) : reject(error)
      );
    });

    return {
      promise: wrappedPromise,
      cancel() {
        hasCanceled_ = true;
      },
    };
};
