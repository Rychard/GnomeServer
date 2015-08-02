import models = GnomeServer.Models;
export declare class GnomeSummary implements models.IGnomeSummary {
    Population: KnockoutObservable<number>;
    Gnomes: KnockoutObservableArray<GnomeServer.Models.IGnome>;
    constructor(obj: any);
}
export declare class Gnome implements models.IGnome {
    ID: KnockoutObservable<number>;
    Name: KnockoutObservable<string>;
    Stats: KnockoutObservable<GnomeServer.Models.IGnomeStats>;
    Location: KnockoutObservable<GnomeServer.Models.ILocation>;
    BodyParts: KnockoutObservableArray<GnomeServer.Models.IGnomeBodyPartStatus>;
    Skills: KnockoutObservableArray<GnomeServer.Models.IGnomeSkill>;
    Profession: KnockoutObservable<GnomeServer.Models.IGnomeProfession>;
    ProfessionSkills: KnockoutComputed<models.IGnomeSkill[]>;
    NonProfessionSkills: KnockoutComputed<models.IGnomeSkill[]>;
    constructor(obj: any);
}
export declare class GnomeStats implements models.IGnomeStats {
    Happiness: KnockoutObservable<number>;
    BloodLevel: KnockoutObservable<number>;
    Rest: KnockoutObservable<number>;
    Hunger: KnockoutObservable<number>;
    Thirst: KnockoutObservable<number>;
    constructor(obj: any);
}
export declare class Location implements models.ILocation {
    X: KnockoutObservable<number>;
    Y: KnockoutObservable<number>;
    Z: KnockoutObservable<number>;
    Text: KnockoutComputed<string>;
    constructor(obj: any);
}
export declare class GnomeBodyPartStatus implements models.IGnomeBodyPartStatus {
    BodyPart: KnockoutObservable<string>;
    Statuses: KnockoutObservableArray<string>;
    StatusText: KnockoutComputed<string>;
    constructor(obj: any);
}
export declare class GnomeSkill implements models.IGnomeSkill {
    Name: KnockoutObservable<string>;
    Skill: KnockoutObservable<number>;
    constructor(obj: any);
    static SortBySkill(arr: models.IGnomeSkill[], descending: boolean): GnomeSkill[];
    private static CompareByskill(a, b);
}
