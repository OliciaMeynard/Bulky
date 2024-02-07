var dataTable;

$(document).ready(function () {
    loadDataTable();
})


////DATATABLE
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall'},
        "columns": [



            { data: 'name', "width": "35%" },
            { data: 'streetAddress', "width": "10%" },
            { data: 'city', "width": "10%" },
            { data: 'state', "width": "10%" },
            { data: 'postalCode', "width": "5%" },
            { data: 'phoneNumber', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group role="group"">
                            <a href="/admin/company/upsert?id=${data}" asp-controller="Company" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Edit</a>
                            <a onClick=deleteCompany('/admin/company/deleteapi/?id=${data}') class="btn btn-danger mx-2"><i class="bi bi-x-octagon-fill"></i> Delete</a>
                            </div>`
                },
                "width": "20%"
            },

        ]
    });
}

/////SWEETALERT
function deleteCompany(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {

                //title: "Deleted!",
                //text: "Your file has been deleted.",
                //icon: "success"

                $.ajax({
                    url: url,
                    type: 'DELETE',
                    success: function (data) {

                        ///reload the datatable
                        dataTable.ajax.reload();

                        toastr.success(data.message);
                    }

            });
        }
    });
}
