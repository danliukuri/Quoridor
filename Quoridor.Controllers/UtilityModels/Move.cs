using Quoridor.Models;
using Quoridor.Models.General;
using Quoridor.Models.General.Either;

namespace Quoridor.Controllers.UtilityModels
{
    public class Move
    {
        readonly EitherLeftOrRight<EitherLeftOrRight<Direction, (Direction, FieldNode)>,
            EitherLeftOrRight<Direction, (Direction, Direction)>> moveParameters;

        public Move(Direction walkDirection)
        {
            moveParameters = moveParameters = new EitherLeftOrRight<EitherLeftOrRight<Direction, (Direction, FieldNode)>,
                EitherLeftOrRight<Direction, (Direction, Direction)>>(
                new EitherLeftOrRight<Direction, (Direction, FieldNode)>(walkDirection));
        }
        public Move(EitherLeftOrRight<Direction, (Direction directionFromThisPlayer,
            Direction directionFromAnotherPlayer)> jump)
        {
            moveParameters = new EitherLeftOrRight<EitherLeftOrRight<Direction, (Direction, FieldNode)>,
                EitherLeftOrRight<Direction, (Direction, Direction)>>(jump);
        }
        public Move(Direction wallPlaceDirection, FieldNode wallPlaceField)
        {
            moveParameters = new EitherLeftOrRight<EitherLeftOrRight<Direction, (Direction, FieldNode)>,
                EitherLeftOrRight<Direction, (Direction, Direction)>>((wallPlaceDirection, wallPlaceField));
        }

        public bool IsWalk => moveParameters.IsLeft && moveParameters.LeftOrDefault().IsLeft;
        public bool IsJump => !moveParameters.IsLeft;
        public bool IsWallPlace => moveParameters.IsLeft && !moveParameters.LeftOrDefault().IsLeft;

        public Direction GetWalkParameters() => moveParameters.LeftOrDefault().LeftOrDefault();
        public EitherLeftOrRight<Direction, (Direction directionFromThisPlayer,
            Direction directionFromAnotherPlayer)> GetJumpParameters() => moveParameters.RightOrDefault();
        public (Direction, FieldNode) GetWallPlaceParameters() => moveParameters.LeftOrDefault().RightOrDefault();
    }
}