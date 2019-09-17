var apiUrl = 'https://localhost:5001/v1/';
var _userStoreKey = 'OnlineTrackUser';
var __onlineUser = null;

function post(request, data, authenticated) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', apiUrl + request);
    xhr.setRequestHeader('Content-Type', 'application/json');

    return new Promise(function(resolve, reject) {
        xhr.onreadystatechange = function() {
            if (
                this.readyState === 4 &&
                this.status >= 200 &&
                this.status < 300
            ) {
                resolve(JSON.parse(this.responseText));
            }
        };

        xhr.onerror = function() {
            reject(this.responseText);
        };

        try {
            xhr.send(JSON.stringify(data));
        } catch (e) {
            reject(e);
        }
    });
}

function login(user) {
    localStorage.setItem(_userStoreKey, JSON.stringify(user));
    __onlineUser = user;
}

function isLoggedIn() {
    if(__onlineUser) {
        return true;
    } else {
        var localUser = localStorage.getItem(_userStoreKey);
        if(localUser) {
            login(JSON.parse(localUser));
            return true;
        } else {
            return false;
        }
    }
}

function checkLogin() {
    if(isLoggedIn()) {
        return;
    } else {
        location.href = "/index.html";
    }
}

function loginRedirect() {
    if(isLoggedIn()) {
        location.href = "/home.html";
    } else {
        return;
    }
}