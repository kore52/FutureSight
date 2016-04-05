namespace FutureSight.lib
{
    public class UntapAction : MTGAction
    {
        private MTGPermanent permanent;
        private bool isTapped;

        public UntapAction(MTGPermanent permanent)
        {
            this.permanent = permanent;
        }

        // アクションを行う
        public override void DoAction(GameState game)
        {
            isTapped = permanent.IsTapped();
            if (isTapped)
            {
                permanent.RemoveState(MTGPermanentState.Tapped);
                game.SetStateCheckRequired();
            }
        }

        // アクションを戻す
        public override void UndoAction(GameState game)
        {
            if (isTapped)
                permanent.AddState(MTGPermanentState.Tapped);
        }
    }
}