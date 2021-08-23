import { v4 as uuidv4 } from 'uuid';
import { IBasketItem } from './item';

export interface IBasket {
  id: string;
  items: IBasketItem[];
  result: any[];
}

// Whenever a Basket instance is created, there will be an empty array of Items and a unique identifier.
export class Basket implements IBasket{
  id = uuidv4();
  items: IBasketItem[] = [];
  result: any[];
}
