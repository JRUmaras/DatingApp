import { constStrings } from "../_helpers/constants";
import { IUserParams } from "../_interfaces/IUserParams";
import { IUser } from "./user";

export class UserParams implements IUserParams {
    gender: string;
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'lastActive';
    likesPredicate = constStrings.likePredicates.liked;

    constructor(user: IUser) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
    }
}