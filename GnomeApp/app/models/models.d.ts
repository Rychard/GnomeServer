import models = GnomeServer.Models;
export declare class GnomeSummary implements models.IGnomeSummary {
    Population: number;
    Gnomes: GnomeServer.Models.IGnome[];
    constructor(obj: any);
}
export declare class Gnome implements models.IGnome {
    ID: number;
    Name: string;
    Title: string;
    Stats: GnomeServer.Models.IGnomeStats;
    Location: GnomeServer.Models.IGnomeLocation;
    BodyParts: GnomeServer.Models.IGnomeBodyPartStatus[];
    Skills: GnomeServer.Models.IGnomeSkill[];
    constructor(obj: any);
}
export declare class GnomeStats implements models.IGnomeStats {
    Happiness: number;
    BloodLevel: number;
    Rest: number;
    Hunger: number;
    Thirst: number;
    constructor(obj: any);
}
export declare class GnomeLocation implements models.IGnomeLocation {
    X: number;
    Y: number;
    Z: number;
    Text: string;
    constructor(obj: any);
}
export declare class GnomeBodyPartStatus implements models.IGnomeBodyPartStatus {
    BodyPart: string;
    Statuses: string[];
    StatusText: string;
    constructor(obj: any);
}
export declare class GnomeSkill implements models.IGnomeSkill {
    Name: string;
    Skill: number;
    constructor(obj: any);
    static SortBySkill(arr: GnomeSkill[], descending: boolean): GnomeSkill[];
    private static CompareByskill(a, b);
}
