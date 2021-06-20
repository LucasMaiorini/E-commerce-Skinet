// We create a class instead a interface because we will initialize a value
export class ShopParams {
  // The id of choosen type by user. It's 0 by default.
  typeId = 0;
  // The id of choosen type by user. It's 0 by default.
  brandId = 0;
  sort = 'name';
  pageNumber = 1;
  pageSize = 6;
  search: string;
}
