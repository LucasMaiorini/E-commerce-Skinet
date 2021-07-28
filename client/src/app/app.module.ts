import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { HomeModule } from './home/home.module';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    CoreModule,
    HomeModule,
    SharedModule,
    AppRoutingModule,
    // Some Ngx-Bootstrap elements need BrowserAnimationModule to work properly.
    BrowserAnimationsModule,
    // HttpClientModule - Necessary to make usage of API.
    HttpClientModule,
    // Necessary to use buttons from Ngx-Bootstrap.
    ButtonsModule.forRoot(),
    NgxSpinnerModule,
  ],
  providers: [
    // HTTP_INTERCEPTORS for use interceptors in the project. Interceptors belongs to core module.
    // Interceptors allows to take http errors and redirect the user to some error page, for example.
    // It is also able to generate an encrypted token as the user log in the system.
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},

  ],
  // Apply bootstrap in our AppComponent
  bootstrap: [AppComponent]
})
export class AppModule { }
