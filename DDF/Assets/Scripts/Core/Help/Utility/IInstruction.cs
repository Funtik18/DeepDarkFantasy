using System;

namespace DDF.Help {
    public interface IInstruction {
        bool isExecuting { get; }
        bool isPaused { get; }
        bool isStoped { get; }

        Instruction Execute();
        void OnStarted();
        void OnPaused();
        void OnResumed();
        void OnTerminated();
        void OnDone();
        
        event Action<Instruction> onStarted;
        event Action<Instruction> onPaused;
        event Action<Instruction> onTerminated;
        event Action<Instruction> onDone;
    }
}