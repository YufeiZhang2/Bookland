let dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            {"data": "name", "width": "15%"},
            {"data": "email", "width": "15%"},
            {"data": "phoneNumber", "width": "15%"},
            {"data": "company.name", "width": "15%"},
            {"data": "role", "width": "15%"},
            {
                "data": {id: "id", lockoutEnd: "lockoutEnd"},
                "render": function (data) {
                    let today = new Date().getTime();
                    let lockout = new Date(data.lockoutEnd).getTime();
                    if(lockout > today){
                        return `
                    <div class="text-center">
                         <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor: pointer; width:80px">
                            <i class="fas fa-lock-open"> Unlock </i>
                         </a>
                    </div>
                    `;
                    }
                    else{
                        return `
                    <div class="text-center">
                         <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor: pointer; width:80px">
                            <i class="fas fa-lock"> Lock </i>
                         </a>
                    </div>
                    `
                    }
                }, "width":"25%"
            },
            {
                "data": "id",
                "render" : function(data){
                 return `
                    <div class="text-center">
                         <a onclick=Delete("/Admin/User/Delete/${data}") class="btn btn-danger text-white" style="cursor: pointer">
                            <i class="fas fa-trash-alt"></i>
                         </a>
                    </div>
                    `;
             },
                "width":"25%"
            }
        ]
    })
}

function LockUnlock(id){
    $.ajax({
        type:"Post",
        url: "/Admin/User/LockUnlock",
        data: JSON.stringify(id),
        contentType: "application/json",
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