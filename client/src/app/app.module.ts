import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { HttpClientModule } from '@angular/common/http';
import { CoreModule } from './core/core.module';
import { ShopModule } from './shop/shop.module';
import { SharedModule } from './shared/shared.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    // Import to use the components in Core.
    CoreModule,
    // Import to use the components in Shop.
    ShopModule,
    // Import to use the components in shared
    SharedModule,
    // Import to use Routing.
    AppRoutingModule,
    // Some Ngx-Bootstrap elements need BrowserAnimationModule to work properly.
    BrowserAnimationsModule,
    // HttpClientModule - Necessary to make usage of API.
    HttpClientModule,
    // Necessary to use buttons from Ngx-Bootstrap.
    ButtonsModule.forRoot()
  ],
  providers: [],
  // Apply bootstrap in our AppComponent
  bootstrap: [AppComponent]
})
export class AppModule { }
