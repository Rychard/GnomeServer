define(["require", "exports", 'durandal/app', 'knockout'], function (require, exports, app, ko) {
    var Speed = (function () {
        function Speed() {
            var _this = this;
            this.endpointRoot = "http://localhost:8081/";
            this.speedDecrease = function () {
                var current = _this.speedCurrent();
                if (_this.speedCurrent() > 1) {
                    var newValue = current - 1;
                    _this.speedCurrent(newValue);
                    _this.setSpeed();
                }
            };
            this.speedPause = function () {
                _this.pause();
            };
            this.speedPlay = function () {
                _this.play();
            };
            this.speedIncrease = function () {
                var current = _this.speedCurrent();
                // Set an arbitrary limit of 20x.
                // From what I can tell, the simulation starts skipping stuff after 5x.
                if (_this.speedCurrent() < 20) {
                    var newValue = current + 1;
                    _this.speedCurrent(newValue);
                    _this.setSpeed();
                }
            };
            this.speedCanPause = ko.pureComputed({
                owner: this,
                read: function () {
                    return !_this.speedIsPaused();
                }
            });
            this.speedCanPlay = ko.pureComputed({
                owner: this,
                read: function () {
                    return _this.speedIsPaused();
                }
            });
            this.speedText = ko.pureComputed({
                owner: this,
                read: function () {
                    if (_this.speedIsPaused()) {
                        return "(Paused) " + _this.speedCurrent().toString();
                    }
                    else {
                        return _this.speedCurrent().toString();
                    }
                },
                deferEvaluation: true
            });
            this.getSpeed = function () {
                var endpoint = _this.endpointRoot + "Game/Speed";
                $.get(endpoint, function (data) {
                    _this.speedCurrent(parseInt(data.Speed));
                    _this.speedIsPaused(data.IsPaused.toLowerCase() === "true");
                }).fail(function (data) {
                    console.log("getSpeed - error");
                    console.log(data);
                });
            };
            this.setSpeed = function () {
                var endpoint = _this.endpointRoot + "Game/Speed?speed=" + _this.speedCurrent().toString();
                $.post(endpoint).fail(function (data) {
                    console.log("setSpeed - error");
                    console.log(data);
                });
            };
            this.pause = function () {
                var endpoint = _this.endpointRoot + "Game/Pause";
                $.post(endpoint, function () {
                    _this.speedIsPaused(true);
                }).fail(function (data) {
                    console.log("pause - error");
                    console.log(data);
                });
            };
            this.play = function () {
                var endpoint = _this.endpointRoot + "Game/Play";
                $.post(endpoint, function () {
                    _this.speedIsPaused(false);
                }).fail(function (data) {
                    console.log("play - error");
                    console.log(data);
                });
            };
            this.activate = function () {
                _this.speedIsPaused = ko.observable(true);
                _this.speedCurrent = ko.observable(0);
                // Poll the server for this data every x seconds.
                _this.update();
                _this.speedCurrent.subscribe(function (newValue) {
                    app.trigger('gameSpeedChanged', newValue);
                });
                _this.speedIsPaused.subscribe(function (newValue) {
                    if (newValue) {
                        app.trigger('gamePaused');
                    }
                    else {
                        app.trigger('gameUnpaused');
                    }
                });
            };
            this.update = function () {
                setTimeout(function () {
                    _this.getSpeed();
                    _this.update();
                }, 2000);
            };
        }
        return Speed;
    })();
    return Speed;
});
//# sourceMappingURL=viewmodel.js.map