var dataTable;

$(document).ready(function () {
    loadDataTable();
})


////DATATABLE
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall'},
        "columns": [
            //{ data: 'name' },
            //{ data: 'position' },
            //{ data: 'salary' },
            //{ data: 'office' }
            {
                data: 'imageUrl',
                "render": function (data) {

                    if (data == "") {
                        return `<img src="\\images\\placeholderimage.png" alt="image" style="width: 50px;" />`
                    }
                    else {
                        return `<img src="${data}" alt="image" style="width: 50px;" />`
                    }

                   // return `<img src="${data}" alt="@obj.Title" style="width: 50px;" />`
                },
                "width": "20%"
            },
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "10%" },
            { data: 'price', "width": "5%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group role="group"">
                            <a href="/admin/product/upsert?id=${data}" asp-controller="Product" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Edit</a>
                            <a onClick=deleteProduct('/admin/product/deleteapi/?id=${data}') class="btn btn-danger mx-2"><i class="bi bi-x-octagon-fill"></i> Delete</a>
                            </div>`
                },
                "width": "20%" },

        ]
    });
}

/////SWEETALERT
function deleteProduct(url) {
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
