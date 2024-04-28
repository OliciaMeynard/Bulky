var dataTable;

$(document).ready(function () {
    loadDataTable();
})


////DATATABLE
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall'},
        "columns": [

            { data: 'name', "width": "25%" },
            { data: 'email', "width": "10%" },
            { data: 'phoneNumber', "width": "5%" },
            { data: 'company.name', "width": "15%" },
            { data: 'role', "width": "10%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd"},
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {

                        return `<div class="text-center">

                                    <a onClick=lockunlock('${data.id}') class="btn btn-danger text-white" style="cursor: pointer; width: 100px;">
                                        <i class="bi bi-lock-fill"></i> locked
                                    </a>
                                    <a href="/Admin/User/RoleManagement?userId=${data.id}" class="btn btn-danger text-white" style="cursor: pointer; width: 150px;">
                                        <i class="bi bi-pencil-square"></i> Permission
                                    </a>
                                </div>`
                    } else {
                        return `<div class="text-center">
                                     <a onClick=lockunlock('${data.id}') class="btn btn-success text-white" style="cursor: pointer; width: 100px;">
                                        <i class="bi bi-unlock-fill"></i> unlock
                                    </a>

                                    <a href="/Admin/User/RoleManagement?userId=${data.id}" class="btn btn-danger text-white" style="cursor: pointer; width: 150px;">
                                        <i class="bi bi-pencil-square"></i> Permission
                                    </a>
                                </div>`
                    }


                },
                "width": "40%" },

        ]
    });
}

/////SWEETALERT
function lockunlock(id) {
  
                 $.ajax({
                    type: 'POST',
                    url: '/Admin/User/LockUnlock',
                    data: JSON.stringify(id),
                    contentType: 'application/json',
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message);
                            dataTable.ajax.reload();
                        }
                    }

            });

}


