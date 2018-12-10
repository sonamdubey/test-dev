IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_DealerPanel_CarAssignmentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_DealerPanel_CarAssignmentDetails]
GO

	--Name of SP/Function                    : CRM_DealerPanel_CarAssignmentDetails
--Applications using SP                  : Dealer Panel
--Modules using the SP                   : DashBoard.cs
--Technical department                   : Database
--Summary                                : Returns details of question asked from customer dduring dealer assignment and status of customer
--Author                                 : AMIT Kumar 6th dec 2013
--Modification history                   : 1 
CREATE PROCEDURE [dbo].[CRM_DealerPanel_CarAssignmentDetails]
@LeadId		NUMERIC(18,0)
AS
BEGIN		
	SELECT CVOL.PurchaseTime,
		CASE WHEN CVOL.Eagerness= 1 THEN '' WHEN CVOL.Eagerness= -1 THEN 'None' WHEN CVOL.Eagerness= 2 THEN 'High' WHEN CVOL.Eagerness= 3 THEN 'Normal' WHEN CVOL.Eagerness= 4 THEN 'Low' END AS Eagerness,
		CASE WHEN CVOL.PurchaseMode=1 THEN 'Finance' WHEN CVOL.PurchaseMode=2 THEN 'Outright' END AS PurchaseMode,
		CVOL.PurchaseOnName,CVOL.PurchaseOnNameType,CVOL.CurrentCarOwned,CVOL.BuyingSpan,CVOl.Usage,CVOL.UsageType,
		CVOL.CarOwnership,CVOL.CompanyName,CVOL.PurchaseContact,CVOL.Occasion,CVOL.MonthlyUsageCity
	FROM  CRM_VerificationOthersLog CVOL WITH(NOLOCK) 
	WHERE  CVOL.LeadId = @LeadId
END
