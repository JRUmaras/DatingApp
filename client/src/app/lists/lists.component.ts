import { Component, OnInit } from '@angular/core';
import { constStrings } from '../_helpers/constants';
import { IPagination } from '../_helpers/pagination';
import { IUserParams } from '../_interfaces/IUserParams';
import { IMember } from '../_models/member';
import { MembersService } from '../_services/members.service';

@Component({
    selector: 'app-lists',
    templateUrl: './lists.component.html',
    styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

    members: Partial<IMember[]>;
    userParams: IUserParams;
    pagination: IPagination;

    readonly predicateLiked = constStrings.likePredicates.liked;
    readonly predicateLikedBy = constStrings.likePredicates.likedBy;

    get title() {
        return this.userParams.likesPredicate === this.predicateLiked
            ? 'Members you\'ve liked'
            : 'Members that have liked you';
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
        this.loadLikes();
    }

    loadLikes() {
        this.membersService
            .getLikes(this.userParams)
            .subscribe((paginatedMembers) => {
                this.members = paginatedMembers.items;
                this.pagination = paginatedMembers.pagination;
            });
    }

    pageChanged(event: any) {
        this.userParams.pageNumber = event.page;
        this.membersService.userParams = this.userParams;
        this.loadLikes();
    }
}
