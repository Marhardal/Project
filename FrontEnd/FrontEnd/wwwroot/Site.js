window.SwalService = {
    _getMixin: function () {
        return Swal.mixin({
            toast: true,
            position: "top",
            width: "auto",
            maxWidth: "450px",  // Control maximum width
            padding: "0.75rem 1.25rem",
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            customClass: {
                container: 'fixed-swal-container',
                popup: 'fixed-swal-popup'
            },
            didOpen: (toast) => {
                // Fix positioning and width
                setTimeout(() => {
                    const container = document.querySelector('.fixed-swal-container');
                    if (container) {
                        container.style.left = '50%';
                        container.style.transform = 'translateX(-50%)';
                        container.style.right = 'auto';
                        container.style.top = '0';
                        container.style.bottom = 'auto';
                        container.style.width = 'auto';
                        container.style.maxWidth = '450px';
                    }
                    
                    const popup = document.querySelector('.fixed-swal-popup');
                    if (popup) {
                        popup.style.maxWidth = '450px';
                        popup.style.width = 'auto';
                        popup.style.minWidth = '200px';
                        popup.style.whiteSpace = 'normal';
                        popup.style.wordBreak = 'break-word';
                    }
                }, 10);
                
                toast.onmouseenter = Swal.stopTimer;
                toast.onmouseleave = Swal.resumeTimer;
            }
        });
    },

    success: function (message) {
        this._getMixin().fire({
            icon: "success",
            title: message
        });
    },

    error: function (message) {
        return Swal.fire({
            icon: 'error',
            title: 'Error',
            text: message
        });
    },

    confirm: function (message) {
        return Swal.fire({
            icon: 'warning',
            title: 'Are you sure?',
            text: message,
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'Cancel'
        }).then(result => result.isConfirmed);
    }
};

window.Navigation = {
    back: () => history.go(-1),
    go: (steps) => history.go(steps),
    forward: () => history.go(1),
}

window.Spinner = {
    show: () => document.getElementById('global-spinner').style.display = 'flex',
    hide: () => document.getElementById('global-spinner').style.display = 'none',
}

window.Storage = {
    setToken: (token) => {
        const payload = {
            token: token,
            expiry: new Date().getTime() + (30 * 60 * 1000) // 30 min from now
        };
        localStorage.setItem('authToken', JSON.stringify(payload));
    },

    getToken: () => {
        const item = localStorage.getItem('authToken');
        if (!item) return null;

        const payload = JSON.parse(item);
        if (new Date().getTime() > payload.expiry) {
            localStorage.removeItem('authToken');
            return null; // token expired
        }

        return payload.token;
    },

    clearToken: () => {
        localStorage.removeItem('authToken');
    }
}

window.Inactivity = {
    _timer: null,
    _dotnet: null,
    _minutes: 30,

    start: (minutes, dotnet) => {
        Inactivity._minutes = minutes;
        Inactivity._dotnet = dotnet;
        Inactivity.reset();

        ['mousemove', 'keydown', 'mousedown', 'touchstart', 'scroll', 'click']
            .forEach(event =>
                document.addEventListener(event, Inactivity.reset, true)
            );
    },

    reset: () => {
        clearTimeout(Inactivity._timer);
        Inactivity._timer = setTimeout(() => {
            Inactivity.logout();
        }, Inactivity._minutes * 60 * 1000);
    },

    logout: () => {
        // Clear token from local storage
        localStorage.removeItem('authToken');
        // or if you clear everything
        localStorage.clear();

        Inactivity._dotnet.invokeMethodAsync('OnInactive');
    },

    stop: () => {
        clearTimeout(Inactivity._timer);
        ['mousemove', 'keydown', 'mousedown', 'touchstart', 'scroll', 'click']
            .forEach(event =>
                document.removeEventListener(event, Inactivity.reset, true)
            );
    }
}

window.downloadFile = (fileName, contentType, bytes) => {
    console.log('downloadFile called', fileName, bytes?.length);
    const blob = new Blob([new Uint8Array(bytes)], { type: contentType });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
}