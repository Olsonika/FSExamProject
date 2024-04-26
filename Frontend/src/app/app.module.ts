import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import {DashboardPage} from "./dashboard/dashboard.page";
import {LoginRegisterPage} from "./login-register/login-register.page";
import {ReactiveFormsModule} from "@angular/forms";


@NgModule({
  declarations: [AppComponent, DashboardPage, LoginRegisterPage],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, ReactiveFormsModule],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
  bootstrap: [AppComponent],
})
export class AppModule {}
