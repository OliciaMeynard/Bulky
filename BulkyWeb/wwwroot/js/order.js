var dataTable;

//$(document).ready(function () {
//    var url = window.location.search;
//    if (url.includes("inprocess")) {
//        loadDataTable("inprocess")
//    }
//    else {
//        if (url.includes("completed")) {
//            loadDataTable("completed")
//        }
//        else {
//            if (url.includes("pending")) {
//                loadDataTable("pending")
//            }
//            else {
//                if (url.includes("approved")) {
//                    loadDataTable("approved")
//                }
//                else {
//                    loadDataTable("all")
//                }

//            }

//        }

//    }


//})

////my code 


function checkUrl() {



    if (window.location.href.indexOf("inprocess").toString() != -1) {
        loadDataTable("inprocess")
    }
    else if (window.location.href.indexOf("completed").toString() != -1) {
        loadDataTable("completed")
    }
    else if (window.location.href.indexOf("pending").toString() != -1) {
        loadDataTable("pending")
    }
    else if (window.location.href.indexOf("approved").toString() != -1) {
        loadDataTable("approved")
    }
    else {
        loadDataTable("all")
    }
}
checkUrl()



////DATATABLE
function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status='+status },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "25%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group role="group"">
                            <a href="/admin/order/details?orderId=${data}" asp-controller="Order" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                            </div>`
                },
                "width": "20%"
            },

        ]
    });
}



///////OLD CODE NO FILTER
//function loadDataTable() {
//    dataTable = $('#tblData').DataTable({
//        "ajax": { url: '/admin/order/getall' },
//        "columns": [
//            { data: 'id', "width": "5%" },
//            { data: 'name', "width": "25%" },
//            { data: 'phoneNumber', "width": "10%" },
//            { data: 'applicationUser.email', "width": "20%" },
//            { data: 'orderStatus', "width": "10%" },
//            { data: 'orderTotal', "width": "10%" },
//            {
//                data: 'id',
//                "render": function (data) {
//                    return `<div class="w-75 btn-group role="group"">
//                            <a href="/admin/order/details?orderId=${data}" asp-controller="Order" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
//                            </div>`
//                },
//                "width": "20%"
//            },

//        ]
//    });
//}