import { Component, OnInit } from '@angular/core';
import { constStrings } from '../_helpers/constants';
import { IMember } from '../_models/member';
import { MembersService } from '../_services/members.service';

@Component({
    selector: 'app-lists',
    templateUrl: './lists.component.html',
    styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

    members: Partial<IMember[]>;
    predicate = constStrings.likePredicates.liked;

    readonly predicateLiked = constStrings.likePredicates.liked;
    readonly predicateLikedBy = constStrings.likePredicates.likedBy;

    get title() {
        return this.predicate === this.predicateLiked
            ? 'Members you\'ve liked'
            : 'Members that have liked you';
    }

    constructor(private membersService: MembersService) { }

    ngOnInit(): void {
        this.loadLikes();
    }

    loadLikes() {
        this.membersService
            .getLikes(this.predicate)
            .subscribe((members) => {
                this.members = members;
                console.log(this.members);
            });
    }
}
