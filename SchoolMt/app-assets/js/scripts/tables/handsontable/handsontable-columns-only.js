
document.addEventListener("DOMContentLoaded", function() {

    /**********************************
     *      StretchH last column       *
     **********************************/

    var stretch = document.getElementById('stretch'),
        hot1;

    hot1 = new Handsontable(stretch, {
        data: Handsontable.helper.createSpreadsheetData(10, 6),
        colWidths: 47,
        rowHeaders: true,
        colHeaders: true,
        stretchH: 'last',
        contextMenu: true
    });



    /**********************
     *      Freezing       *
     **********************/

    var myData = Handsontable.helper.createSpreadsheetData(60, 100),
        container = document.getElementById('freezing'),
        hot;

    hot = new Handsontable(container, {
        data: myData,
        rowHeaders: true,
        colHeaders: true,
        fixedColumnsLeft: 2,
        contextMenu: true,
        manualColumnFreeze: true
    });

});
