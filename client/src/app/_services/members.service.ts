import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

import { Member } from '../_models/member';
import { MembersCache } from '../_helpers/members-cache';
import { Photo } from '../_models/photo';

@Injectable({
    providedIn: 'root'
})
export class MembersService {

    baseUrl = environment.apiUrl;
    readonly membersCache: MembersCache = new MembersCache();

    constructor(private http: HttpClient) { }

    getMembers(): Observable<Member[]> {
        if (this.membersCache.hasValues) return of(this.membersCache.members);

        return this.http.get<Member[]>(this.baseUrl + 'users').pipe(map((members: Member[]) => {
            this.membersCache.members = members;
            return members;
        }));
    }

    getMember(username: string): Observable<Member> {
        if (this.membersCache.hasValues) 
        {
            const member = this.membersCache.getByUsername(username);
            if (member !== undefined) return of(member);
        }

        return this.http.get<Member>(this.baseUrl + 'users/' + username).pipe(map((member: Member) => {
            return member;
        }));
    }

    updateMember(member: Member): Observable<object> {
        this.membersCache.save(member);
        return this.http.put(this.baseUrl + 'users', member);
    }

    setMainPhotoForMember(photo: Photo) : Observable<object> {
        return this.http.put(`${this.baseUrl}users/set-main-photo/${photo.id}`, {});
    }

    deletePhoto(photo: Photo): Observable<object> {
        return this.http.delete(`${this.baseUrl}users/delete-photo/${photo.id}`);
    }
}
