//CODE FROM THE ARBOR.JS SITE...DOWN
var Renderer = function (canvas) {
    var canvasNode = canvas;
    var canvas = $(canvas).get(0)
    var ctx = canvas.getContext("2d");
    var particleSystem,contorImages = 0;
    var that = {
        init: function (system) {
            particleSystem = system
            particleSystem.screenSize(canvas.width, canvas.height)
            particleSystem.screenPadding(80) // leave an extra 80px of whitespace per side

            // set up some event handlers to allow for node-dragging
            that.initMouseHandling()
        },

        redraw: function () {
            // fire event from canvas
           // $(canvasNode).trigger('redraw', [particleSystem]);

            ctx.fillStyle = "white"
            ctx.fillRect(0, 0, canvas.width, canvas.height)

            particleSystem.eachEdge(function (edge, pt1, pt2) {
                // edge: {source:Node, target:Node, length:#, data:{}}
                // pt1:  {x:#, y:#}  source position in screen coords
                // pt2:  {x:#, y:#}  target position in screen coords

                // determine edge width based on number of emails
                var edges = 0;
            
                label = edge.data.label
                //if (edges > 1) console.log(edges);
                // draw a line from pt1 to pt2
                if (edge.source.name == "you") {
                    ctx.strokeStyle = "rgba(15, 0, 103, 20);"
                    ctx.lineWidth = (1)
                    ctx.beginPath()
                    ctx.moveTo(pt1.x, pt1.y)
                    ctx.lineTo(pt2.x, pt2.y)
                    ctx.stroke()
                } else {
                    ctx.strokeStyle = "rgba(215, 40, 40, 30);"
                    ctx.lineWidth = (1)
                    ctx.beginPath()
                    ctx.moveTo(pt1.x, pt1.y)
                    ctx.lineTo(pt2.x, pt2.y)
                    if (finish) {
                        ctx.fillStyle = 'blue';
                        ctx.font = 'italic 12px sans-serif';
                        ctx.textBaseline = 'top';
                        measure = ctx.measureText(label);
                        ctx.fillText(label, pt1.x - measure.width / 2, pt1.y + 40);
                    }
                    ctx.stroke()
                }
            })

            particleSystem.eachNode(function (node, pt) {
                // node: {mass:#, p:{x,y}, name:"", data:{}}
                // pt:   {x:#, y:#}  node position in screen coords
                var w = 40, label = node.data.label, measure, image = new Image();
                if (node.name != "you") {
                    if (node.data.size == 1) {
                        image.src = node.data.color;
                        ctx.drawImage(image, pt.x - w / 2, pt.y - w / 2, 20, 20);
                        ctx.fillStyle = 'black';
                        ctx.font = 'italic 14px sans-serif';
                        ctx.textBaseline = 'top';
                        measure = ctx.measureText(label);
                        ctx.fillText(label, pt.x - measure.width / 2, pt.y + 20);
                    } else {
                        image.src = node.data.color;
                        ctx.drawImage(image, pt.x - w / 2, pt.y - w / 2, 40, 40);
                        ctx.fillStyle = 'black';
                        ctx.font = 'italic 14px sans-serif';
                        ctx.textBaseline = 'top';
                        measure = ctx.measureText(label);
                        ctx.fillText(label, pt.x - measure.width / 2, pt.y + 20);
                    }
                } else {
                    image.src = "images/me.png";
                    ctx.drawImage(image, pt.x - w / 2, pt.y - w / 2);
                    ctx.fillStyle = 'red';
                    ctx.font = 'italic 16px sans-serif';
                    ctx.textBaseline = 'top';
                    measure = ctx.measureText(label);
                    ctx.fillText(label, pt.x - measure.width / 2, pt.y + 20);
                }
            })
        },

        initMouseHandling: function () {
            // no-nonsense drag and drop (thanks springy.js)
            var dragged = null;
            var nodeData, edgesFrom;

            // set up a handler object that will initially listen for mousedowns then
            // for moves and mouseups while dragging
            var handler = {
                clicked: function (e) {
                    var pos = $(canvas).offset();
                    _mouseP = arbor.Point(e.pageX - pos.left, e.pageY - pos.top)
                    dragged = particleSystem.nearest(_mouseP);

                    if (dragged && dragged.node !== null) {
                        // while we're dragging, don't let physics move the node
                        dragged.node.fixed = true
                        nodeData = dragged.node.data;

                        if (nodeData.type == 'recipient') {
                            edgesFrom = particleSystem.getEdgesFrom(dragged.node);

                            if (edgesFrom.length < 1) {
                                var branch = dragged.node.data.r2e(dragged.node.data, dragged.node.data.period);
                                particleSystem.graft(branch);
                            } else {
                                $.each(edgesFrom, function (index, edge) {
                                    particleSystem.pruneNode(edge.target);
                                });
                            }
                        }
                    }

                    $(canvas).bind('mousemove', handler.dragged)
                    $(window).bind('mouseup', handler.dropped)

                    return false
                },
                dragged: function (e) {
                    var pos = $(canvas).offset();
                    var s = arbor.Point(e.pageX - pos.left, e.pageY - pos.top)

                    if (dragged && dragged.node !== null) {
                        var p = particleSystem.fromScreen(s)
                        dragged.node.p = p
                    }

                    return false
                },

                dropped: function (e) {
                    if (dragged === null || dragged.node === undefined) return
                    if (dragged.node !== null) dragged.node.fixed = false
                    dragged.node.tempMass = 1000
                    dragged = null
                    $(canvas).unbind('mousemove', handler.dragged)
                    $(window).unbind('mouseup', handler.dropped)
                    _mouseP = null
                    return false
                }
            }

            // start listening
            $(canvas).mousedown(handler.clicked);
        },

    }
    return that
}