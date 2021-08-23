import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;
  quantity = 1;

  constructor(
    private shopService: ShopService,
    private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private basketService: BasketService,
  ) {
    // Used to set the breadcrumbs just after we got the result from the API.
    this.bcService.set('@productDetails', ' ');
  }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(): void {
    // 'id' is the name of route params that we set in app-routing
    this.shopService.getProduct(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(response => {
      this.product = response;
      // Here is configurated the BreadCrumbs. @productDetails must be exactly the same as defined in shop-routing.module.ts
      this.bcService.set('@productDetails', this.product.name);
    }, error => {
      console.log(error);
    });
  }

  incrementQuantity(): void {
    this.quantity++;
  }

  decrementQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  addItemToBasket(): void {
    this.basketService.addItemToBasket(this.product, this.quantity);
  }
}
