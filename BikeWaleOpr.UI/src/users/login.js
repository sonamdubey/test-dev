var googleUser = {};

$(document).ready(function () {
    if (isAuthenticated.toLowerCase() == "false") { 
        showErrorMessage();
    }
    startApp();
});

var startApp = function () {    
    gapi.load('auth2', function () {
        
        auth2 = gapi.auth2.init({ 
            client_id: '417730742919-mth5klvuievpjh584agduun3dpn4680o.apps.googleusercontent.com', // client id provided by google
            hosted_domain: 'carwale.com',
            response_type: 'token',
            /* ----- this properties of api can be use for redirection to uri in case we need to redirect -----
            ux_mode: 'redirect',
            redirect_uri: 
            */
        });
        attachSignin(document.getElementById('customBtn'));
    });
};

function attachSignin(element) {
    auth2.attachClickHandler(element, {},
        function (googleUser) {
            if (googleUser.getHostedDomain().toLowerCase() == "carwale.com") {
                location.href = '/users/authenticate/?idtoken=' + googleUser.getAuthResponse().id_token + '&returnUrl=' + getUrlParameter('returnUrl');
            }
            else {
                showErrorMessage();
            }
        }, function (error) {
            showErrorMessage();
        });
}

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;
    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0].toLowerCase() === sParam.toLowerCase()) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

function showErrorMessage() {
    Materialize.toast("Wrong Username or password entered", 5000);
}
