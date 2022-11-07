import { Injectable } from '@angular/core';
import { Player } from '../models/player';
import { Guid } from 'guid-typescript';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {
  private controllerUrl = "Players";
  constructor(private http: HttpClient) { }

  public getPlayers() : Observable<Player[]> {
    return this.http.get<Player[]>(`${environment.apiUrl}/${this.controllerUrl}`);
  }

  public updatePlayer(player: Player) : Observable<Player[]> {
    return this.http.put<Player[]>(`${environment.apiUrl}/${this.controllerUrl}`, player);
  }
}
