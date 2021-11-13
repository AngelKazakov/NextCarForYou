$(document).ready(function () {
    $("#selectBrandId").change(function() {
        console.log($("#selectBrandId").val());
        $("#selectModelId").empty();
        $.ajax({
            type: 'GET',
            url: '/Advertisement/GetModels',
            dataType: 'json',
            data: { brandId: $("#selectBrandId").val() },
            success: function(models) {
                $("#selectModelId").append('<option value="">' + '--None--' + '</option>');
                $.each(models,
                    function(i, model) {
                        $("#selectModelId").append('<option value="' + model.id + '">' + model.name + '</option>');
                    });
                $("#selectModelId").val($("#selectModelId option:first").val());
            },
            error: function(ex) {
                alert('Failed to retrieve models.' + ex);
            }
        });
        return false;
    });
});

$(document).ready(function () {
    $("#selectRegionId").change(function() {
        console.log($("#selectRegionId").val());
        $("#selectCityId").empty();
        $.ajax({
            type: 'GET',
            url: '/Advertisement/GetAllCities',
            dataType: 'json',
            data: { regionId: $("#selectRegionId").val() },
            success: function(models) {
                $("#selectCityId").append('<option value="">' + '--None--' + '</option>');
                $.each(models,
                    function(i, model) {
                        $("#selectCityId").append('<option value="' + model.id + '">' + model.name + '</option>');
                    });
                $("#selectCityId").val($("#selectCityId option:first").val());
            },
            error: function(ex) {
                alert('Failed to retrieve models.' + ex);
            }
        });
    });
});
