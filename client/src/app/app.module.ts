import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';

import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    // Some Ngx-Bootstrap elements need BrowserAnimationModule to work properly.
    BrowserAnimationsModule,
    // HttpClientModule - Necessary to make usage of API.
    HttpClientModule,
    ButtonsModule.forRoot()
  ],
  providers: [],
  // Apply bootstrap in our AppComponent
  bootstrap: [AppComponent]
})
export class AppModule { }
