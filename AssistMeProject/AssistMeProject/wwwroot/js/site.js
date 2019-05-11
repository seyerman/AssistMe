// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Answer Section

/**
 * 
 * @param {string} interactionType "iv": Interesting Vote, "pv": Positive Vote, "v": View
 * @param {any} userID 
 * @param {any} elementID
 * @param {any} btnID DOM id (Document Object for Javascipt)
 */
function interaction(interactionType = "v", userID, elementID, btnID) {

    url = "";
    if (interactionType == "iv") {
        url = "/InterestingV/Create?UserID=" + userID + "&QuestionID=" + elementID
        var iconSelectedClass = "icon-star";
        var iconNoSelectedClass = "icon-star-empty";
        var btn = $("#" + btnID);
        var icon = btn.find("i");
        var contador = btn.find("span");

        $.ajax({ url: url })
            .done(function (data) {

                switch (data) {
                    case -1:
                       
                        contador[0].innerHTML = (Number.parseInt(contador[0].innerHTML) - 1);
                        icon.addClass(iconNoSelectedClass);
                        icon.removeClass(iconSelectedClass);
                        break;
                    case 1:
                        
                        contador[0].innerHTML = (Number.parseInt(contador[0].innerHTML) + 1);
                    
                        icon.addClass(iconSelectedClass);
                        icon.removeClass(iconNoSelectedClass);
                        break;
                    case 0: break;
                }
            }).fail(function () { alert("error" + url); });
    }

    if (interactionType == "pv") {
        url = "/PositiveV/Create?UserID=" + userID + "&AnswerID=" + elementID
        var btn = $("#" + btnID);
        var icon = btn.find("i");
        contador = btn.find("span");

        $.ajax({ url: url })
            .done(function (data) {

                switch (data) {
                   case -1:
                        contador[0].innerHTML = (Number.parseInt(contador[0].innerHTML) - 1);
                        icon.addClass(iconNoSelectedClass);
                        icon.removeClass(iconSelectedClass);
                        break;
                    case 1:
                        contador[0].innerHTML = (Number.parseInt(contador[0].innerHTML) + 1);
                        console.log((Number.parseInt(contador[0].innerHTML) + 1));                    
                        break;
                    case 0: break;
                }
            }).fail(function () { alert("error" + url); });
    }


    
    

   


    
}
