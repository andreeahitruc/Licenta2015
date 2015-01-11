var contor = 1;
var sys = arbor.ParticleSystem(4000, 400, 0.9);
sys.screenSize(1000, 600)
sys.screenPadding(80)
var selectedEdge = selectedNode = prevNode = prevColor = null;
var numAdded = 0; var canvas, ctx; var image; var images = []; var finish = false; var friendsLinkObj; var arrayLinks = []; var array = localStorage['array'];
$(document).ready(function (e) {

    try {
        var cookie = readCookie('UserProfile');
        if (cookie == null)
            window.location.replace("http://localhost:99/Licenta/sitejs/");
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
                sys.addNode(data[index]["UserId"], { 'label': data[index]["IconFarm"], 'color': data[index]["PathAlias"], 'shape': data[index]["UserId"], size: 0 });
                images.push(data[index]["PathAlias"]);
                contor++;
            });
            $.each(data, function (index, value) {
                sys.addEdge('you', data[index]["UserId"], { color: 'blue', 'directed': true });
            });
            $('#goodBun').hide();
        }
    })
    LinksToShow = { links: array };
    $.ajax({
        type: 'POST',
        url: apiBaseURL + '/GetPageLinks',
        dataType: 'json',
        data:LinksToShow,
        success: function (data) {
            $.each(data, function (index, value) {
                $('.links').append('<ul class="ul'+index+'"></ul>')
                $.each(value, function (index2, value2) {
                    $('.ul' + index).append('<li style="display:inline-block"><img src="' + value2.image + '" style="border-radius:50%;width:50px;height:50px;"/><br/>' + value2.name + '</li>');
                    if (index2 == 0) $('.ul' + index).append('<li style="display:inline-block">---strong---</li>');
                });
            });
        }
        });
    $.ajax({
        type: 'GET',
        url: apiBaseURL + '/DrawLinksComments/' + readCookie('UserProfile')[0],
        dataType: 'json',
        success: function (data) {
            friendsLinkObj = data;
            $.each(data, function (index, value) {
                if (Object.keys(value.friends).length > 1) {
                    $.each(Object.keys(value.friends), function (index2, valueN) {
                        addEdgeNew(sys,valueN, Object.keys(value.friends), data[index]["PhotoId"]);
                    });
                }
            });
        }
    });
});
function addEdgeNew(sys, u, v, catName) {
    var contor = 0;
    for (var x = 0; x <= v.length - 1; x++) {
        currentEdge = sys.getEdges(u, v[x]).concat(sys.getEdges(v[x], u));
        if (currentEdge.length == 0) {
            var test = u + ',' + v[x]; var testInv = v[x] + ',' + u;
            if ((($.inArray(test, arrayLinks) > -1) || ($.inArray(testInv, arrayLinks) > -1)) && v[x] != u) {
                var node = sys.getNode(test);
                if (node != null) {
                    var LinkNode1 = sys.getNode(test.split(',')[0]);
                    var LinkNode2 = sys.getNode(test.split(',')[1]);
                    var edge1 = sys.getEdges(node, LinkNode1);
                    var edge2 = sys.getEdges(node, LinkNode2);
                    if (edge1.length != 0)
                        edge1[0].data.color = 'green';
                    if (edge2.length != 0)
                        edge2[0].data.color = 'green';
                    node.data.categories = node.data.categories + ',' + catName;
                }
                // intNode= sys.addNode(test, { 'label': catName, 'color': 'images/category.png', 'size': 1, 'friends': test });//create an intermediate node for category
            } else {
                if (v[x] != u) {
                    arrayLinks.push(test);
                    var intNode = sys.addNode(test, { 'label': "", 'color': 'images/message.png', 'size': 1, 'friends': test, 'categories': catName, 'linkNode': 'true' });//create an intermediate node for category
                    //sys.addEdge(u, v[x], { color: 'red', label: catName });
                    sys.addEdge(intNode, u, { color: 'red', label: catName });
                    sys.addEdge(intNode, v[x], { color: 'red', label: catName });
                    //var edge = sys.addEdge(u, v[x], { color: 'red', label: label + ", " + catName });
                }
            }
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
            if (x.node.data.linkNode == "true") {
                console.log(x.node.data.label);// get friend details
                $('.modalCategories').html('');
                var ob = {
                    catName: x.node.data.categories,
                    friends: x.node.data.friends
                }
                $.ajax({
                    type: 'POST',
                    url: apiBaseURL + '/GetCommentDetails/',
                    content: 'json',
                    data: ob,
                    success: function (data) {
                        $('.modalFriendImage').attr('src', 'images/message.png');
                        $('#friendDetails').modal('show');
                        var html = '<div style="height:20px;"></div><table id="categoryTable"><tr><td id="friend0Name" align="center"></td><td></td><td id="friend1Name" align="center"></td></tr>' +
                                                 '<tr><td id="friend0Img" align="center"></td><td align="center"><img src="https://cdn4.iconfinder.com/data/icons/ionicons/512/icon-link-128.png" style="height:60px"; width:70px;></img></td><td id="friend1Img" align="center"></td></tr>' +
                                                 '<tr><td id="friend0Tags" align="center"></td><td align="center"></td><td id="friend1Tags" align="center"></td></tr>';
                        $('.modalCategories').append(html);
                        $.each(data.friends, function (index, value) {
                            $('#friend' + index + 'Name').html('<p>' + value.name + '</p>');
                            $('#friend' + index + 'Img').html('<img src="' + value.image + '" style="height:40px;width:40px;border-radius:50%" />');
                        });
                        $('.modalCategories').append('<div class="categoriesSection"><p>Commented on the same photo:</p></div>');
                        $('.modalCategories').append('<div class="categories"><a href="' + data.pathUrl + '" target="_blank"><img src="' + data.photoUrl + '" style="border-radius: 50%;"/></div>');
                    },
                    error: function (xhr, status, error) {
                        alert(status);
                    }
                });
            }

        }
        return false;
    }

    $("#viewport").dblclick(nodepressed);

})

