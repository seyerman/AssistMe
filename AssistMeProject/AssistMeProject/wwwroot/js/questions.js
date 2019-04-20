var list = [],
    typeList = undefined,
    elementID = undefined,
    defaultOrder = undefined, // {attr:string, order=1}
    tempInfo = { uid: undefined, type: undefined, qid: undefined },
    filterOptions = {};

function initializeDefault(e_id, u_id, t, q_id) {
    elementID = e_id;
    tempInfo.uid = u_id;
    tempInfo.type = t;
    tempInfo.qid = q_id;
}

/**
 * 
 * @param {int} uid
 * @param {string} type "Questions": lista de preguntas, "Answers": Lista de respuestas de una pregunta.
 * @param {int} qid ID de la pregunta si es una lista de respuestas
 */
function getList(uid, type = "Questions", qid = undefined) {
    typeList = type;
    var url = "";
    if (type == "Questions")
        url = "/api/Questions/" + uid;
    else if (type == "Answers" && qid != undefined)
        url = "/api/Answers/" + qid + "/" + uid;
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
        }).fail(function () { alert("Error: " + url); console.log("Error: " + url);});
}

function objectToUrlParams(obj = {}) {
    var params = "";
    Object.keys(obj).forEach(k => params += "&" + k + "=" + obj[k]);
    return params;
}

function sortList(attr, order = 1) {
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
            fSorter = (a, b) => order * (a - b);
            break;
        case "string":
            fSorter = (a, b) => order * a.localeCompare(b);
            break;
        default:
            console.log("Error: compare type " + t);
            return;
            break;
    }
    list.sort(fSorter);
}
