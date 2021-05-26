import { Member } from "../_models/member";

export class MembersCache {
    private membersArray: Member[] = [];
    private cachedAt: number = Date.now();
    private expirationTermInMilliseconds: number = 5 * 60 * 1000;

    get isValid() : boolean {
        return Date.now() - this.cachedAt < this.expirationTermInMilliseconds;
    }

    get members() : Member[] {
        if (!this.isValid) return [];

        return this.membersArray.slice(0, this.membersArray.length);
    }

    set members(membersInput: Member[]) {
        this.membersArray = membersInput.slice(0, membersInput.length);
        this.cachedAt = Date.now();
    }

    get length() : number {
        if (!this.isValid) return 0;

        return this.membersArray.length;
    }

    get hasValidValues() : boolean {
        if (!this.isValid) return false;

        return this.membersArray.length > 0;
    }

    getByUsername(username: string) : Member {
        if (!this.isValid) return undefined;

        return this.membersArray.find(member => member.username === username);
    }

    save(memberToSave: Member) {
        if (!this.isValid) return;

        let wasUpdated = false;
        this.membersArray.forEach((member, index) => {
            if (member.id === memberToSave.id)
            {
                this.membersArray[index] = memberToSave;
                wasUpdated = true;
            }
        });

        if (wasUpdated) return;

        this.membersArray.push(memberToSave);
    }

    // private validate() {
    //     if (this.isValid) return;
    //     this.membersArray = [];
    // }
}