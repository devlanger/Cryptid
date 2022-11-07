import { Guid } from 'guid-typescript';

export class Player
{
    id?: Guid = Guid.create();
    nickname = "";
    role = "";
}