import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
// This intercepts the httpRequest
// 'next' is the http response which is coming back
// If we catch any errors we will be able to do something with it
export class ErrorInterceptor implements HttpInterceptor {

  // Router provides access to navigation functionality
  constructor(
    private router: Router,
    private toastr: ToastrService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          if (error.status === 400) {
            if (error.errors) {
              throw error.error;
            } else {
              this.toastr.error(error.error.message, error.error.statusCode);
            }
          }
          if (error.status === 401) {
            this.toastr.error(error.error.message, error.error.statusCode);
          }
          if (error.status === 404) {
            this.router.navigateByUrl('/not-found');
          }
          if (error.status === 500) {
            // Creates an object called error and set to it the object error generated as response of this very method (our method).
            const navigationExtras: NavigationExtras = { state: {error: error.error}};
            this.router.navigateByUrl('/server-error', navigationExtras);
          }
        }
        return throwError(error);
      })
    );
  }
}
