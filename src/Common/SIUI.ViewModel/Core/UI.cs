﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace SIUI.ViewModel.Core
{
    public static class UI
    {
        public static TaskScheduler Scheduler { get; private set; } // TODO: -> private

        public static void Initialize()
        {
            Scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public static void Execute(Action action, Action<Exception> onError)
        {
            void wrappedAction()
            {
                try
                {
                    action();
                }
                catch (Exception exc)
                {
                    onError(exc);
                }
            }

            if (TaskScheduler.Current != Scheduler && Scheduler != null)
            {
                Task.Factory.StartNew(wrappedAction, CancellationToken.None, TaskCreationOptions.None, Scheduler);
                return;
            }

            wrappedAction();
        }
    }
}
