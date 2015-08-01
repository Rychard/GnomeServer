import models = GnomeServer.Models;
export declare class GnomeSummary implements models.IGnomeSummary {
    Population: KnockoutObservable<Number>;
    Gnomes: KnockoutObservableArray<GnomeServer.Models.IGnome>;
    constructor(obj: any);
}
export declare class Gnome implements models.IGnome {
    ID: KnockoutObservable<Number>;
    Name: KnockoutObservable<String>;
    Stats: KnockoutObservable<GnomeServer.Models.IGnomeStats>;
    Location: KnockoutObservable<GnomeServer.Models.IGnomeLocation>;
    BodyParts: KnockoutObservableArray<GnomeServer.Models.IGnomeBodyPartStatus>;
    Skills: KnockoutObservableArray<GnomeServer.Models.IGnomeSkill>;
    Profession: KnockoutObservable<GnomeServer.Models.IGnomeProfession>;
    ProfessionSkills: KnockoutComputed<models.IGnomeSkill[]>;
    NonProfessionSkills: KnockoutComputed<models.IGnomeSkill[]>;
    constructor(obj: any);
}
export declare class GnomeStats implements models.IGnomeStats {
    Happiness: KnockoutObservable<Number>;
    BloodLevel: KnockoutObservable<Number>;
    Rest: KnockoutObservable<Number>;
    Hunger: KnockoutObservable<Number>;
    Thirst: KnockoutObservable<Number>;
    constructor(obj: any);
}
export declare class GnomeLocation implements models.IGnomeLocation {
    X: KnockoutObservable<Number>;
    Y: KnockoutObservable<Number>;
    Z: KnockoutObservable<Number>;
    Text: KnockoutComputed<String>;
    constructor(obj: any);
}
export declare class GnomeBodyPartStatus implements models.IGnomeBodyPartStatus {
    BodyPart: KnockoutObservable<String>;
    Statuses: KnockoutObservableArray<String>;
    StatusText: KnockoutComputed<String>;
    constructor(obj: any);
}
export declare class GnomeSkill implements models.IGnomeSkill {
    Name: KnockoutObservable<String>;
    Skill: KnockoutObservable<Number>;
    constructor(obj: any);
    static SortBySkill(arr: models.IGnomeSkill[], descending: boolean): GnomeSkill[];
    private static CompareByskill(a, b);
}
