import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IProductBrand } from '../shared/models/productBrand';
import { IPagination } from '../shared/models/pagination';
import { IProductType } from '../shared/models/productType';
import { map } from 'rxjs/operators';
import { ShopParams } from './models/shopParams';
import { IProduct } from '../shared/models/product';
@Injectable({
  providedIn: 'root'
})
export class ShopService {

  // baseUrl to access our API in Back end.
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getProducts(shopParams: ShopParams): Observable<IPagination> {
    // HttpParams represents the parameters that will be send in the url (query parameters).
    let params = new HttpParams();

    // If brandId = 0 no specific brand was selected
    if (shopParams.brandId !== 0) {
      // The query parameter must be string
      params = params.append('brandId', shopParams.brandId.toString());
    }

    // If typeId = 0 no specific brand was selected
    if (shopParams.typeId !== 0) {
      // The query parameter must be string
      params = params.append('typeId', shopParams.typeId.toString());
    }

    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }
    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());
    return this.http.get<IPagination>(this.baseUrl + 'products', { observe: 'response', params })
      // As we are using the 'observe: response' we are making it an HttpResponse<Observable<IPagination>>
      // instead an Observable<IPagination>,
      // that's why we need to use the map operator. We take the response body (which is an Observable<HttpResponse>) and return it.
      .pipe(
        map(response => {
          return response.body
        })
      );
  }

  getProductTypes(): Observable<IProductType[]> {
    return this.http.get<IProductType[]>(this.baseUrl + 'products/types');
  }

  getProductBrands(): Observable<IProductBrand[]> {
    return this.http.get<IProductBrand[]>(this.baseUrl + 'products/brands');
  }

  getProduct(id: number) {
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }
}
