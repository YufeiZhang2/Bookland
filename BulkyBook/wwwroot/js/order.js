let dataTable;

$(document).ready(function () {
    let ordersByStatusPath = window.location.search;
    if(ordersByStatusPath.includes("inprocess")){
        loadDataTable("GetOrderList?status=inprocess");
    }
    else if(ordersByStatusPath.includes("completed")){
        loadDataTable("GetOrderList?status=completed");
    }
    else if(ordersByStatusPath.includes("pending")){
        loadDataTable("GetOrderList?status=pending");
    }
    else if(ordersByStatusPath.includes("rejected")){
        loadDataTable("GetOrderList?status=rejected");
    }
    else if(ordersByStatusPath.includes("all")){
        loadDataTable("GetOrderList?status=all");
    }
});

function loadDataTable(ordersByStatusPath) {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Order/" + ordersByStatusPath 
        },
        "columns": [
            {"data": "id", "width": "10%"},
            {"data": "name", "width": "15%"},
            {"data": "phoneNumber", "width": "15%"},
            {"data": "applicationUser.email", "width": "15%"},
            {"data": "orderStatus", "width": "15%"},
            {"data": "orderTotal", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                        <a href="/Admin/Order/Details/${data}" class="btn btn-success text-white" style="cursor: pointer">
                            <i class="fas fa-edit"></i>
                        </a>
                    </div>
                    `;
                }, "width":"5%"
            }
        ]
    })
}
