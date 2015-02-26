$(function () {

    // Reference the auto-generated proxy for the hub.
    var hub_man_proxy = $.connection.hubMan;

    // Create a function that the hub can call back to show Game ID.
    hub_man_proxy.client.setGameId = function (id) {
        // Add the Game ID to the page.
        $('#gameId').html(id);
        console.log("Started connection with id " + id);
    };

    // Create a function that the hub can call back to add Player.
    hub_man_proxy.client.addPlayer = function (id, name) {
        // Add Player to Game
        World.addPlayer(id, name);
        World.draw();
    };

    // Create a function that the hub can call back to remove Player.
    hub_man_proxy.client.removePlayer = function (id) {
        // Add Player to Game
        World.removePlayer(id);
        World.draw();
    };


    // Create a function that the hub can call back to move player.
    hub_man_proxy.client.movePlayer = function (id, direction) {
        // Invoke movePlayer with User info.
        World.movePlayer( id, direction );
        hub_man_proxy.server.updateRemainingDots( World.getRemaining() );
        hub_man_proxy.server.updateDots( id, World.getPlayer(id).getCocos() );
        World.draw();
    };


    // Start the connection.
    $.connection.hub.start().done(function () {
        hub_man_proxy.server.createGame();
    });
});
