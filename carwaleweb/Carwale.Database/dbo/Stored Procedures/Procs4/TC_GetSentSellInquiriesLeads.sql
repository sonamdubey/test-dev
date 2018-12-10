IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetSentSellInquiriesLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetSentSellInquiriesLeads]
GO

	
-- =============================================
-- Author:		Binumon George
-- Create date: 29th November 2011
-- Description:	For Listing entire sent sell inquiry in excel sheet
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetSentSellInquiriesLeads]
@BranchId INT,
@FDate VARCHAR(25)=NULL,
@EDate VARCHAR(25)=NULL
AS
BEGIN
	Select 'S' + Convert(Varchar,CSI.Id) AS ProfileId, (CMA.Name +' '+ CMO.Name +' '+ CV.Name) AS Car,
	CU.Name AS Customer, CU.Mobile AS Mobile, CU.EMail, DPI.SendDate , CSV.UsedCarValue , 
	CSI.Price , CSI.MakeYear , CSI.Kilometers , CSD.Owners , CSD.RegistrationPlace  
	From AP_DealerPackageInquiries  AS DPI LEFT JOIN  CustomerSellInquiries AS CSI
	 on DPI.SellInquiryId = CSI.Id   LEFT JOIN CarVersions AS CV on CSI.CarVersionId = CV.Id  
	LEFT JOIN CarModels AS CMO on CV.CarModelId = CMO.Id LEFT JOIN CarMakes AS CMA 
	on CMO.CarMakeId = CMA.Id   LEFT JOIN Customers AS CU on CSI.CustomerId = CU.Id
	LEFT JOIN CustomerSellInquiriesValuation AS CSV  on CSV.SellInquiryId = CSI.ID 
	LEFT JOIN CustomerSellInquiryDetails AS CSD on CSD.InquiryId = CSI.ID   Where
	DPI.DealerId = @BranchId AND DPI.SendDate 
	BETWEEN @FDate AND @EDate  Order By SendDate DESC
END
