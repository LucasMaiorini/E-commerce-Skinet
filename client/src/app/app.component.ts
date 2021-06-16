import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { IPagination } from './models/pagination';
import { IProduct } from './models/product';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  // Properties

  title = 'Ski Net';
  // Array of products that will be displayed in component.
  products: IProduct[];


  /**
   * Dependency Injection
   */
  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/products').subscribe((response: IPagination) => {
      // response it's a object defined in the Backend called Pagination. Paginations has the data property.
      this.products = response.data;
    }, error => {
      console.log(error);
    });
  }
}
