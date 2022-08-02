namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PrizmaCheckResponse
    {
        private PrizmaCheckResult? _prizmaCheckResult;
        private PrizmaCheckError? _prizmaCheckError;

        public PrizmaCheckResult? PrizmaCheckResult 
        {
            get { return _prizmaCheckResult; }
            set 
            {
                if (_prizmaCheckError is null) 
                {
                    _prizmaCheckResult = value;
                }
            } 
        }
        public PrizmaCheckError? PrizmaCheckError 
        {
            get { return _prizmaCheckError; }
            set
            {
                if (_prizmaCheckResult is null)
                {
                    _prizmaCheckError = value;
                }
            }
        }

        public PrizmaCheckResponse(PrizmaCheckResult? prizmaCheckResult, PrizmaCheckError? prizmaCheckError)
        {
            PrizmaCheckResult = prizmaCheckResult;
            PrizmaCheckError = prizmaCheckError;
        }
    }
}
