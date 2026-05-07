window.SwalService = {
    //success: function (message) {
    //    return Swal.fire({
    //        icon: 'success',
    //        title: 'Success',
    //        text: message,
    //        timer: 2000,
    //        showConfirmButton: false
    //    });
    //},
    toast: true,
    position: "center", // 👈 change this
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.onmouseenter = Swal.stopTimer;
        toast.onmouseleave = Swal.resumeTimer;
    }

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