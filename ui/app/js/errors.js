function showOnlineTrackError(msg) {
    // document -> body
    var body = document.body;

    // create an error box with msg
    var errorHtml = `
    <div class="error-online-track">
        <div class="msg">
            <p>${msg}</p>
        </div>
        <div onclick="closeError(this)" class="closer">
            <i class="material-icons">close</i>
        </div>
    </div>
    `;

    // add it to the body
    body.innerHTML += errorHtml;
}

function closeError(elm) {
    document.body.removeChild(elm.parentElement);
}