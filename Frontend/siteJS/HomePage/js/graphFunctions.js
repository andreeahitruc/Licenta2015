var contor = 1;
var sys = arbor.ParticleSystem(4000, 400, 0.9);
sys.screenSize(1000, 600)
sys.screenPadding(80)
var selectedEdge = selectedNode = prevNode = prevColor = null;
var numAdded = 0; var canvas, ctx; var image; var images = []; var finish = false; var friendsLinkObj;
setTimeout(function () {
 
}, 200)

$(document).ready(function (e) {
    
    try {
        var cookie = readCookie('UserProfile');
        if (cookie == null)
            window.location.replace("http://localhost/Licenta/sitejs/");
    } catch (Exception) { }
    $.ajax({
        type: 'GET',
        url: apiBaseURL + '/GetAllFriends/' + readCookie('UserProfile')[0],
        dataType: 'json',
        success: function (data) {
            sys.parameters({ gravity: true });//for gr
            sys.renderer = Renderer("#viewport");//for gr
            sys.addNode('you', { 'color': 'red', 'shape': 'dot', 'label': 'YOU' });
            $.each(data, function (index, value) {
                sys.addNode(data[index]["UserId"], { 'label': data[index]["IconFarm"], 'color': data[index]["PathAlias"], 'shape': data[index]["UserId"] });
                images.push(data[index]["PathAlias"]);
                contor++;
            });
            $.each(data, function (index, value) {
                sys.addEdge('you', data[index]["UserId"], { color: 'blue', 'directed': true });
            });
        }
    })
    $('#links').on('click', function () {
       // var friend = 1;
        // recursive(friend);
        $(this).hide();
        $.ajax({
            type: 'GET',
            url: apiBaseURL + '/DrawLinks/' + readCookie('UserProfile')[0],
            dataType: 'json',
            success: function (data) {
                friendsLinkObj = data;
                $.each(data, function (index, value) {
                    if (Object.keys(data[index]["friends"]).length > 1)
                    {
                        $.each(Object.keys(data[index]["friends"]), function (index2, value) {
                            addEdgeNew(sys, Object.keys(data[index]["friends"])[index2], Object.keys(data[index]["friends"]),data[index]["CategoryName"]);
                        });
                    }
                });
            }
        });
    })
});
function addEdgeNew(sys, u, v, catName) {
    var contor = 0;
    for (var x = 0; x <= v.length-1; x++)
    {
        currentEdge = sys.getEdges(u, v[x]).concat(sys.getEdges(v[x], u));
        if (currentEdge.length == 0) {
            var edge = sys.addEdge(u, v[x], { color: 'red', label: catName });
            //var edge = sys.addEdge(u, v[x], { color: 'red', label: label + ", " + catName });
        }
    }
}
$(document).ready(function () {
    function nodepressed(e) {
        var arrayFriends;
        var pos = $(this).offset();
        var p = { x: e.pageX - pos.left, y: e.pageY - pos.top }
        var x = sys.nearest(p);
        //var nodeAddress = "https://www.google.ro/" + selected.node.name;
        if (x.distance < 20) {
            console.log(x.node.data.label);
            if (x.node.data.label != "YOU") {
                $('#friendDetails').modal('show');
                $('.modalFriendImage').attr('src', x.node.data.color);
                $('.modalFriendName').html(x.node.data.label);
                $.ajax({
                    type: 'GET',
                    url: apiBaseURL + '/GetUserLinks/' + x.node.data.shape + '/' + readCookie('UserProfile')[0],
                    dataType: 'json',
                    success: function (data) {
                        $('.modalCategories').html('');
                        $.each(Object.keys(data), function (index, value) {
                            if (value == "tags") {
                                $('.modalCategories').append('<div class="container">' +
                                    '<div class="leftBar ' + value + '">' + value + '</div><br/>' +
                                    '<div class="rightBar ' + value + '"></div>' +
                                    '</div>');
                                $('.leftBar.' + value).html('INTERESTS: ');
                                var tags = "";
                                for (var i = 0; i <= data[value].length - 1;i++)
                                {
                                    if (i == data[value].length - 1)
                                    { tags = tags + data[value][i] } else
                                    tags = tags + data[value][i]+', '
                                }
                                $('.rightBar.' + value).html(tags);
                            }
                            if (value == "categories")
                            {
                                $('.modalCategories').append('<div class="container">' +
                                   '<div class="nameCategCon"></div><br/>' +
                                   '<div class="allcategories"></div>' +
                                   '</div>');
                                $('.nameCategCon').html("ASSOCIATED CATEGORIES: ");
                                var catgs = "";
                                for (var i = 0; i <= data[value].length - 1; i++) {
                                    if (i == data[value].length - 1)
                                    { catgs = catgs + data[value][i] } else
                                        catgs = catgs + data[value][i] + ', '
                                }
                                $('.allcategories').html(catgs);
                            }
                            if (value == "friends") {
                                $('.modalCategories').append('<div class="container">' +
                                    '<div class="leftBar ' + value + '">' + value + '</div><br/>' +
                                    '<div class="rightBar ' + value + '"></div>' +
                                    '</div>');
                                $('.leftBar.' + value).html('LINKED INTERESTS: ');
                                var fr = "";
                                for (var i = 0; i <= data[value].length - 1; i++) {
                                    if (i == data[value].length - 1)
                                    { fr = fr + data[value][i] } else
                                        fr = fr + data[value][i] + ', '
                                }
                                $('.rightBar.' + value).html(fr);
                            }
                        });
                    }
                });
            }
         
            

        }
        return false;
    }
   
    $("#viewport").dblclick(nodepressed);   
})

