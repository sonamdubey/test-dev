/*
* Error logger for javascript errors
*
* @author Meet Shah <meet.shah@carwale.com>
*
* @param {String || Array} log_source The js file sources to match errors from.
* @param {String} log_endpoint The api url to send the error object to.
* @example
*   ClientErrorLogger(["aeplcdn", "carwale"], "/api/exceptions/");
*/
export const ClientErrorLogger = function (log_source, log_endpoint) {
    if (log_source && log_endpoint) {
        log_source = log_source instanceof Array ? new RegExp(log_source.join('|')) : log_source;
    }
    else {
        throw new Error("Invalid logger configuration.");
    }
    if (!window.onerror) {

        window.onerror = function (messageOrEvent, source, lineNo, colNo, error) {
            const errorObject = {
                messageOrEvent: messageOrEvent,
                source: source,
                lineNo: lineNo,
                colNo: colNo,
                error: error ? error.stack : ''
            };
            const xmlhttp = new XMLHttpRequest();
            xmlhttp.open("POST", log_endpoint);
            xmlhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
            xmlhttp.send(JSON.stringify(errorObject));
            return false;
        }
    }
    else {
        throw new Error("Logging already enabled.")
    }
};
