var contor = 1;
var sys = arbor.ParticleSystem(4000, 400, 0.9);
sys.screenSize(1000, 600)
sys.screenPadding(80)
var selectedEdge = selectedNode = prevNode = prevColor = null;
var numAdded = 0; var canvas, ctx; var image; var images = []; 
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
                sys.addNode('friend' + contor, { 'label': data[index]["IconFarm"], 'color': data[index]["PathAlias"] });
                images.push(data[index]["PathAlias"]);
                contor++;
            });
            for (var i = 1; i < contor; i++) {
                sys.addEdge('you', 'friend' + i, { color: 'blue', 'directed': true });
            }
            $.get("../files/friends.html", function (data) {
                $("#hiddenFriends").html(data);
               // addEdgeNew(sys, 'friend1', 'friend500');
            });
   
        }
    })
    $('#links').on('click', function () {
        var friend = 1;
        recursive(friend);
    })
});


 function recursive(friend)
{
    $('.friend' + friend).each(function (i, obj) {
        try {
            $('.' + $(this).attr('class').split(' ')[1]).each(function (i, obj) {
                var ob1 = $(this);
                if (obj.className.split(' ')[0] != "friend" + friend) {
                    addEdgeNew(sys, obj.className.split(' ')[0], "friend" + friend)
                    $('#show').append("<span>" + "friend" + friend + "->" + obj.className.split(' ')[0] + "</span>");
                }
            });
        } catch (Exception) { }
    });
    if (friend == contor) {
        
        return 0;
    }
    else {
        var x = friend + 1;
        return recursive(x);
    }
}
function addEdgeNew(sys,u, v) {
    currentEdge = sys.getEdges(u, v).concat(sys.getEdges(v, u));
    if (currentEdge.length == 0) {
       // sys.addNode(v, { 'color': 'blue', 'shape': 'dot', 'radius': 30, 'alpha': 1, 'label': "New Node" });
        var edge = sys.addEdge(u, v, { color: 'red' });

    }

}
$(document).ready(function () {
    $('#draw').click(function () {
        drawImagex(sys);
    })
    function nodepressed(e) {
        var pos = $(this).offset();
        var p = { x: e.pageX - pos.left, y: e.pageY - pos.top }
        var x = sys.nearest(p);
        //var nodeAddress = "https://www.google.ro/" + selected.node.name;
        if (x.distance < 20) {
            console.log(x.node.data.label);
        }
        return false;
    }
   
    $("#viewport").dblclick(nodepressed);
           // //$('#viewport').mousemove(handler.moved);
     
})

