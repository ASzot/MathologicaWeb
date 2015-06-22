using MathSolverWebsite.MathSolverLibrary.Equation;
using System.Linq;

namespace MathSolverWebsite.MathSolverLibrary.TermType
{
    internal abstract class TermType
    {
        protected string[] _cmds = null;
        protected MultiLineHelper _multiLineHelper = null;

        public int CmdCount
        {
            get { return _cmds.Length; }
        }

        public TermType(params string[] cmds)
        {
            _cmds = cmds;
        }

        public TermType()
        {
        }

        public virtual void AttachMultiLineHelper(MultiLineHelper mlh)
        {
            _multiLineHelper = mlh;
        }

        public virtual SolveResult ExecuteCommand(string command, ref EvalData pEvalData)
        {
            if (_multiLineHelper != null)
                _multiLineHelper.DoAssigns(ref pEvalData);

            return SolveResult.Failure();
        }

        public virtual SolveResult ExecuteCommandIndex(int cmdIndex, ref EvalData pEvalData)
        {
            if (_cmds == null || cmdIndex < 0 || cmdIndex >= _cmds.Length)
                return SolveResult.Failure();

            return ExecuteCommand(_cmds[cmdIndex], ref pEvalData);
        }

        public virtual string[] GetCommands()
        {
            return _cmds;
        }

        public virtual bool IsValidCommand(string cmd)
        {
            if (_cmds.Contains(cmd))
                return true;
            return false;
        }
    }
}