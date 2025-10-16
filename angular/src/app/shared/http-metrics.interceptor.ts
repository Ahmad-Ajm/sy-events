import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, finalize } from 'rxjs';

// اعتراض بسيط يقيس زمن الطلب ويطبع القيم؛ يمكن لاحقًا تجميع p95/p99
export class HttpMetricsInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const started = performance.now();
    return next.handle(req).pipe(
      finalize(() => {
        const ms = performance.now() - started;
        // TODO: إرسال القياسات إلى خدمة Metrics للتجميع (p95/p99)
        // حالياً: طباعة فقط للتحقق
        console.debug('[HTTP]', req.method, req.url, Math.round(ms) + 'ms');
      })
    );
  }
}


