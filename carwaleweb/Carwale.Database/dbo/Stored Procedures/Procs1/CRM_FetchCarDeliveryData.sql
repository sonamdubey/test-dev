IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchCarDeliveryData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchCarDeliveryData]
GO

	
CREATE PROCEDURE [dbo].[CRM_FetchCarDeliveryData]

	@CarBasicDataId			Numeric,
	@CarDeliveryId			Numeric OutPut,
	@LeadId					Numeric OutPut,
	@DeliveryStatusId		Int OutPut,
	@DeliveryStatus			VarChar(150) OutPut,
	@DealerId				Numeric OutPut,
	@DealerName				VarChar(200) OutPut,
	@ChasisNumber			VarChar(50) OutPut,
	@EngineNumber			VarChar(50) OutPut,
	@Color					VarChar(100) OutPut,
	@RegistrationNumber		VarChar(50) OutPut,
	@DeliveryComments		VarChar(1000) OutPut,
	@UpdatedById			Numeric OutPut,
	@UpdatedByName			VarChar(100) OutPut,
	@ContactPerson			VarChar(50) OUTPUT,
	@Contact				VarChar(50) OUTPUT,
	@InvoiceId				Numeric OutPut,
	@InvoiceNo				VarChar(50) OutPut,

	@ExpectedDeliveryDate	DateTime OutPut,
	@ActualDeliveryDate		DateTime OutPut,
	@CreatedOn				DateTime OutPut,
	@UpdatedOn				DateTime OutPut
				
 AS
BEGIN

	SELECT	
		@CarDeliveryId			= CDD.Id,
		@LeadId					= CBD.LeadId,
		@DeliveryStatusId		= CDD.DeliveryStatusId,
		@DeliveryStatus			= CIA.Name,
		@DealerId				= CDD.DealerId,
		@DealerName				= ND.Name,
		@ChasisNumber			= CDD.ChasisNumber,
		@EngineNumber			= CDD.EngineNumber,
		@Color					= CDD.Color,
		@RegistrationNumber		= CDD.RegistrationNumber,
		@DeliveryComments		= CDD.DeliveryComments,
		@UpdatedById			= CDD.UpdatedBy,
		@UpdatedByName			= OU.UserName,
		@ContactPerson			= CDD.ContactPerson,	
		@Contact				= CDD.Contact,
		@InvoiceId				= CCI.InvoiceId,
		@InvoiceNo				= CAI.InvoiceNo,

		@ExpectedDeliveryDate	= CDD.ExpectedDeliveryDate,
		@ActualDeliveryDate		= CDD.ActualDeliveryDate,
		@CreatedOn				= CDD.CreatedOn,
		@UpdatedOn				= CDD.UpdatedOn

	FROM ((((((CRM_CarDeliveryData AS CDD LEFT JOIN CRM_CarBasicData AS CBD ON CDD.CarBasicDataId = CBD.Id)
			LEFT JOIN OprUsers AS OU ON CDD.UpdatedBy = OU.Id)
			LEFT JOIN NCS_Dealers AS ND ON CDD.DealerId = ND.Id)
			LEFT JOIN CRM_EventTypes AS CIA ON CDD.DeliveryStatusId = CIA.Id)
			LEFT JOIN CRM_CarInvoices AS CCI ON CDD.CarBasicDataId = CCI.CBDId)
			LEFT JOIN CRM_ADM_Invoices AS CAI ON CCI.InvoiceId = CAI.Id)
			

	WHERE CDD.CarBasicDataId = @CarBasicDataId
END
