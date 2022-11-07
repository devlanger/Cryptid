import { Component } from '@angular/core';
import { Player } from './models/player';
import { PlayerService } from './services/player.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'CryptidFrontend.UI';
  players: Player[] = [];
  playerToEdit?: Player;

  constructor(private playerService: PlayerService) { }

  ngOnInit() : void {
    this.playerService
    .getPlayers()
    .subscribe((result: Player[]) => {this.players = result;});
  }

  createNewPlayer() {
    this.playerToEdit = new Player();
  }

  updatePlayersList(players: Player[])
  {
    this.players = players;
  }

  editPlayer(player: Player) {
    this.playerToEdit = player;
  }
}
