$('button#submit').click(function() {
    $.ajax({
        type: "POST",
        url: "/api/rest/iis/siteAdd",
        data: $('form#site').serialize(),
        success: function() {
            $('div#add-site-modal').modal('hide');
            $('table#sites').bootstrapTable('refresh');
        },
        error: function(data, status, error) {
            alert(error);
        }
    });
});