using System;

namespace ItHappend
{
    public class EventEvaluation
    {
        public int Evaluation { get; private set; }

        public EventEvaluation(int evaluation)
        {
            Evaluation = evaluation;
        }

        public void ReEvaluate(int newEvaluation)
        {
            if (ValidateEvaluation(newEvaluation))
            {
                Evaluation = newEvaluation;    
            }
        }

        private static bool ValidateEvaluation(int newEvaluation)
        {
            throw new NotImplementedException();
        }
    }
}