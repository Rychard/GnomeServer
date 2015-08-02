import app = require('durandal/app');
import ko = require('knockout');

class Speed {
    // TODO: Determine the actual endpoint from the API if it differs from the default?
    endpointRoot = "http://localhost:8081/";
    updateToken: any;

    speedCurrent: KnockoutObservable<number>;
    speedIsPaused: KnockoutObservable<Boolean>;

    speedDecrease = () => {
        var current = this.speedCurrent();
        if (this.speedCurrent() > 1) {
            var newValue = current - 1;
            this.speedCurrent(newValue);
            this.setSpeed();    
        }
    }

    speedPause = () => {
        this.pause();
    }

    speedPlay = () => {
        this.play();
    }

    speedIncrease = () => {
        var current = this.speedCurrent();
        
        // Set an arbitrary limit of 20x.
        // From what I can tell, the simulation starts skipping stuff after 5x.
        if (this.speedCurrent() < 20) {
            var newValue = current + 1;
            this.speedCurrent(newValue);
            this.setSpeed();    
        }
    }

    speedCanPause: KnockoutComputed<Boolean> = ko.pureComputed({
        owner: this,
        read: () => {
            return !this.speedIsPaused();
        }
    });

    speedCanPlay: KnockoutComputed<Boolean> = ko.pureComputed({
        owner: this,
        read: () => {
            return this.speedIsPaused();
        }
    });

    speedText: KnockoutComputed<String> = ko.pureComputed({
        owner: this,
        read: () => {
            if (this.speedIsPaused()) {
                return "(Paused) " + this.speedCurrent().toString();
            } else {
                return this.speedCurrent().toString();
            }
        },
        deferEvaluation: true
    });

    getSpeed = () => {
        var endpoint = Utils.getRootUrl() + "Game/Speed";
        $.get(endpoint, (data) => {
            this.speedCurrent(parseInt(data.Speed));
            this.speedIsPaused(data.IsPaused.toLowerCase() === "true");
        }).fail((data) => {
            console.log("getSpeed - error");
            console.log(data);
        });
    }

    setSpeed = () => {
        var endpoint = this.endpointRoot + "Game/Speed?speed=" + this.speedCurrent().toString();
        $.post(endpoint).fail((data) => {
            console.log("setSpeed - error");
            console.log(data);
        });
    }

    pause = () => {
        var endpoint = this.endpointRoot + "Game/Pause";
        $.post(endpoint, () => {
            this.speedIsPaused(true);
        }).fail((data) => {
            console.log("pause - error");
            console.log(data);
        });
    }

    play = () => {
        var endpoint = this.endpointRoot + "Game/Play";
        $.post(endpoint, () => {
            this.speedIsPaused(false);
        }).fail((data) => {
            console.log("play - error");
            console.log(data);
        });
    }

    activate = () => {
        this.speedIsPaused = ko.observable<Boolean>(true);
        this.speedCurrent = ko.observable<number>(0);

        // Poll the server for this data every x seconds.
        this.update();

        this.speedCurrent.subscribe((newValue) => {
            app.trigger('gameSpeedChanged', newValue);
        });

        this.speedIsPaused.subscribe((newValue) => {
            if (newValue) {
                app.trigger('gamePaused');
            } else {
                app.trigger('gameUnpaused');
            }
        });
    };

    update = () => {
        setTimeout(() => {
            this.getSpeed();
            this.update();
        }, 2000);
    }
}

export = Speed;