import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

import { Member } from '../_models/member';
import { MembersCache } from '../_helpers/members-cache';
import { PaginatedItems } from '../_helpers/pagination';
import { Photo } from '../_models/photo';

@Injectable({
    providedIn: 'root'
})
export class MembersService {

    baseUrl = environment.apiUrl;
    readonly membersCache: MembersCache = new MembersCache();
    paginatedMembers: PaginatedItems<Member[]> = new PaginatedItems<Member[]>();

    constructor(private http: HttpClient) { }

    getMembers(pageNumber?: number, pageSize?: number): Observable<PaginatedItems<Member[]>> {
        //if (this.membersCache.hasValidValues) return of(this.membersCache.members);

        // return this.http.get<Member[]>(this.baseUrl + 'users').pipe(map((members: Member[]) => {
        //     this.membersCache.members = members;
        //     return members;
        // }));

        let params = new HttpParams();

        if (pageNumber !== null && pageSize !== null) {
            params = params.append('pageNumber', pageNumber.toString());
            params = params.append('pageSize', pageSize.toString());
        }

        return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
            map(response => {
                this.paginatedMembers.items = response.body;

                const pagination = response.headers.get('Pagination');
                if (pagination !== null) {
                    this.paginatedMembers.pagination = JSON.parse(pagination);
                }

                return this.paginatedMembers;
            }));
    }

    getMember(username: string): Observable<Member> {
        if (this.membersCache.hasValidValues) 
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
