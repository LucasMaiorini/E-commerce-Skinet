import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { IProductBrand } from '../shared/models/productBrand';
import { IProductType } from '../shared/models/productType';
import { ShopParams } from './models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  // The default behavior since Angular 9 is static:false
  // but as the search field it's part of the template that will always be available, it's set to true.
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  // Array of products that will be displayed in component.
  products: IProduct[];
  // Array of brands that will be displayed in left section.
  brands: IProductBrand[];
  // Array of product types that will be displayed in left section.
  types: IProductType[];
  shopParams = new ShopParams();
  // Total amount of products
  totalCount: number;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
  ];

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProductTypes();
    this.getProductBrands();
    this.getProducts();
  }

  getProducts(): void {
    this.shopService.getProducts(this.shopParams).subscribe(response => {
      this.products = response.data;
      this.shopParams.pageNumber = response.pageIndex;
      this.shopParams.pageSize = response.pageSize;
      this.totalCount = response.count;
      console.log(this.products);
    }, error => {
      console.log(error);
    });
  }
  getProductBrands(): void {
    this.shopService.getProductBrands().subscribe(response => {
      // we set a new item in the response array (id:0 and name: All), then we pass all the content of the array (with spread operator)
      this.brands = [{ id: 0, name: 'All' }, ...response];
    }, error => {
      console.log(error);
    });
  }
  getProductTypes(): void {
    this.shopService.getProductTypes().subscribe(response => {
      // we set a new item in the response array (id:0 and name: All), then we pass all the content of the array (with spread operator)
      this.types = [{ id: 0, name: 'All' }, ...response];
    }, error => {
      console.log(error);
    });
  }

  onBrandSelected(brandId: number): void {
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    // We call the getProducts again because we need it to return only the products following the filter that we just set.
    this.getProducts();
  }

  onTypeSelected(typeId: number): void {
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    // We call the getProducts again because we need it to return only the products following the filter that we just set.
    this.getProducts();
  }

  onSortSelected(sort: string): void {
    this.shopParams.sort = sort;
    // We call the getProducts again because we need it to return only the products following the filter that we just set.
    this.getProducts();
  }

  // the event value is the number of the page.
  onPageChanged(event: number) {
    // The if condition guarantees that the page will not be changed when triggered by any event that we don't control
    if (this.shopParams.pageNumber !== event) {
      this.shopParams.pageNumber = event;
      // We call the getProducts again because we need it to return only the products following the filter that we just set.
      this.getProducts();
    }
  }

  onSearch() {
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }

}
