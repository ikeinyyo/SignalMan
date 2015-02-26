var sprites = ["man1", "man2", "man3", "man4", "man5", "man6", "man7", "man8", "man9", "man10", "man11", "man12", "man13", "man14", "man15", "man16"];

var Player = (function () {
    
    var Player = function (id, name) {
        this.id = id;
        this.name = name;
        this.position = {
            x: 1,
            y: 1,
        };
        this.cocos = 0;


        this.newPosition = function (direction) {
            var next_pos = { x: this.position.x, y: this.position.y };

            switch (direction) {
                case "up":
                    next_pos.y -= 1;
                    break;
                case "down":
                    next_pos.y += 1;
                    break;
                case "right":
                    next_pos.x += 1;
                    break;
                case "left":
                    next_pos.x -= 1;
                    break;
            }
            return next_pos;
        };

        this.getCoco = function () {
            return ++this.cocos;
        };

        this.getX = function () {
            return this.position.x;
        };

        this.getY = function () {
            return this.position.y;
        };

        this.createSprite = function () {
            if (sprites && sprites.length) {
                var img = new Image();
                var index = parseInt(Math.random() * sprites.length);
                var sprite = sprites[index];
                sprites.splice(index, 1);

                img.src = '/tiles/' + sprite + '.png';
                return img;
            }
        }

        this.sprite = this.createSprite();

        this.toDOM = function () {
            if (this.sprite) {
                return "<li><img width='20' src='" + this.sprite.src + "'/> " + this.name + ": " + this.cocos + "</li>";
            }
            return "";
        }

    };
    return Player;
})();


var World = {
    remaining: 0,
    players: [],
    addPlayer: function (player) {
        this.players[player.id] = player;

        var rdm_pos;
        do {
            rdm_pos = { x: parseInt(Math.random() * 12), y: parseInt(Math.random() * 12) };
        } while (this.read(rdm_pos) == "#");

        player.position = rdm_pos;
        player.cocos = 0;
        this.movePlayer(player.id, "");
    },
    map: [],
    sprites: {},
    canvas: document.getElementById("canvas").getContext("2d"),
    addSprite: function (name) {
        var img = new Image();
        img.src = '/tiles/' + name + '.png';
        this.sprites[name] = img;
    },
    read: function (position) {
        return this.map[position.y][position.x];
    },
    getCoco: function () {
        this.current_player.getCoco();
        return --this.remaining;
    },
    movePlayer: function (id_player, direction) {
        this.current_player = this.players[id_player];
        var position = this.current_player.newPosition(direction);

        switch (this.read(position)) {
            case "#":
                return false;
            case ".":
                this.getCoco();
            case " ":
                this.move(position);
                return true;
            default:
                return true;
        }
        this.current_player = null;
    },
    move: function (position) {
        this.map[this.current_player.position.y][this.current_player.position.x] = " ";
        //this.map[position.y][position.x] = this.current_player.id;
        this.current_player.position = position;

    },
    draw: function () {
        var TAM_TILE = 32;
        var canvas = document.getElementById("canvas");
        var context = canvas.getContext("2d");

        var posX = posY = 0;

        var wall_tile = new Image();
        wall_tile.src = '/tiles/wall.png';

        for (var i = 0; i < this.map.length; i++) {
            for (var j = 0; j < this.map[i].length; j++) {
                var current;
                switch (this.map[i][j]) {
                    case "#":
                        current = this.sprites.wall;
                        break;
                    case ".":
                        current = this.sprites.dot;
                        break;
                    case " ":
                        current = this.sprites.empty;
                        break;
                    default:
                        current = this.players[this.map[i][j]].sprite;
                        break;
                }
                context.drawImage(current, posX, posY, TAM_TILE, TAM_TILE);
                posX += TAM_TILE;
            };
            posY += TAM_TILE;
            posX = 0;
        };

        var list = $("#list");
        list.html("");

        var keys = Object.keys(this.players)
        var that = this;
        var points = keys.map(function (key) { return { key: key, cocos: that.players[key].cocos }; });
        points.sort(function (a, b) { return b.cocos - a.cocos });

        for (var p in points) {
            var player = this.players[points[p].key];
            console.log(player);
            context.drawImage(player.sprite, player.getX() * TAM_TILE, player.getY() * TAM_TILE, TAM_TILE, TAM_TILE);
            list.append(player.toDOM());
        };

        $("#dots").html(this.remaining);

    },
    init: function () {
        this.addSprite("dot");
        this.addSprite("empty");
        this.addSprite("wall");
        sprites = ["man1", "man2", "man3", "man4", "man5", "man6", "man7", "man8", "man9", "man10", "man11", "man12", "man13", "man14", "man15", "man16"];

        this.map = [["#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#"],
            ["#", ".", ".", ".", ".", ".", ".", ".", ".", "#", ".", "#", ".", ".", ".", ".", ".", ".", "#", ".", ".", "#", ".", ".", ".", ".", ".", ".", "#"],
            ["#", ".", "#", "#", "#", "#", "#", ".", ".", ".", ".", "#", ".", ".", ".", ".", ".", ".", "#", ".", ".", "#", ".", "#", "#", "#", "#", "#", "#"],
            ["#", ".", "#", ".", ".", ".", ".", ".", ".", "#", ".", "#", ".", ".", ".", ".", ".", ".", "#", ".", ".", "#", ".", "#", ".", ".", ".", ".", "#"],
            ["#", ".", "#", ".", ".", ".", ".", ".", ".", "#", ".", "#", "#", "#", "#", ".", "#", "#", "#", ".", ".", "#", ".", "#", ".", "#", ".", ".", "#"],
            ["#", ".", "#", "#", "#", "#", ".", "#", "#", "#", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#", ".", ".", "#"],
            ["#", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#", ".", ".", "#"],
            ["#", ".", ".", ".", ".", ".", ".", "#", "#", "#", ".", "#", "#", "#", "#", ".", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#", ".", ".", "#"],
            ["#", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#", ".", ".", "#"],
            ["#", ".", "#", ".", ".", ".", ".", "#", "#", "#", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#", "#", "#", "#", "#", ".", ".", "#"],
            ["#", ".", "#", ".", ".", ".", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#", ".", ".", "#"],
            ["#", ".", "#", ".", ".", ".", ".", "#", "#", "#", ".", "#", "#", "#", "#", "#", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#", ".", ".", "#"],
            ["#", ".", "#", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#", ".", ".", "#"],
            ["#", ".", "#", ".", ".", ".", ".", "#", "#", "#", ".", ".", ".", ".", ".", "#", ".", ".", ".", ".", ".", "#", ".", ".", ".", "#", ".", ".", "#"],
            ["#", ".", "#", ".", ".", ".", ".", "#", "#", "#", ".", ".", ".", ".", ".", ".", ".", "#", "#", "#", "#", "#", "#", "#", ".", "#", ".", ".", "#"],
            ["#", ".", "#", "#", "#", "#", "#", "#", ".", "#", ".", "#", "#", "#", "#", "#", ".", ".", ".", ".", ".", ".", ".", "#", ".", "#", ".", ".", "#"],
            ["#", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#"],
            ["#", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "#"],
            ["#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#"]],
        this.remaining = 339

        //this.addPlayer(new Player("a", "alex"));
        //this.addPlayer(new Player("g", "gallardo"));

        for (var key in this.players) {
            this.addPlayer(this.players[key]);
        };

        setTimeout(function () { this.draw() }.bind(this), 500);

    }
};

$(document).keypress(function (e) {
    switch (e.which) {
        case 106:
            World.movePlayer("a", "left");
            break;
        case 97:
            World.movePlayer("g", "left");
            break;
        case 105:
            World.movePlayer("a", "up");
            break;
        case 119:
            World.movePlayer("g", "up");
            break;
        case 108:
            World.movePlayer("a", "right");
            break;
        case 100:
            World.movePlayer("g", "right");
            break;
        case 107:
            World.movePlayer("a", "down");
            break;
        case 115:
            World.movePlayer("g", "down");
            break;
    }
    World.draw();
});

$("#reset").click(function () { World.init(); });
World.init();
