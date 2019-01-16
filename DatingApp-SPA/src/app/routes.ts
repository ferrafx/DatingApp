import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListComponent } from './list/list.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDatailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';

export const approutes: Routes = [
    { path: '', component:HomeComponent},
    {
        path: '', 
        runGuardsAndResolvers : 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component:MemberListComponent,
                    resolve: { users: MemberListResolver}},
             {path:'members/:id',component:MemberDetailComponent, 
                    resolve: {user: MemberDatailResolver}}, //you choose the name of property user 
            {path:'member/edit',component:MemberEditComponent, 
                    resolve: {user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges] }, //you choose the name of property user 
            { path: 'messages', component:MessagesComponent},
            { path: 'lists', component:ListComponent},
        ]
    },

    { path: '**', redirectTo: '', pathMatch: 'full'}
];