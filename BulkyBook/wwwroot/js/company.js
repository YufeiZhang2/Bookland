let dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            {"data": "name", "width": "15%"},
            {"data": "streetAddress", "width": "15%"},
            {"data": "city", "width": "15%"},
            {"data": "state", "width": "15%"},
            {"data": "postalCode", "width": "15%"},
            {"data": "phoneNumber", "width": "15%"},
            {"data": "isAuthorisedCompany", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                        <a href="/Admin/Company/Upsert/${data}" class="btn btn-success text-white" style="cursor: pointer">
                            <i class="fas fa-edit"></i>
                        </a>
                         <a onclick=Delete("/Admin/Company/Delete/${data}") class="btn btn-danger text-white" style="cursor: pointer">
                            <i class="fas fa-trash-alt"></i>
                         </a>
                    </div>
                    `;
                }, "width":"15%"
            }
        ]
    })
}

function Delete(url){
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then(isDelete => {
        if(isDelete){
            $.ajax({
                type:"Delete",
                url: url,
                success: function (data){
                    if(data.success){
                        swal({
                            title: "Successfully",
                            text: data.message,
                            icon: "success",
                        });
                        dataTable.ajax.reload();
                    }
                    else{
                        swal({
                            title: "Sorry",
                            text: data.message,
                            icon: "error",
                        });
                    }
                }
            })
        }
    })
}