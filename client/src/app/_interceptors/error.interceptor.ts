import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { NavigationExtras, Router } from '@angular/router';

import { Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastrService: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(catchError(httpResponseError => {
      if (httpResponseError) {
        switch (httpResponseError.status) {
          case 400:
            const actualErrors = httpResponseError.error.errors;
            if (actualErrors) {
              // Flatten the errors from the JSON response into a string
              const modalStateErrors = [];
              for (const key in actualErrors) {
                if (actualErrors[key]) {
                  modalStateErrors.push(actualErrors[key]);
                }
              }
              throw modalStateErrors.flat();
            } else {
              this.toastrService.error(httpResponseError.statusText, httpResponseError.status);
            }
            break;
          
          case 401:
            this.toastrService.error(httpResponseError.statusText, httpResponseError.status);
            break;

          case 404:
            this.router.navigateByUrl('/not-found');
            break;

          case 500:
            const navigationExtras: NavigationExtras = {
              state: {
                error: httpResponseError.error
              }
            }
            this.router.navigateByUrl('/server-error', navigationExtras);
            break;
          
          default:
            this.toastrService.error('Something unexpected went wrong... :(');
            console.log(httpResponseError);
            break;
        }
      }

      return throwError(httpResponseError);
    }));
  }
}
