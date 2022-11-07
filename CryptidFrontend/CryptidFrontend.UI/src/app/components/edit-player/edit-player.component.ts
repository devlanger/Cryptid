import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Player } from 'src/app/models/player';
import { PlayerService } from 'src/app/services/player.service';

@Component({
  selector: 'app-edit-player',
  templateUrl: './edit-player.component.html',
  styleUrls: ['./edit-player.component.css']
})
export class EditPlayerComponent implements OnInit {

  @Input() player?: Player;
  @Output() playersUpdated = new EventEmitter<Player[]>()

  constructor(private playerService : PlayerService) { }

  ngOnInit(): void {
  }

  updatePlayer(player: Player)
  {
    this.playerService
    .updatePlayer(player)
    .subscribe((players: Player[]) => { this.playersUpdated.emit(players); });
  }

  createPlayer(player: Player)
  {
    //this.playerService
    //.updatePlayer(player)
    //.subscribe((players: Player[]) => { this.playersUpdated.emit(players); });
  }
}
