declare class Speed {
    endpointRoot: string;
    updateToken: any;
    speedCurrent: KnockoutObservable<number>;
    speedIsPaused: KnockoutObservable<Boolean>;
    speedDecrease: () => void;
    speedPause: () => void;
    speedPlay: () => void;
    speedIncrease: () => void;
    speedCanPause: KnockoutComputed<Boolean>;
    speedCanPlay: KnockoutComputed<Boolean>;
    speedText: KnockoutComputed<String>;
    getSpeed: () => void;
    setSpeed: () => void;
    pause: () => void;
    play: () => void;
    activate: () => void;
    update: () => void;
}
export = Speed;
