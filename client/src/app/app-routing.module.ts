import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';

import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthorizationGuard } from './_guards/authorization.guard';
import { UnsavedChangesGuard } from './_guards/unsaved-changes.guard';

const routes: Routes = [
 { path: '', component: HomeComponent },
 { 
   path: '', 
   runGuardsAndResolvers: 'always', 
   canActivate: [AuthorizationGuard],
   children: [
     { path: 'members', component: MemberListComponent },
     { path: 'members/id/:id', component: MemberDetailComponent },
     { path: 'members/:username', component: MemberDetailComponent },
     { path: 'member/edit', component: MemberEditComponent, canDeactivate: [UnsavedChangesGuard]},
     { path: 'lists', component: ListsComponent },
     { path: 'messages', component: MessagesComponent }]
 },
 { path: 'errors', component: TestErrorsComponent},
 { path: 'not-found', component: NotFoundComponent},
 { path: 'server-error', component: ServerErrorComponent },
 { path: '**', component: HomeComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
