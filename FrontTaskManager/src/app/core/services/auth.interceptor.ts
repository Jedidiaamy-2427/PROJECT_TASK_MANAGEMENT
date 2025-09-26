import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const token = localStorage.getItem('auth_token');
  if (token) {
    const cloned = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
    return next(cloned).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status === 401 && auth.refreshToken) {
          // tenter un refresh puis rejouer
          return auth.refresh().pipe(
            switchMap(() => {
              const newToken = auth.token;
              const retried = newToken ? req.clone({ setHeaders: { Authorization: `Bearer ${newToken}` } }) : req;
              return next(retried);
            })
          );
        }
        return throwError(() => err);
      })
    );
  }
  return next(req);
};
