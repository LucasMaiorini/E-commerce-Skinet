import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket } from '../shared/models/basket';
import { IBasketItem } from '../shared/models/item';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  // $ represents an Observable
  basket$: Observable<IBasket>;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  removeBasketItem(item: IBasketItem): void {
    this.basketService.removeItemFromBasket(item);
  }

  incrementItemQuantity(item: IBasketItem): void {
    this.basketService.incrementItemQuantity(item);
  }
  decrementItemQuantity(item: IBasketItem): void {
    this.basketService.decrementItemQuantity(item);
  }
}
