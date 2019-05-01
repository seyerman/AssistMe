var list = [],
    typeList = undefined,
    elementID = undefined,
    defaultOrder = undefined, // {attr:string, order=1}
    tempInfo = { uid: undefined, type: undefined, qid: undefined },
    filterOptions = {};


/**
 * Función de site.js
 * Envía una petición al servidor para marcar una respuesta como correcta.
 * @param {any} uid ID de usuario que marca la respuesta como correcta.
 * @param {any} aid ID de la respuesta a marcar.
 */
function markAsCorrect(uid, aid) {
    var url = "/api/Answers/Correct/" + aid + "/" + uid;

    $.ajax({ url: url })

        .done(function (data) {
            if (data.error == "") {
                var btn = $("#answer_" + aid);
                if (data.status == 1 || data.status == -1)
                    btn.toggleClass("question-answered-done");
            } else {
                console.log("Error:", data.error);
            }
        }).fail(function () { alert("Error: Error al marcar la respuesta como correcta" ); console.log("Error: En respuesta correcta " + url); });

}

function initializeDefault(e_id, u_id, t, q_id) {
    elementID = e_id;
    tempInfo.uid = u_id;
    tempInfo.type = t;
    tempInfo.qid = q_id;
    if (typeList == undefined) typeList = t;
}

document.addEventListener("DOMContentLoaded", function () {
    {
        var uid = 1;
        var path = document.location.pathname;
        if (path.includes("/Questions/List/")) {
            // initializeDefault("answerlist", uid, "Question", undefined);
        } else if (path.includes("/Questions/Details/")) {
            if (path[0] == "/") path = path.substr(1);
            path.replace("#", "?");
            path = path.split("?")[0];
            var p = path.split("/");
            initializeDefault("answerlist", uid, "Answers", p[p.length - 1]);
            console.log("Loaded for answers lists");
        }
        console.log("Loaded", path, path.includes("/Question/Details/"));
    }
});
/**
 * 
 * @param {int} uid
 * @param {string} type "Questions": lista de preguntas, "Answers": Lista de respuestas de una pregunta.
 * @param {int} qid ID de la pregunta si es una lista de respuestas
 */
function getList(uid, type = typeList, qid = tempInfo.qid) {
    if (type == undefined)
        type = "Questions";
    typeList = type;
    var url = "";
    if (type == "Questions")
        url = "/api/Questions/" + uid;
    else if (type == "Answers")
        var _qid = qid != undefined ? qid : tempInfo.qid != undefined ? tempInfo.qid : undefined;
            if (_qid != undefined)
                url = "/api/Answers/" + _qid + "/" + uid;
    if (url == "") {
        console.log("Error: " + type + " List");
        return;
    }
    url += "?" + objectToUrlParams(filterOptions);
    $.ajax({ url: url })
        .done(function (data) {
            list = data;
            console.log(data);
            if (defaultOrder != undefined)
                sortList(defaultOrder.attr, defaultOrder.order);
            else
                sortList("date");
        }).fail(function () { alert("Error: " + url); console.log("Error: " + url);});
}

function objectToUrlParams(obj = {}) {
    var params = "";
    Object.keys(obj).forEach(k => params += "&" + k + "=" + obj[k]);
    return params;
}

function sortList(attr, order = undefined) {
    if (list.length == 0)
        return;

    if (typeList == undefined) {
        if (tempInfo.uid != undefined)
            getList(tempInfo.uid, tempInfo.type, tempInfo.qid);
        else
            console.log("Error: No default info");
        return;
    }

    var fSorter = undefined;
    var t = typeof list[0][attr];
    switch (t) {
        case "number":
            if (order == undefined) order = -1;
            fSorter = (a, b) => order * (a[attr] - b[attr]);
            break;
        case "string":
            if (order == undefined) order = 1;
            fSorter = (a, b) => order * a[attr].localeCompare(b[attr]);
            break;
        case "object":
            if (list[0][attr].length !== undefined) {
                fSorter = (a, b) => order * (a[attr].length - b[attr].length);
            } else {
                console.log("Error: compare type " + t);
            }
            break;
        default:
            console.log("Error: compare type " + t);
            return;
            break;
    }
    list.sort(fSorter);
    updateElementList();
}


var questionHTML = function (obj) {
    
}

var answerHTML = function (obj) {
    var ele = document.createElement("li");
    ele.classList.add("comment");
    var autor = obj.autor ? obj.autor : {name:"admin", img: "http://placehold.it/60x60/FFF/444" };

    var commentListHTML = "";
    obj.comments.forEach(ob => commentListHTML += commentHTML(ob));
    

    ele.innerHTML = `
    <div class="comment-body comment-body-answered clearfix">
        <div class="avatar">
            <a href="#" original-title="admin" class="tooltip-n"><img alt="" src="${autor.img}"></a>
        </div>
        <div class="comment-text">
            <div class="author clearfix">
                <div class="comment-author"><a href="#">${autor.name}</a></div>
                    <div class="comment-vote">
                        <ul class="question-vote">
                            <li><a href="#" class="question-vote-up" title="Like"></a></li>
                        </ul>
                    </div>
                    <span class="question-vote-result">+${obj.votes}</span>
                        <div class="comment-meta">
                            <div class="date"><i class="icon-time"></i>${obj.date.split("T")[0]}</div>
                        </div>
                        <a href="/Comments/Create?AnswerId=${obj.questionID}" class="comment-reply"><i class="icon-comment"></i>Comentar</a>
                    </div>
                    <div class="text">
                        <p>${obj.description}</p>
                    </div>
                    <div class="question-answered question-answered-done"><i class="icon-ok"></i>Best Answer</div>
                </div>
            </div>
            <div>
           </div>
           <div id="commentlist_${obj.id}">
           <ol class="commentlist clearfix">
                ${commentListHTML}
           </ol>
        </div>`;
    return ele;


}

var commentHTML = function (obj) {
    var autor = obj.autor ? obj.autor : { name: "anom", img: "http://placehold.it/60x60/FFF/444" };
    var innerHTML = `
    <ul class="children">
        <li class="comment">
            <div class="comment-body clearfix">
                <div class="avatar">
                    <a href="#" original-title="admin" class="tooltip-n"><img alt="" src="${autor.img}"></a>
                </div>
                <div class="comment-text">
                    <div class="author clearfix">
                        <div class="comment-author"><a href="#">${autor.name}</a></div>
                        <div class="comment-meta">
                            <div class="date"><i class="icon-time"></i>${obj.date.split("T")[0]}</div>
                        </div>
                    </div>
                    <div class="text">
                        <p>${obj.description}</p>
                    </div>
                </div>
            </div>
        </li>
    </ul>
    `;

    return innerHTML;
}

var elementsHTML = {
    "Questions": questionHTML,
    "Answers": answerHTML
};

function updateElementList() {
    if (elementID == undefined) {
        alert("No se encuentra el contenedor a ordenar.");
        console.log("Error: No hay definido un contenedor para la lista de elementos.");
        return;
    }
    var listContainer = document.getElementById(elementID);
    listContainer.innerHTML = "";
    var content = document.createElement("div");
    content.classList.add("page-content");
    listContainer.appendChild(content);
    var ol = document.createElement("ol");
    ol.classList.add("commentlist", "clearfix");
    content.appendChild(ol);
    var getHTML = elementsHTML[typeList];
    list.forEach(ele => {
        var e = getHTML(ele);
        ol.appendChild(e);
    });
}
