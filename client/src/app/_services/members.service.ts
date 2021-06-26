import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, min } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

import { Member } from '../_models/member';
import { MembersCache } from '../_helpers/members-cache';
import { PaginatedItems } from '../_helpers/pagination';
import { Photo } from '../_models/photo';
import { UserParams } from '../_models/userParams';

@Injectable({
    providedIn: 'root'
})
export class MembersService {

    baseUrl = environment.apiUrl;
    readonly membersCache: MembersCache = new MembersCache();

    constructor(private http: HttpClient) { }

    getMembers(userParams: UserParams): Observable<PaginatedItems<Member[]>> {
        //if (this.membersCache.hasValidValues) return of(this.membersCache.members);

        // return this.http.get<Member[]>(this.baseUrl + 'users').pipe(map((members: Member[]) => {
        //     this.membersCache.members = members;
        //     return members;
        // }));

        let params = new HttpParams();

        params = this.appendPaginationParams(params, userParams.pageNumber, userParams.pageSize);
        params = this.appendAgePreferenceParams(params, userParams.minAge, userParams.maxAge);
        params = params.append('gender', userParams.gender.toString());
        params = params.append('orderBy', userParams.orderBy);

        return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params);
    }

    private getPaginatedResult<T>(url: string, params: HttpParams): Observable<PaginatedItems<T>> {
        const paginatedMembers = new PaginatedItems<T>();

        return this.http.get<T>(url, { observe: 'response', params }).pipe(
            map(response => {
                paginatedMembers.items = response.body;

                const pagination = response.headers.get('Pagination');
                if (pagination !== null) {
                    paginatedMembers.pagination = JSON.parse(pagination);
                }

                return paginatedMembers;
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

    private appendPaginationParams(params: HttpParams, pageNumber: number, pageSize: number): HttpParams {
        params = params.append('pageNumber', pageNumber.toString());
        params = params.append('pageSize', pageSize.toString());

        return params;
    }

    private appendAgePreferenceParams(params: HttpParams, minAge: number, maxAge: number) {
        params = params.append('minAge', minAge.toString());
        params = params.append('maxAge', maxAge.toString());

        return params;
    }
}
