IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchCarCustomerServiceRequestsData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchCarCustomerServiceRequestsData]
GO

	CREATE PROCEDURE [dbo].[CRM_FetchCarCustomerServiceRequestsData]

	@CarBasicDataId			Numeric,
	@CSRId					Numeric OutPut,
	@TDLocation				VarChar(500) OutPut,

	@TDRequest				Bit OutPut,
	@BookingRequest			Bit OutPut,
	@InterestedInFinance	Bit OutPut,

	@TDDate					DateTime OutPut
				
 AS
	
BEGIN

	SELECT	
		@CSRId				 = CSR.Id,
		@TDLocation			 = CSR.TDLocation,

		@TDRequest			 = CSR.TDRequest,
		@BookingRequest		 = CSR.BookingRequest,
		@InterestedInFinance = CSR.InterestedInFinance,

		@TDDate				 = CSR.TDDate

	FROM CRM_CarCustomerServiceRequests AS CSR

	WHERE CSR.CarBasicDataId = @CarBasicDataId
END





