import { Component, isDevMode, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  // Properties
  title = 'Ski Net';

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    if (isDevMode()) {
      console.log('Development');
    } else {
      console.log('Production');
    }
    const basketId = localStorage.getItem('basket_id');
    if (basketId) {
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log('initialized basket');
      }, error => {
        console.log(error);
      });
    } else {
      console.log('sem basket');
    }
  }

}
