let arrow = document.querySelectorAll(".arrow");

for (var i = 0; i < arrow.length; i++) {
    arrow[i].addEventListener("click", (e) => {
        let arrowParent = e.target.parentElement.parentElement;//selecting main parent of arrow
        arrowParent.classList.toggle("showMenu");
    });
}
let sidebar = document.querySelector(".sidebar");
let sidebarBtn = document.querySelector(".logo-icon");
console.log(sidebarBtn);
sidebarBtn.addEventListener("click", () => {
    sidebar.classList.toggle("close");
});

let sidebarBtn1 = document.querySelector(".logo-icon");
console.log(sidebarBtn1);
sidebarBtn1.addEventListener("click", () => {
    expand_section.classList.toggle("expand_sec");
});



function myFunction() {

    var element = document.getElementById("navbarSupportedContent");
    element.classList.remove("close");

    var element = document.getElementById("expand_section");
    element.classList.add("expand_sec");

}

function myFunction1() {

    var element = document.getElementById("expand_section");
    element.classList.remove("expand_sec");
    //var element = document.getElementById("navbarSupportedContent");

    var element = document.getElementById("navbarSupportedContent");
    element.classList.add("close");

    var element = document.getElementById("navbarpopupoverlay");
    element.classList.remove("popupoverlaymenu");



}

function myFunction2() {

    var element = document.getElementById("navbarSupportedContent");
    element.classList.remove("close");

    var element = document.getElementById("navbarpopupoverlay");
    element.classList.add("popupoverlaymenu");



}


function searchMenu(txt) {
    var menu, filter, a, menucount;
    filter = $(txt).val();
    var data = document.getElementsByClassName("sub-menu")
    var formname = document.getElementsByClassName("link_name")
    for (k = 0; k < formname.length; k++) {
        if (formname[k].parentElement.parentElement.nextElementSibling != null) {
            for (i = 0; i < data.length; i++) {
                menucount = 0;
                menu = data[i].getElementsByTagName("a");
                if (menu.length > 0) {
                    for (j = 0; j < menu.length; j++) {
                        if (menu[j].innerHTML.toUpperCase().indexOf(filter.toUpperCase()) > -1) {
                            menu[j].style.display = "";
                            menucount = menucount + 1;
                        }
                        else {
                            menu[j].style.display = "none";
                        }
                    }
                    if (parseInt(menucount) > 0) {
                        data[i].parentElement.style.display = "";
                    }
                    else {
                        data[i].parentElement.style.display = "none";
                    }
                }
                //else
                //{
                //    if (data[i].innerHTML.toUpperCase.indexOf(filter.toUpperCase()) > -1)
                //    {

                //    }
                //    else
                //    {
                //    }
                //}

            }
        }
        else {
            if (formname[k].innerHTML.toUpperCase().indexOf(filter.toUpperCase()) > -1) {
                formname[k].parentElement.style.display = "";
            }
            else {
                formname[k].parentElement.style.display = "none";
            }
        }
    }
}

