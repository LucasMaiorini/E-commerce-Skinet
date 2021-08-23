import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket } from '../shared/models/basket';
import { IBasketTotals } from '../shared/models/basketTotals';
import { IBasketItem } from '../shared/models/item';
import { IJson } from '../shared/models/Json';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})

// The things is slightly different here, because we are accessing the redis database and getting info from the basket.
export class BasketService {
  baseUrl = environment.apiUrl;
  // BehaviorSubject always emits its first value, which, in this case, is null.
  private basketSource = new BehaviorSubject<IBasket>(null);
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  // We create a variable to make the basketSource accessible from the outside.
  // To make it clear that is an observable we put a $ on it.
  basket$ = this.basketSource.asObservable();
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }


  getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id)
      .pipe(
        // (basket: IBasket) is what we'll get as response from the 'get'.
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          console.log(this.getCurrentBasketValue());
          this.calculateTotals();
        })
      );
  }

  setBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket)
      .subscribe((response: IJson) => {
        this.basketSource.next(response.result);
        this.calculateTotals();
      }, error => {
        console.log(error);
      });
  }

  // This method is used when we want to get the basketSource value without subscribing
  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1): void {
    const itemToAdd: IBasketItem = this.mapProductItemtoBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  // Defines if the itemToAdd must update or be pushed to BasketItem array.
  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    // If index is equal -1 means that there's no such Id on intems array of IBasketItem.
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    // Stores the id in client's side
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemtoBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      quantity,
      pictureUrl: item.pictureUrl,
      type: item.productType,
      brand: item.productBrand,
    };
  }

  removeItemFromBasket(item: IBasketItem): void {
    const basket = this.getCurrentBasketValue();
    if (basket.items.some(x => x.id === item.id)) {
      basket.items = basket.items.filter(i => i.id !== item.id);
      if (basket.items.length > 0) {
        this.setBasket(basket);
      } else {
        this.deleteBasket(basket);
      }
    }
  }

  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });
  }

  incrementItemQuantity(item: IBasketItem): void {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem): void {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if (basket.items[foundItemIndex].quantity > 1) {
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }
  }
  private calculateTotals(): void {
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    // reduce applies the callback function in each element of the array.
    // result (which initial value is setted as 0 ) is the value that aggregate the sums.
    // bi (basketItem) represents the BasketItem.
    // Each basket.items will have its price multiplied by its quantity and the sum will me added to the result.
    // e.g.: (result,bi) => (10 * 2) + 0; After that, the result value will be 20.
    // e.g.: (result,bi) => (5 * 1) + 20; After that, the result value will be 25
    const subtotal = basket.items.reduce((result, bi) => (bi.price * bi.quantity) + result, 0);

    const total = subtotal + shipping;
    this.basketTotalSource.next({ shipping, total, subtotal });
  }
}
