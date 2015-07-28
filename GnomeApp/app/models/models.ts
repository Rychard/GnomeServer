import models = GnomeServer.Models;

export class GnomeSummary implements models.IGnomeSummary {
    Population: number;
    Gnomes: GnomeServer.Models.IGnome[];

    constructor(obj: any) {
        this.Population = obj.Population;
        this.Gnomes = obj.Gnomes.map((item: Gnome) => { return new Gnome(item); });
    }
}

export class Gnome implements models.IGnome {
    ID: number;
    Name: string;
    Title: string;
    Stats: GnomeServer.Models.IGnomeStats;
    Location: GnomeServer.Models.IGnomeLocation;
    BodyParts: GnomeServer.Models.IGnomeBodyPartStatus[];
    Skills: GnomeServer.Models.IGnomeSkill[];

    constructor(obj: any) {
        this.ID = obj.ID;
        this.Name = obj.Name;
        this.Title = obj.Title;
        this.Stats = new GnomeStats(obj.Stats);
        this.Location = new GnomeLocation(obj.Location);
        this.BodyParts = obj.BodyParts.map((item: GnomeBodyPartStatus) => { return new GnomeBodyPartStatus(item); });
        this.Skills = GnomeSkill.SortBySkill(obj.Skills.map((item: GnomeSkill) => { return new GnomeSkill(item); }), true);
    }
}

export class GnomeStats implements models.IGnomeStats {
    Happiness: number;
    BloodLevel: number;
    Rest: number;
    Hunger: number;
    Thirst: number;

    constructor(obj: any) {
        this.Happiness = obj.Happiness;
        this.BloodLevel = obj.BloodLevel;
        this.Rest = obj.Rest;
        this.Hunger = obj.Hunger;
        this.Thirst = obj.Thirst;
    }
}

export class GnomeLocation implements models.IGnomeLocation {
    X: number;
    Y: number;
    Z: number;
    Text: string;

    constructor(obj: any) {
        this.X = obj.X;
        this.Y = obj.Y;
        this.Z = obj.Z;
        this.Text = "(Layer " + this.Y + " @ " + this.X + ", " + this.Z + ")";
    }
}

export class GnomeBodyPartStatus implements models.IGnomeBodyPartStatus {
    BodyPart: string;
    Statuses: string[];
    StatusText: string;

    constructor(obj: any) {
        this.BodyPart = obj.BodyPart;
        this.Statuses = obj.Statuses;
        this.StatusText = this.Statuses.join(', ');
    }
}

export class GnomeSkill implements models.IGnomeSkill {
    Name: string;
    Skill: number;

    constructor(obj: any) {
        this.Name = obj.Name;
        this.Skill = obj.Skill;
    }

    public static SortBySkill(arr: GnomeSkill[], descending: boolean) : GnomeSkill[] {
        var sorted = arr.sort(this.CompareByskill);
        if (descending) { sorted.reverse(); }
        return sorted;
    }

    private static CompareByskill(a: GnomeSkill, b: GnomeSkill) {
        if (a.Skill < b.Skill) { return -1; }
        if (a.Skill > b.Skill) { return 1; }
        return 0;
    }
}