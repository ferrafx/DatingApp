import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule} from '@Angular/forms'
import { AppRoutingModule } from './app-routing.module';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { BsDropdownModule } from 'ngx-bootstrap';
import { MemberListComponent } from './member-list/member-list.component';
import { ListComponent } from './list/list.component';
import { MessagesComponent } from './messages/messages.component';
import { approutes } from './routes';
import { AuthGuard } from './_guards/auth.guard';


@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListComponent,
      MessagesComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      AppRoutingModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      RouterModule.forRoot(approutes)
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      AuthGuard
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
