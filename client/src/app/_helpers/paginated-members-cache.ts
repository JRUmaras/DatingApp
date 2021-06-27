import { IMember } from "../_models/member";
import { IPagination, PaginatedItems } from "./pagination";

export class PaginatedMembersCache {
    private expirationTermInMilliseconds: number = 15 * 1000; // 5 * 60 * 1000;
    private cacheById = new Map<number, CachedMember>();
    private cacheByUsername = new Map<string, CachedMember>();
    private queryCache = new Map<string, CachedQuery>();
    private isValid = false;

    private isExpired(cachedAt: number) : boolean {
        return Date.now() - cachedAt > this.expirationTermInMilliseconds;
    }

    // Avoid using this, this is a hack because I was lazy to implement smarter invalidation for certain conditions.
    invalidate() {
        this.isValid = false;
    }

    save(members: IMember[], queryKey: string = null, pagination: IPagination = null) {
        members.forEach(member => {
            let cachedMember = new CachedMember();
            cachedMember.cachedAt = Date.now();
            cachedMember.member = member;

            this.cacheById.set(member.id, cachedMember);
            this.cacheByUsername.set(member.username, cachedMember);
        });

        this.isValid = true;

        if (queryKey === null) return;

        let cachedQuery = new CachedQuery();
        cachedQuery.cachedAt = Date.now();
        cachedQuery.memberIds = members.map(member => member.id);
        cachedQuery.pagination = pagination;

        this.queryCache.set(queryKey, cachedQuery);
    }

    getByUsername(username: string) : IMember {
        const cachedMember = this.cacheByUsername.get(username);
        if (cachedMember === undefined || this.isValid === false || this.isExpired(cachedMember.cachedAt)) return null;

        return cachedMember.member;
    }

    getQuery(queryKey: string) : PaginatedItems<IMember[]> {
        const cachedQuery = this.queryCache.get(queryKey);
        // Validity of a cached query is primarily defined by the caching times of members.
        // If no members belong to the query cache - then don't repeat calls to API until
        // cachedQuery itself expires.
        if (cachedQuery === undefined || this.isValid === false || (cachedQuery.memberIds.length === 0 && this.isExpired(cachedQuery.cachedAt))) return null;

        let members: IMember[] = [];
        for (let id of cachedQuery.memberIds) {
            let cachedMember = this.cacheById.get(id);
            if (this.isExpired(cachedMember.cachedAt)) return null;
            members.push(cachedMember.member);
        };

        let paginatedMembers = new PaginatedItems<IMember[]>();
        paginatedMembers.items = members;
        paginatedMembers.pagination = cachedQuery.pagination;

        return paginatedMembers;
    }
}

class CachedMember {
    cachedAt: number;
    member: IMember;
}

class CachedQuery {
    cachedAt: number;
    memberIds: number[];
    pagination: IPagination;
}