using Cryptid.Shared.Logic.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cryptid.Shared
{
    public class CommandReader
    {
        public static byte[] WriteCommandToBytes(CommandBase commandBase)
        {
            var stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);
            writer.Write((byte)commandBase.id);
            writer.Write(commandBase.gameId);

            switch (commandBase)
            {
                case MoveAction.Command move:
                    writer.Write(move.unitId);
                    writer.Write(move.posX);
                    writer.Write(move.posZ);
                    break;
                case NextTurnAction.Command nextTurn:

                    break;
                case AttackAction.Command attack:
                    writer.Write(attack.UnitId);
                    writer.Write(attack.TargetId);
                    break;
            }

            return stream.ToArray();
        }

        public static CommandBase ReadCommandFromBytes(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
            CommandType commandId = (CommandType)reader.ReadByte();
            string gameId = reader.ReadString();

            CommandBase command = null;
            switch (commandId)
            {
                case CommandType.MOVE:
                    command = new MoveAction.Command
                    {
                        unitId = reader.ReadString(),
                        posX = reader.ReadInt32(),
                        posZ = reader.ReadInt32()
                    };
                    break;
                case CommandType.NEXT_TURN:
                    command = new NextTurnAction.Command { };
                    break;
                case CommandType.ATTACK_TARGET:
                    command = new AttackAction.Command
                    {
                        UnitId = reader.ReadString(),
                        TargetId = reader.ReadString(),
                    };
                    break;
            }

            if(command != null)
            {
                command.id = commandId;
                command.gameId = gameId;
            }

            return command;
        }
    }
}
