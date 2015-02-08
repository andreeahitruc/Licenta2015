var contor = 1; contorFriends = 0;
var sys = arbor.ParticleSystem(4000, 400, 0.9);
sys.screenSize(1000, 600)
sys.screenPadding(80)
var selectedEdge = selectedNode = prevNode = prevColor = null;
var numAdded = 0; var canvas, ctx; var image; var images = []; var finish = false; var friendsLinkObj; var arrayLinks = []; var goodBundles = []; var strongBundles = [];
setTimeout(function () {
 
}, 200)

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
                sys.addNode(data[index]["UserId"], { 'label': data[index]["IconFarm"], 'color': data[index]["PathAlias"], 'shape': data[index]["UserId"],size:0 });
                images.push(data[index]["PathAlias"]);
                contor++; contorFriends++;
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
        $('.comm-btn').css('display','inline-block');
        $('.links').css('display','none');
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
                setTimeout(function () {
                    var contor = 0; var categoriesS = 0; var categoriesC = 0;
                    sys.eachNode(function (node, pt) {
                       contor++;
                        if (node.data.categories != null)
                        {
                            if ($.inArray(',', node.data.categories) > -1) {
                                categoriesC++;
                            } else { categoriesS++; }
                        }
                    });
                    //var procentRed = contor * (categoriesS / 100);
                    //var procentRedApplied = Math.round(procentRed) * 8;
                    //var procentOrange = contor * (categoriesC / 100);
                    //var procentOrApplied = Math.round(procentOrange) * 8;
                    // $('#frBun').show();
                    $('#frBun').css('display','inline-block');
                    $('#frBun').html('<p>Friendship bundles:' + ' ' + contorFriends + ' links</p>');
                    // $('#strongBun').show();
                    $('#strongBun').css('display','inline-block');
                    $('#strongBun').html('<p>Strong bundles:' + ' ' + categoriesC + ' links</p>');
                    // $('#goodBun').show();
                    $('#goodBun').css('display','inline-block');
                    $('#goodBun').html('<p>Good bundles:' + ' ' + categoriesS + ' links</p>');

                   
                }, 1000);
            }
        });
        $.ajax({
            type: 'GET',
            url: apiBaseURL + '/DrawLinksForUser/' + readCookie('UserProfile')[0],
            dataType: 'json',
            success: function (data) {
                var nodeNew = sys.addNode(readCookie('UserProfile')[0], { 'label': "Your Links", 'color': 'red', 'shape': 'dot' });
                var YouNode = sys.getNode('you');
                sys.addEdge(nodeNew,YouNode,{ color: 'yellow', label: ""});
                $.each(data, function (index, value) {
                    if (Object.keys(data[index]["friends"]).length >= 1) {
                        addEdgeNew(sys,readCookie('UserProfile')[0], Object.keys(data[index]["friends"]), data[index]["CategoryName"]);
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
            var test = u + ',' + v[x]; var testInv = v[x] + ',' + u;
            if ((($.inArray(test, arrayLinks) > -1) || ($.inArray(testInv, arrayLinks) > -1)) && v[x] != u) {
                var node = sys.getNode(test);
                if (node != null) {
                    var itemIns = u + '+' + v[x];
                    //prepare array for comments section
                    if ($.inArray(itemIns, strongBundles) <= -1)
                        strongBundles.push(itemIns);
                    localStorage['array'] = strongBundles;
                    //var indexRM = goodBundles.indexOf(test);
                    //if (indexRM  > -1) {
                    //    goodBundles = goodBundles.splice(indexRM, 1);
                    //}
                        //end
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
            } else
            {
                if (v[x] != u) {
                    arrayLinks.push(test);
                    if (u != readCookie('UserProfile')[0])
                        var intNode = sys.addNode(test, { 'label': "", 'color': 'images/category.png', 'size': 1, 'friends': test, 'categories': catName, 'linkNode': 'false' });//create an intermediate node for category
                    else
                        var intNode = sys.addNode(test, { 'label': "", 'color': 'images/category.png', 'size': 1, 'friends': test, 'categories': catName,'linkNode':'true' });//create an intermediate node for category
                    //sys.addEdge(u, v[x], { color: 'red', label: catName });
                    sys.addEdge(intNode,u, { color: 'red', label: catName });
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
            console.log(x.node.data.label);// get friend details
            if (x.node.data.linkNode == "true") {
                $('.modalCategories').html('');
                var ob = {
                    catName: x.node.data.categories,
                    friends: x.node.data.friends
                }
                $.ajax({
                    type: 'POST',
                    url: apiBaseURL + '/GetCategoryUserDetails/',
                    dataType: 'json',
                    data: ob,
                    success: function (data) {
                        $('.modalFriendImage').attr('src', 'images/category.png');
                        $('#friendDetails').modal('show');
                          var html = '<div style="height:20px;"></div><table id="categoryTable"><tr><td id="friend0Name" align="center"></td><td></td><td id="friend1Name" align="center"></td></tr>' +
                                                   '<tr><td id="friend0Img" align="center"></td><td align="center"><img src="https://cdn4.iconfinder.com/data/icons/ionicons/512/icon-link-128.png" style="height:60px"; width:70px;></img></td><td id="friend1Img" align="center"></td></tr>' +
                                                   '<tr><td id="friend0Tags" align="center"></td><td align="center"></td><td id="friend1Tags" align="center"></td></tr>';
                                $('.modalCategories').append(html);
                                //  $('.modalCategories').append('<img src="../img/loading.gif" class="loadingImageModal" /> Retreving the common images...');
                                //  $('.modalCategories').append('<div id="commonImages"></div>');
                                $.each(data, function (index, value) {
                                    $('#friend' + index + 'Name').html('<p>' + value.friendName + '</p>');
                                    $('#friend' + index + 'Img').html('<img src="' + value.friendPhoto + '" style="height:40px;width:40px;border-radius:50%" />');
                                    var tags = '';
                                    $.each(value.tagsName, function (index, value) {
                                        tags = tags + value + ' ';
                                    });
                                    $('#friend' + index + 'Tags').html('<p>' + tags + '</p>');
                                });
                                var categories = x.node.data.categories;
                                var categoriesArray =[];
                                var newCategory = "";
                                categoriesArray = categories.split(',');
                                for(var i=0;i< categoriesArray.length;i++)
                                {
                                    if(i != categoriesArray.length -1)
                                    newCategory = newCategory + categoriesArray[i]+', ';
                                    else
                                        newCategory = newCategory + categoriesArray[i];
                                }
                                $('.modalCategories').append('<div class="categoriesSection"><p>Interest Categories:</p></div>');
                                $('.modalCategories').append('<div class="categories"><p>' + newCategory + '</p></div>');
                    }
                });
            } else {
                if (x.node.data.label != "YOU" && x.node.data.label != "Your Links") {
                    $('#friendDetails').modal('show');
                    $('.modalFriendImage').attr('src', x.node.data.color);
                    $('.modalFriendName').html(x.node.data.label);
                    if (x.node.data.size == 0) {
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
                                        for (var i = 0; i <= data[value].length - 1; i++) {
                                            if (i == data[value].length - 1)
                                            { tags = tags + data[value][i] } else
                                                tags = tags + data[value][i] + ', '
                                        }
                                        $('.rightBar.' + value).html(tags);
                                    }
                                    if (value == "categories") {
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
                    } else {
                        $('.modalCategories').html('');
                        var ob = {
                            catName: x.node.data.categories,
                            friends: x.node.data.friends
                        }
                        $.ajax({
                            type: 'POST',
                            url: apiBaseURL + '/GetCategoryDetails/',
                            dataType: 'json',
                            data: ob,
                            success: function (data) {
                                //$('.modalCategories').append('<table>');
                                var html = '<div style="height:20px;"></div><table id="categoryTable"><tr><td id="friend0Name" align="center"></td><td></td><td id="friend1Name" align="center"></td></tr>' +
                                                   '<tr><td id="friend0Img" align="center"></td><td align="center"><img src="https://cdn4.iconfinder.com/data/icons/ionicons/512/icon-link-128.png" style="height:60px"; width:70px;></img></td><td id="friend1Img" align="center"></td></tr>' +
                                                   '<tr><td id="friend0Tags" align="center"></td><td align="center"></td><td id="friend1Tags" align="center"></td></tr>';
                                $('.modalCategories').append(html);
                                //  $('.modalCategories').append('<img src="../img/loading.gif" class="loadingImageModal" /> Retreving the common images...');
                                //  $('.modalCategories').append('<div id="commonImages"></div>');
                                $.each(data, function (index, value) {
                                    $('#friend' + index + 'Name').html('<p>' + value.friendName + '</p>');
                                    $('#friend' + index + 'Img').html('<img src="' + value.friendPhoto + '" style="height:40px;width:40px;border-radius:50%" />');
                                    var tags = '';
                                    $.each(value.tagsName, function (index, value) {
                                        tags = tags + value + ' ';
                                    });
                                    $('#friend' + index + 'Tags').html('<p>' + tags + '</p>');
                                });
                                var categories = x.node.data.categories;
                                var categoriesArray =[];
                                var newCategory = "";
                                categoriesArray = categories.split(',');
                                for(var i=0;i< categoriesArray.length;i++)
                                {
                                    if(i != categoriesArray.length -1)
                                    newCategory = newCategory + categoriesArray[i]+', ';
                                    else
                                        newCategory = newCategory + categoriesArray[i];
                                }
                                $('.modalCategories').append('<div class="categoriesSection"><p>Interest Categories:</p></div>');
                                $('.modalCategories').append('<div class="categories"><p>' + newCategory + '</p></div>');

                                //$.ajax({//get common pictures
                                //    type: 'POST',
                                //    url: apiBaseURL + '/CommonPictures/',
                                //    dataType: 'json',
                                //    data: ob,
                                //    success: function (data) {
                                //        $.each(data, function (index, value) {
                                //            $.each(value.photos, function (indexNew, valueNew) {
                                //                $('#commonImages').append('<img src="' + valueNew[0].Medium640Url + '" style="width:100px;height:100px;"></img>');
                                //            })
                                //        })
                                //        $('.loadingImageModal').hide();
                                //    }
                                //});
                            }
                        });
                    }
                }
            }
        }
        return false;
    }
   
    $("#viewport").dblclick(nodepressed);
   
})