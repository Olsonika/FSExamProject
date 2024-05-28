import { Injectable } from '@angular/core';
import { WebSocketClientService } from './ws.client.service';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  public isAuthenticated$: Observable<boolean>;

  constructor(private webSocketClientService: WebSocketClientService) {
    this.isAuthenticated$ = this.webSocketClientService.authenticationStatus$.asObservable();
  }

  authenticateWithJwt(jwt: string) {
    this.webSocketClientService.authenticateWithJwt(jwt);
  }

  logout() {
    this.webSocketClientService.logout();
  }
}
