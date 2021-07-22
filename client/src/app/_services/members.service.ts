import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, min, take } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

import { IMember } from '../_models/member';
import { MembersCache } from '../_helpers/members-cache';
import { IPagination, PaginatedItems } from '../_helpers/pagination';
import { Photo } from '../_models/photo';
import { UserParams } from '../_models/userParams';
import { PaginatedMembersCache } from '../_helpers/paginated-members-cache';
import { AccountService } from './account.service';
import { User } from '../_models/user';

@Injectable({
    providedIn: 'root'
})
export class MembersService {

    baseUrl = environment.apiUrl;

    private user: User;
    private _userParams: UserParams;

    readonly membersCache: MembersCache = new MembersCache();
    readonly paginatedMembersCache: PaginatedMembersCache = new PaginatedMembersCache();

    constructor(private http: HttpClient, private accountService: AccountService) { 
        this.accountService.currentUser$
            .pipe(take(1))
            .subscribe(user => {
                this.user = user;
                this.userParams = new UserParams(user);
            });
    }

    public get userParams() {
        return this._userParams;
    }

    public set userParams(value: UserParams) {
        this._userParams = value;
    }

    resetUserParams() : UserParams {
        this.userParams = new UserParams(this.user);
        return this.userParams;
    }

    getMembers(userParams: UserParams): Observable<PaginatedItems<IMember[]>> {
        //if (this.membersCache.hasValidValues) return of(this.membersCache.members);

        // return this.http.get<Member[]>(this.baseUrl + 'users').pipe(map((members: Member[]) => {
        //     this.membersCache.members = members;
        //     return members;
        // }));

        const queryKey = Object.values(userParams).join('-');
        const members = this.paginatedMembersCache.getQuery(queryKey);
        if (members !== null) return of(members);

        let params = new HttpParams();

        params = this.appendPaginationParams(params, userParams.pageNumber, userParams.pageSize);
        params = this.appendAgePreferenceParams(params, userParams.minAge, userParams.maxAge);
        params = params.append('gender', userParams.gender.toString());
        params = params.append('orderBy', userParams.orderBy);

        return this.getPaginatedResult<IMember[]>(this.baseUrl + 'users', params)
            .pipe(map((paginatedMembers: PaginatedItems<IMember[]>) => {
                this.paginatedMembersCache.save(paginatedMembers.items, queryKey, paginatedMembers.pagination);
                return paginatedMembers;
            }));
    }

    getMember(username: string): Observable<IMember> {
        let member = this.paginatedMembersCache.getByUsername(username);
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
        this.paginatedMembersCache.save([member]);

        return this.http.put(this.baseUrl + 'users', member);
    }

    setMainPhotoForMember(photo: Photo) : Observable<object> {
        this.paginatedMembersCache.invalidate();
        return this.http.put(`${this.baseUrl}users/set-main-photo/${photo.id}`, {});
    }

    deletePhoto(photo: Photo): Observable<object> {
        //this.accountService.currentUser$.pipe(take(1)).subscribe(user => user.)
        this.paginatedMembersCache.invalidate();
        return this.http.delete(`${this.baseUrl}users/delete-photo/${photo.id}`);
    }

    addLike(username: string) {
        return this.http.post(`${this.baseUrl}likes/${username}`, {});
    }

    getLikes(predicate: string) {
        return this.http.post(`${this.baseUrl}likes?predicate=${predicate}`, {});
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
