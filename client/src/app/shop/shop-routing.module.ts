import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ShopComponent } from './shop.component';
import { ProductDetailsComponent } from './product-details/product-details.component';

// Create routes separated by modules is useful when we use Lazy Loading
const routes: Routes = [
  // '' is the root component for shop.module
  { path: '', component: ShopComponent },
  // To define how the breadCrumbs will looks like it's defined an alias that is configured in product-details.component.ts
  { path: ':id', component: ProductDetailsComponent, data: { breadcrumb: { alias: 'productDetails' } } },
]


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    // forChild makes possible to use Lazy Loading
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule,
  ]
})
export class ShopRoutingModule { }
