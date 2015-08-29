$('button#submitSingle').click(function () {
    //Build object
    var siteRestriction = {
        Site: {
            Sitename: $("input#Sitename").val(),
            Hostname: $("input#Hostname").val(),
            Group: undefined
        },
        Restriction: {
            Type: $("input#st_Type").val(),
            Rule: $("input#st_Rule").val(),
            StartMoment: $("input#st_StartMoment").val(),
            StopMoment: $("input#st_StopMoment").val(),
            CreateMoment: undefined
        }
    };
    
    $.ajax({
        type: "POST",
        url: "/api/rest/iis/siteRestrictionAdd",
        data: JSON.stringify(siteRestriction),
        contentType: 'application/json; charset=utf-8',
        success: function () {
            $('div#add-restriction-modal').modal('hide');
            $('table#restrictions').bootstrapTable('refresh');
        },
        error: function (data, status, error) {
            alert(error);
        }
    });
});

$('button#submitGroup').click(function () {
    //Build object
    var groupRestriction = {
        Group: $("input#Group").val(),
        Restriction: {
            Type: $("input#gr_Type").val(),
            Rule: $("input#gr_Rule").val(),
            StartMoment: $("input#gr_StartMoment").val(),
            StopMoment: $("input#gr_StopMoment").val(),
            CreateMoment: undefined
        }
    };

    $.ajax({
        type: "POST",
        url: "/api/rest/iis/groupRestrictionAdd",
        data: JSON.stringify(groupRestriction),
        contentType: 'application/json; charset=utf-8',
        success: function () {
            $('div#add-group-restriction-modal').modal('hide');
            $('table#restrictions').bootstrapTable('refresh');
        },
        error: function (data, status, error) {
            alert(error);
        }
    });
});