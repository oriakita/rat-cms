using System;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Shared.Base
{
    public interface IBaseState
    {
        void SubscribeToDefaultValuesReady(Action action);
    }

    public abstract class BaseState<T> : IBaseState where T : class
    {
        protected event EventHandler<EventArgs> DefaultValuesReady;

        public T Details { get; private set; } = default;
        public bool IsReady { get; set; } = false;
        public bool IsLoading { get; set; } = false;

        public void SubscribeToDefaultValuesReady(Action action)
        {
            // This flag is to prevent run action twice
            var haveNotExecutedYet = true;

            this.DefaultValuesReady += new EventHandler<EventArgs>((senser, eventAgrs) =>
            {
                haveNotExecutedYet = false;

                action();
            });

            if (IsReady && haveNotExecutedYet)
            {
                action();
            }
        }

        //public void SetState(dynamic details)
        //{
        //    this.Details = details;
        //    this.IsReady = true;

        //    if (this.DefaultValuesReady != null)
        //    {
        //        this.DefaultValuesReady(this, new EventArgs { });
        //    }
        //}

        public virtual async Task SetStateAsync(Func<Task<T>> getState)
        {
            this.IsLoading = true;

            this.Details = await getState();

            this.IsLoading = false;
            this.IsReady = true;

            if (this.DefaultValuesReady != null)
            {
                this.DefaultValuesReady(this, new EventArgs { });
            }
        }

    }
}
