$(document).ready(function () {

    var youtubeParser = function(url) {

        var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
        var match = url.match(regExp);
        if (match && match[2].length == 11) {
            return match[2];
        } else {
            return url;
        }

    }

    var formToObject = function (form) {
        var serializedForm = form.serializeArray();
        var formObject = {};
        $.each(serializedForm,
            function (i, v) {
                formObject[v.name] = v.value;
            });
        return formObject;
    }

    $('#addNewMaterial').click(function (e) {
        e.preventDefault();
        var queryAddress = "/api/material/new/" + $('#courseID').text();
        $.ajax({
            url: queryAddress, success: function (result) {
                $('#newMaterial').empty();
                $('#newMaterial').append(result);

                $('#jQuerySubmit').click(jQuerySubmitFunction);
            }
        });
    
    });
    /*
    $('#newMaterialForm').submit(function(e) {

        //prevent Default functionality
        e.preventDefault();

        //get the action-url of the form
        var actionurl = e.currentTarget.action;


        // parsowanie adresu dla filmu Youtube
        if ($('input[name=type]:checked', '#newMaterialForm').val() == "1") {
            var filePath = youtubeParser($('input[name=filePath]', '#newMaterialForm').val());
            $('#newMaterialForm input[name=filePath]').val(filePath);
        }

        // prepare JSON data
        var jsonData = $.toJSON(formToObject($('#newMaterialForm')));

        //do your own request an handle the results
        $.ajax({
            url: actionurl,
            type: 'post',
            dataType: 'json',
            data: jsonData,
            success: function(result) {
                $("#newMaterial").empty();
                $("#newMaterial").append(result);
            }
        });

    });
    */

    //$('#jQuerySubmit').click(function (e) {
    var jQuerySubmitFunction = function (e) {

        //prevent Default functionality
        e.preventDefault();

        //get the action-url of the form
        var actionurl = $('#newMaterialForm').attr("action");


        // parsowanie adresu dla filmu Youtube
        if ($('input[name=type]:checked', '#newMaterialForm').val() == "1") {
            var filePath = youtubeParser($('input[name=filePath]', '#newMaterialForm').val());
            $('#newMaterialForm input[name=filePath]').val(filePath);
        }

        // prepare JSON data
        var jsonData = $.toJSON(formToObject($('#newMaterialForm')));

        //do your own request an handle the results
        $.ajax({
            url: actionurl,
            type: 'post',
            dataType: 'json',
            data: jsonData,
            success: function (result) {
                $("#newMaterial").empty();
                $("#newMaterial").append(result);
            }
        });

    }//);

});