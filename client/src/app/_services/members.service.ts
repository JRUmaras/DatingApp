import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, min, take } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

import { IMember } from '../_models/member';
import { MembersCache } from '../_helpers/members-cache';
import { PaginatedItems } from '../_helpers/pagination';
import { Photo } from '../_models/photo';
import { UserParams } from '../_models/userParams';
import { PaginatedMembersCache } from '../_helpers/paginated-members-cache';
import { AccountService } from './account.service';

@Injectable({
    providedIn: 'root'
})
export class MembersService {

    baseUrl = environment.apiUrl;
    readonly membersCache: MembersCache = new MembersCache();
    readonly membersCache2: PaginatedMembersCache = new PaginatedMembersCache();

    constructor(private http: HttpClient, private accountService: AccountService) { }

    getMembers(userParams: UserParams): Observable<PaginatedItems<IMember[]>> {
        //if (this.membersCache.hasValidValues) return of(this.membersCache.members);

        // return this.http.get<Member[]>(this.baseUrl + 'users').pipe(map((members: Member[]) => {
        //     this.membersCache.members = members;
        //     return members;
        // }));

        const queryKey = Object.values(userParams).join('-');
        const members = this.membersCache2.getQuery(queryKey);
        console.log(members);
        if (members !== null) return of(members);

        let params = new HttpParams();

        params = this.appendPaginationParams(params, userParams.pageNumber, userParams.pageSize);
        params = this.appendAgePreferenceParams(params, userParams.minAge, userParams.maxAge);
        params = params.append('gender', userParams.gender.toString());
        params = params.append('orderBy', userParams.orderBy);

        return this.getPaginatedResult<IMember[]>(this.baseUrl + 'users', params)
            .pipe(map((paginatedMembers: PaginatedItems<IMember[]>) => {
                this.membersCache2.save(paginatedMembers.items, queryKey, paginatedMembers.pagination);
                return paginatedMembers;
            }));
    }

    getMember(username: string): Observable<IMember> {
        let member = this.membersCache2.getByUsername(username);
        if (member !== null) return of(member);

        // if (this.membersCache.hasValidValues) 
        // {
        //     const member = this.membersCache.getByUsername(username);
        //     if (member !== undefined) return of(member);
        // }

        return this.http.get<IMember>(this.baseUrl + 'users/' + username)
            .pipe(map((member: IMember) => {
                return member;
            }));
    }

    updateMember(member: IMember): Observable<object> {
        // this.membersCache.save(member);
        this.membersCache2.save([member]);

        return this.http.put(this.baseUrl + 'users', member);
    }

    setMainPhotoForMember(photo: Photo) : Observable<object> {
        this.membersCache2.invalidate();
        return this.http.put(`${this.baseUrl}users/set-main-photo/${photo.id}`, {});
    }

    deletePhoto(photo: Photo): Observable<object> {
        //this.accountService.currentUser$.pipe(take(1)).subscribe(user => user.)
        this.membersCache2.invalidate();
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
}
