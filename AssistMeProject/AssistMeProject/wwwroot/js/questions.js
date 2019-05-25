var list = undefined,
    typeList = undefined,
    elementID = undefined,
    filterElement = undefined,
    defaultOrder = undefined, // {attr:string, order=1}
    tempInfo = { uid: undefined, type: undefined, qid: undefined },
    filterOptions = {};



/**
 * Función de site.js
 * Envía una petición al servidor para marcar una respuesta como correcta.
 * @param {any} uid ID de usuario que marca la respuesta como correcta.
 * @param {any} aid ID de la respuesta a marcar.
 */
function markAsCorrect(uid=getUserID(), aid) {
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

function initializeDefault(e_id, u_id=getUserID(), t, q_id) {
    elementID = e_id;
    tempInfo.uid = u_id;
    tempInfo.type = t;
    tempInfo.qid = q_id;
    if (typeList == undefined) typeList = t;
    filterElement = getElementFIlter("filter" + elementID);
}
function getElementFIlter(ef_id) {
    var filter = document.getElementById(ef_id);
    filter.innerHTML = `
 <div class="filter-menu">
                <a class="btn-filter flex11 filter-option" data-toggle="collapse" href="#multiCollapseOrder" role="button" aria-expanded="false" aria-controls="multiCollapseOrder">
                    Order <span class="extra"></span>
                </a>
                <a class="btn-filter flex11 filter-option" data-toggle="collapse" href="#multiCollapseFilter" role="button" aria-expanded="false" aria-controls="multiCollapseFilter">
                    Filter <span class="extra"></span>                        
                </a>
            </div>
            <div class="">
                <div class="collapse multi-collapse" id="multiCollapseOrder">
                    <span>Order by:</span>
                    <div class="order-selector">
                        <a class="order-tiem" href="javascript:sortList('date')">Date</a>
                        <a class="order-tiem" href="javascript:sortList('votes')">Votes</a>
                        <a class="order-tiem" href="javascript:sortList('description')">Content</a>
                        <a class="order-tiem" href="javascript:sortList('comments')">Comments</a>
                    </div>
                </div>
                <div class="collapse multi-collapse" id="multiCollapseFilter">
                    <span>Filter by:</span>
                    <div class="order-selector selector-container">
                        <a class="order-tiem" href="#filter-section-date" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="filter-section-date">Date</a>
                        <a class="order-tiem" href="#filter-section-votes" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="filter-section-votes">Votes</a>
                        <a class="order-tiem" href="#filter-section-reply" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="filter-section-reply">Replies</a>
                        <a class="order-tiem" href="#filter-section-studio" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="filter-section-studio">Studio</a>
                    </div>
                    <div class="filter-selector selector-container">
                        <div class="collapse  multi-collapse filter-section " id="filter-section-date">
                            <span>Select Dates:</span>
                            <div class="flex">
                                <div class="opt date-opt">
                                    <label class="date-item" for="filter-section-date-since">Since</label>
                                    <input class="date-item" id="filter-section-date-since" type="date" name="since" value="" />
                                </div>
                                <div class="opt date-opt">
                                    <label class="date-item" for="filter-section-date-until">Until</label>
                                    <input class="date-item" id="filter-section-date-until" type="date" name="until" value="" />
                                </div>
                            </div>
                        </div>
                        <div class="collapse  multi-collapse filter-section " id="filter-section-votes">
                            <span>Votes Options:</span>
                            <div class="flex">
                                <div class="opt">
                                    <input type="radio" name="filter-section-votes-count" value="" checked> clear
                                </div>
                                <div class="opt">
                                    <input type="radio" name="filter-section-votes-count" value="any"> Any
                                </div>
                                <div class="opt">
                                    <input type="radio" name="filter-section-votes-count" value="no"> None
                                </div>
                                <div class="opt">
                                    <input type="radio" name="filter-section-votes-count" value="other"> At Least <input type="number" id="filter-section-votes-count" value="0">
                                </div>
                            </div>
                        </div>
                        <div class="collapse  multi-collapse filter-section " id="filter-section-reply">
                            <span>Replies Options:</span>
                            <div class="flex">
                                <div class="opt">
                                    <input type="radio" name="filter-section-reply-count" value="" checked> clear
                                </div>
                                <div class="opt">
                                    <input type="radio" name="filter-section-reply-count" value="any"> Any
                                </div>
                                <div class="opt">
                                    <input type="radio" name="filter-section-reply-count" value="no"> None
                                </div>
                                <div class="opt">
                                    <input type="radio" name="filter-section-reply-count" value="other"> At Least <input type="number" id="filter-section-reply-count" value="0">
                                </div>
                            </div>
                        </div>
                        <div class="collapse  multi-collapse filter-section " id="filter-section-studio">
                            Filter by studio:
                            <input class="" type="text" name="filter-section-studio-name" id="filter-section-studio-name" value="" />
                        </div>
                    </div>
                    <div class="flex">
                        <a class="btn-filter flex11" href="javascript:filterList()">
                            Apply Filters
                        </a>
                        <a class="btn-filter flex11" href="javascript:clearFilter()">
                            Clear Filters
                        </a>
                    </div>
                </div>
            </div>
    `;
    return filter;
}

document.addEventListener("DOMContentLoaded", function () {
    {
        var uid = getUserID();
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

function filterList() {
    var inputs = [
        { id: "filter-section-date-since", type: "input", name:"since" },
        { id: "filter-section-date-until", type: "input", name:"until" },
        { id: "filter-section-studio-name", type: "input", name:"studios" },
        { id: "filter-section-votes-count", type: "group", name:"votes" },
        { id: "filter-section-reply-count", type: "group", name:"reply" }
    ];
    inputs.forEach(i => {
        var val = getValue(i.id, i.type);
        if (val !== "")
            filterOptions[i.name] = val;
        else
            delete filterOptions[i.name];
    });
    getList(getUserID());
}
function clearFilter() {
    filterOptions = {};
    getList(getUserID());
}
function getValue(id, type = "input") {
    var val = "";
    if (type == "group") {
        val = document.querySelector('input[name="' + id + '"]:checked').value;
    }
    if (type == "input" || val == "other") {
        val = document.getElementById(id).value;
    }
    return val;
}


/**
 * 
 * @param {int} uid
 * @param {string} type "Questions": lista de preguntas, "Answers": Lista de respuestas de una pregunta.
 * @param {int} qid ID de la pregunta si es una lista de respuestas
 */
function getList(uid = getUserID(), type = typeList, qid = tempInfo.qid) {
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
    if (list == undefined) {
        defaultOrder = { attr: attr, order: order };
        getList(getUserID());
        return;
    } else if (list.length == 0) {
        updateElementList();
        return;
    }

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
            if (order == undefined) order = (defaultOrder != undefined && defaultOrder.attr == attr && defaultOrder.order != undefined) ? defaultOrder.order * -1 : - 1;
            fSorter = (a, b) => order * (a[attr] - b[attr]);
            break;
        case "string":
            if (order == undefined) order = (defaultOrder != undefined && defaultOrder.attr == attr && defaultOrder.order != undefined) ? defaultOrder.order * -1 : 1;
            fSorter = (a, b) => order * a[attr].localeCompare(b[attr]);
            break;
        case "object":
            if (list[0][attr].length !== undefined) {
                if (order == undefined) order = (defaultOrder != undefined && defaultOrder.attr == attr && defaultOrder.order != undefined) ? defaultOrder.order * -1 : -1;
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
    defaultOrder = { attr: attr, order: order };
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
    var color = obj.userVote==true? "#ff7361" : "#cccccc";

    ele.innerHTML = `
    <div class="comment-body comment-body-answered clearfix">
    <div class="avatar">
        <a href="#" original-title="${autor.name}" class="tooltip-n"><img alt="" src="${autor.img}"></a>
    </div>
    <div class="comment-text">
        <div class="author clearfix">
            <div class="comment-author"><a href="#">${autor.name}</a></div>
            <div class="comment-vote">
                <ul class="question-vote">
                    <li>
                        <span id="answer_vote_${obj.id}" onclick="interaction('pv', ${getUserID()} ,${obj.id},'answer_vote_${obj.id}' )" class="answer-favorite">
                            <i class="icon-plus" style="color:${color}"></i>
                            +<span style="display:initial">${obj.votes}</span>
                        </span>


                    </li>
                </ul>
            </div>
            <div class="comment-meta">
                <div class="date"><i class="icon-time"></i>${obj.date.split("T")[0]}</div>
            </div>
            <a asp-controller="Comments" asp-action="Create" asp-route-AnswerId="@Model.Id" class="comment-reply"><i class="icon-comment"></i>Comentar</a>
        </div>
        <div class="text">
            <p>${obj.description}</p>
        </div>
        <div id="answer_${obj.id}" class="question-answered pt ${obj.correctAnswer == true ? "question-answered-done" : ""}" onclick="markAsCorrect(1,${obj.id})"><i class="icon-ok"></i>${obj.correctAnswer == true ? "Correct" : "Mark as Correct"}</div>
</div>
</div>
<div id="commentlist_${obj.id}">
    <ol class="commentlist clearfix">
        ${commentListHTML}
    </ol>
</div>
    `;
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
