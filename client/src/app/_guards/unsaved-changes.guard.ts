import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
    providedIn: 'root'
})
export class UnsavedChangesGuard implements CanDeactivate<unknown> {
    canDeactivate(component: MemberEditComponent): boolean {
        if (component.stateUnsaved) {
            return confirm('Unsaved changes will be lost. Continue?');
        }
    return true;
    }

}
