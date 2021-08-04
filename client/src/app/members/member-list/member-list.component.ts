import { Component, OnInit } from '@angular/core';
import { IPagination } from 'src/app/_helpers/pagination';

import { IMember } from 'src/app/_models/member';
import { UserParams } from 'src/app/_models/userParams';
import { MembersService } from 'src/app/_services/members.service';

@Component({
    selector: 'app-member-list',
    templateUrl: './member-list.component.html',
    styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

    //members$: Observable<Member[]>;

    members: IMember[];
    pagination: IPagination;
    userParams: UserParams;

    get numberOfMatches() {
        return this.pagination?.totalCount ?? 0;
    }

    constructor(private membersService: MembersService) { 
         this.userParams = membersService.userParams;
         this.pagination = {
            pageNumber: this.userParams.pageNumber,
            pageSize: this.userParams.pageSize,
            totalCount: 0,
            totalPages: 1
        };
    }

    ngOnInit(): void {
        //this.members$ = this.membersService.getMembers();
        this.loadMembers();
    }

    loadMembers() {
        this.membersService.userParams = this.userParams;
        this.membersService.getMembers(this.userParams)
            .subscribe(paginatedMembers => {
                this.members = paginatedMembers.items;
                this.pagination = paginatedMembers.pagination;
            });
    }

    resetFilters() {
        this.userParams = this.membersService.resetUserParams();
        this.loadMembers();
    }

    pageChanged(event: any) {
        this.userParams.pageNumber = event.page;
        this.membersService.userParams = this.userParams;
        this.loadMembers();
    }
}
