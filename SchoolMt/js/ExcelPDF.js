
function divToPDF(divName) {
    divName = "#" + divName;
    var a = document.getElementById('dataDiv').value;
    var width = Math.round([297, 210] * 300 / 25.4);
    var height = Math.round([841, 594] * 300 / 25.4);
    var pdf = new jsPDF('p', 'pt', 'a4'),
    source = $(divName)[0],
    margins = {
        top: 40,
        bottom: 40,
        left: 40,
        right: 40,
        width: 1000
    };
    pdf.fromHTML(
            source,
            
            margins.left,
            margins.top,
            {
                'width': margins.width
            },
            function (dispose) {
                pdf.save('Test.pdf');
            },
            margins
       );
};

function demoFromHTMLForCTS(table, name, removecol) {
    if (typeof (removecol) === 'undefined') {
        removecol = [];

    }
    var now = new Date(Date.now());
    debugger;
    var dt = new Date();
    var day = dt.getDate();
    var month = dt.getMonth() + 1;
    var year = dt.getFullYear();
    var hour = dt.getHours();
    var mins = dt.getMinutes();
    var postfix = day + "." + month + "." + year + "_" + hour + "." + mins;
    var pdfsize = 'a2';
    var pdf = new jsPDF('l', 'pt', pdfsize);
    //pdf.context2d.pageWrapY = pdf.internal.pageSize.height - 20;

    var res = pdf.autoTableHtmlToJson(document.getElementById(table));

    debugger;
    var arrData = [];
    var arrCol;
    var i = 0;
    if (removecol.length > 0) {
        var get_index = [];
        for (var j = 0; j < removecol.length; j++) {
            arrCol = $.grep(res.columns, function (e, m) {
                if (e == removecol[j]) {
                    get_index.push(m);
                }
                return e != removecol[j];
            });

        }
        while (i < res.data.length) {
            var arrCol1 = $.grep(res.data[i], function (e, m) {
                return get_index.indexOf(m);
            });
            arrData[i] = arrCol1;
            i++;
        }

    }
    else {
        arrCol = res.columns;
        arrData = res.data;
    }
    //var i = 0;
    //while (i < res.data.length)
    //{
    //    res.data.splice
    //}
    pdf.autoTable(arrCol, arrData, {
        drawHeaderCell: function (cell, data) {
            //if (cell.raw === 'ID') {//paint.Name header red
            //    cell.styles.fontSize= 15;
            //    cell.styles.textColor = [255,0,0];
            //} else {
            cell.styles.textColor = 255;
            cell.styles.fontSize = 15;


        },
        tableWidth: 'auto',
        columnWidth: 'wrap',
        createdCell: function (cell, data) {
            //if (cell.raw === 'Jack') {
            cell.styles.fontSize = 15;
            //cell.styles.textColor = [255,0,0];
            // } 
        },
        startY: 20,
        styles: {
            overflow: 'linebreak',
            fontSize: 15,
            rowHeight: 40,
            //columnWidth: 'wrap'
        },

        //columnStyles: {
        //    1: { columnWidth: 'auto' }
        //}

    });

    pdf.save(name + postfix + ".pdf");
}
function demoFromHTML(table, name, removecol) {
    if (typeof(removecol)==='undefined')
        {removecol = [];
    
    }
    var now = new Date(Date.now());
    debugger;
    var dt = new Date();
    var day = dt.getDate();
    var month = dt.getMonth() + 1;
    var year = dt.getFullYear();
    var hour = dt.getHours();
    var mins = dt.getMinutes();
    var postfix = day + "." + month + "." + year + "_" + hour + "." + mins;
    var pdfsize = 'a2';
    var pdf = new jsPDF('l', 'pt', pdfsize);
    //pdf.context2d.pageWrapY = pdf.internal.pageSize.height - 20;

    var res = pdf.autoTableHtmlToJson(document.getElementById(table));
   
    debugger;
    var arrData = [];
    var arrCol;
    if (removecol.length > 0) {
        while (i < res.data.length) {
            arrData[i] = $.grep(res.data[i], function (n, i) {
                return $.inArray(i, removecol) == -1;
            });
            i++;
        }
        arrCol = $.grep(res.columns, function (n, i) {
            return $.inArray(i, removecol) == -1;
        });
    }
    else
    {
        arrCol = res.columns;
        arrData = res.data;
    }
    //var i = 0;
    //while (i < res.data.length)
    //{
    //    res.data.splice
    //}
    pdf.autoTable(arrCol, arrData, {
        drawHeaderCell: function (cell, data) {
            //if (cell.raw === 'ID') {//paint.Name header red
            //    cell.styles.fontSize= 15;
            //    cell.styles.textColor = [255,0,0];
            //} else {
                cell.styles.textColor = 255;
                cell.styles.fontSize = 15;

            
        },
        tableWidth: 'auto',
        columnWidth: 'wrap',
        createdCell: function (cell, data) {
            //if (cell.raw === 'Jack') {
                cell.styles.fontSize= 15;
                //cell.styles.textColor = [255,0,0];
           // } 
        },
        startY: 20,
        styles: {
            overflow: 'linebreak',
            fontSize: 15,
            rowHeight: 40,
            //columnWidth: 'wrap'
        },
       
        //columnStyles: {
        //    1: { columnWidth: 'auto' }
        //}

    });

    pdf.save(name + postfix + ".pdf");
}
function ResultsToTable(table, name) {
    //var now = new Date(Date.now());
    //var formatted = now.getDate().toString() + now.getMonth().toString() + now.getFullYear().toString() + now.getHours().toString() + now.getMinutes().toString();
    //$("#data").table2excel({
    //    exclude: ".noExl",
    //    name: name + formatted
    //});

   
    var dt = new Date();
    var day = dt.getDate();
    var month = dt.getMonth() + 1;
    var year = dt.getFullYear();
    var hour = dt.getHours();
    var mins = dt.getMinutes();
    var postfix = day + "." + month + "." + year + "_" + hour + "." + mins;
    var a = document.createElement('a');
    var data_type = 'data:application/vnd.ms-excel';
    var table_div = document.getElementById(table);
    var table_html = table_div.outerHTML.replace(/ /g, '%20');
    a.href = data_type + ', ' + table_html;
    //setting the file name
    a.download = name + postfix + '.xls';
    a.click();

}