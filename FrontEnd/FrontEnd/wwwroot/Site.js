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