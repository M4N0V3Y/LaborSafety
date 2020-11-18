(function () {
    function getJwt(user, pass, appKey, success, error, complete) {
        var body = `grant_type=password&username=${user}&password=${pass}`;

        var getUrl = window.location;
        var baseUrl = '/';

        if (getUrl.host.trim().toLowerCase().indexOf('localhost') === -1 && getUrl.host.indexOf('127.0.0.1') === -1) {
            baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1] + "/";
        }

        $.ajax({
            url: baseUrl + 'login',
            type: 'POST',
            beforeSend: function (request) {
                request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                request.setRequestHeader('Authorization-App', appKey);
            },
            data: body,
            success: function (data, status, xhr) {
                success && success(data.access_token);
            },
            error: function (jqXhr, err, msg) {
                debugger;
                var errorJx = JSON.parse(jqXhr.responseText);
                errorJx.error = JSON.parse(errorJx.error);

                error && error(errorJx.error.errorDetails);
            },
            complete: complete
        });
    }
    function setJwt(key, appKey) {
        swaggerUi.api.clientAuthorizations.authz = {};
        swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization("Authorization", "Bearer " + key, "header"));
        swaggerUi.api.clientAuthorizations.add("keyApp", new SwaggerClient.ApiKeyAuthorization("Authorization-App", appKey, "header"));
    }

    $(function () {

        $('#input_apiKey')
            .attr('placeholder', 'JWT | User,Pass,App Token')
            .off()
            .on('keyup', function (e) {
                e.preventDefault();

                if (e && e.keyCode) {
                    if (e.keyCode !== 13)
                        return;
                }

                var key = this.value;
                window.sessionStorage.setItem('key', key);

                var appToken = '';

                if (!key) {
                    key = '';
                }

                if (key.indexOf(',') > -1) {

                    var keySplit = key.split(',');

                    var user = keySplit[0];
                    var pass = keySplit[1];
                    appToken = keySplit[2];

                    $('#input_apiKey').prop("disabled", true);
                    getJwt(user, pass, appToken,
                        function (jwt) {
                            $('#input_apiKey').css('background', '#65f30f');
                            setJwt(jwt, appToken);
                        },
                        function (err) {
                            $('#input_apiKey').css('background', '#fd7474');
                            setJwt('');
                            alert(err);
                        }, function () {
                            $('#input_apiKey').prop("disabled", false);

                        });
                } else {
                    $('#input_apiKey').css('background', '#FFF');

                    setJwt(key, appToken);
                }
            });

        var oldKey = window.sessionStorage.getItem('key');
        if (oldKey) {
            $('#input_apiKey').val(oldKey).change();
        }
    });
})();