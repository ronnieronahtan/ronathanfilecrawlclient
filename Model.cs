using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ronathanFileCrawler
{
    public class ImplemenationValidationRequest
    {
        public string valiationId { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public fileForValidation[] evidence { get; set; }
        public string[] controls { get; set; }
    }
    public class fileForValidation
    {
        public string fileName { get; set; }
        public string file { get; set; }

    }

    public class implementationValidationResponseHelper
    {
        public string valiationId { get; set; }
        public string[] controls { get; set; }
        public string[] evidenceCount { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string validationPrefix { get; set; }
    }
    public class ImplemenationValidationResponse
    {
        public string valiationId { get; set; }
        public CtrlmplementationValidationResponse[] ctrlImplementationValidations { get; set; }

    }
    public class CtrlmplementationValidationResponse
    {
        public string controlId { get; set; }
        public string classification { get; set; }
        public float fidelity { get; set; }
        public AssObjImpllemationValidationResponse[] assesmentobjectives { get; set; }
        //function aggregates control with a like contolId
        public void Aggreagate(CtrlmplementationValidationResponse toAdd)
        {

            //determine if the current control or the contol to add has a higher fidelity and use that one
            if (toAdd.fidelity > this.fidelity)
            {
                this.fidelity = toAdd.fidelity;
            }
            //for each assessment objective in the control determine if the assesment objective exists in the control to add it so add it if it does not exist
            foreach (AssObjImpllemationValidationResponse ao in toAdd.assesmentobjectives)
            {
                bool found = false;
                foreach (AssObjImpllemationValidationResponse ao2 in this.assesmentobjectives)
                {
                    if (ao2.objectiveId == ao.objectiveId)
                    {
                        found = true;
                        //determine if the current control or the contol to add has a higher fidelity and use that one
                        if (ao2.fidelity < ao.fidelity)
                        {
                            ao2.fidelity = ao.fidelity;
                        }
                    }
                }
                if (!found)
                {
                    this.assesmentobjectives.Append(ao);
                }

            }


        }

    }
    public class AssObjImpllemationValidationResponse
    {
        public string objectiveId { get; set; }
        public string classification { get; set; }
        public float fidelity { get; set; }
    }
    public class InsufficientTokensMessage
    {
        public string Message { get; set; }
        public int AvalibleTokens { get; set; }
        public int NumberTokensNeeded { get; set; }
    }

}
