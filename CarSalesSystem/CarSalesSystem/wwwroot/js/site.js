$(document).ready(function () {
    $("#selectBrandId").data('pre', $(this).val());
    $("#selectBrand2Id").data('pre', $(this).val());
    $("#selectRegionId").data('previousRegion', $(this).val());

    $("#selectBrandId").change(function () {
        var beforeChange = $(this).data('pre');
        if (this.value === beforeChange) return;

        $("#selectModelId").empty();
        $.ajax({
            type: 'GET',
            url: '/Advertisement/GetModels',
            dataType: 'json',
            data: { brandId: $("#selectBrandId").val() },
            success: function (models) {
                $("#selectModelId").append('<option value="">' + '--None--' + '</option>');
                $.each(models,
                    function (i, model) {
                        $("#selectModelId").append('<option value="' + model.id + '">' + model.name + '</option>');
                    });
                $("#selectModelId").val($("#selectModelId option:first").val());
            },
            error: function (ex) {
                alert('Failed to retrieve models.' + ex);
            }
        });

        $(this).data('pre', $(this).val());

        return false;
    });

    $("#selectBrand2Id").change(function () {
        var beforeChange = $(this).data('pre');
        if (this.value === beforeChange) return;

        $("#selectModel2Id").empty();
        $.ajax({
            type: 'GET',
            url: '/Advertisement/GetModels',
            dataType: 'json',
            data: { brandId: $("#selectBrand2Id").val() },
            success: function (models) {
                $("#selectModel2Id").append('<option value="">' + '--None--' + '</option>');
                $.each(models,
                    function (i, model) {
                        $("#selectModel2Id").append('<option value="' + model.id + '">' + model.name + '</option>');
                    });
                $("#selectModel2Id").val($("#selectModel2Id option:first").val());
            },
            error: function (ex) {
                alert('Failed to retrieve models.' + ex);
            }
        });

        $(this).data('pre', $(this).val());

        return false;
    });


    $("#selectRegionId").change(function () {
        var beforeChange = $(this).data('previousRegion');
        if (this.value === beforeChange) return;

        console.log($("#selectRegionId").val());
        $("#selectCityId").empty();
        $.ajax({
            type: 'GET',
            url: '/Advertisement/GetAllCities',
            dataType: 'json',
            data: { regionId: $("#selectRegionId").val() },
            success: function (models) {
                $("#selectCityId").append('<option value="">' + '--None--' + '</option>');
                $.each(models,
                    function (i, model) {
                        $("#selectCityId").append('<option value="' + model.id + '">' + model.name + '</option>');
                    });
                $("#selectCityId").val($("#selectCityId option:first").val());
            },
            error: function (ex) {
                alert('Failed to retrieve models.' + ex);
            }
        });

        $(this).data('previousRegion', $(this).val());
    });
});

function addOrRemoveFromFavoriteAdvertisement(advertisementId) {
    var removeFromFavId = "#removeFromFavorite_" + advertisementId;
    var addToFavId = "#addToFavorite_" + advertisementId;
    $.ajax({
        type: 'POST',
        url: '/User/AddAdvertisementToFavorite',
        dataType: 'json',
        data: { advertisementId: advertisementId },
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        success: function (result) {
            if (result) {
                $(addToFavId).css('display', 'none');
                $(removeFromFavId).css('display', 'block');
            } else {
                $(addToFavId).css('display', 'block');
                $(removeFromFavId).css('display', 'none');
            }
        },
        error: function (ex) {
            if (ex.status == 401) {
                window.location = "https://localhost:44300/Identity/Account/Login/";
            } else {
                alert('Failed to perform operation.' + ex);
            }
        }
    });
}