IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TDCheckCustomerStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TDCheckCustomerStatus]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,27th Feb, 2013>
-- Description:	<Description,This Sp will check if the customer/Lead for that customer is already existing before adding test drive>
-- =============================================
CREATE PROCEDURE [dbo].[TC_TDCheckCustomerStatus]
	-- Add the parameters for the stored procedure here
	@BranchId BIGINT,
    @CustMobile VARCHAR(20),
    @TC_TDCarsId SMALLINT,
    @CustName VARCHAR(200),
    @CustEmail VARCHAR(200),
    @TC_NewCarInqId BIGINT OUTPUT,
    @TC_TDCalId BIGINT OUTPUT,
    @InquirySourceId SMALLINT OUTPUT,
    @CustomerId BIGINT OUTPUT
AS
BEGIN
	DECLARE @ActiveLeadId BIGINT
	DECLARE @IsFake TINYINT
	DECLARE @ModelId INT
	
	SELECT @ActiveLeadId=ActiveLeadId,@IsFake=ISNULL(Isfake,0),@CustomerId = Id
		FROM TC_CustomerDetails WITH(NOLOCK)
		Where  Mobile= @CustMobile AND BranchId=@BranchId AND IsActive=1 --AND IsleadActive=1
	IF(@CustomerId IS NOT NULL)
	BEGIN	
		IF(@IsFake=1) --Customer is fake no need to process further
		BEGIN
			RETURN 99
		END
		
		IF(@ActiveLeadId IS NULL) -- No Active Lead found
		BEGIN
			RETURN 2
		END
		UPDATE TC_CustomerDetails SET CustomerName = @CustName, Email = @CustEmail WHERE Id=@CustomerId
		-- Getting ModelId of the testDrive car in order to find ouut new car inquiries for the selected model only
		SELECT @ModelId = V.ModelId FROM TC_TDCars TDC INNER JOIN vwMMV V ON V.VersionId = TDC.VersionId WHERE TDC.TC_TDCarsId = @TC_TDCarsId 
		
		DECLARE @CalStatus SMALLINT
		SELECT TOP 1 @TC_NewCarInqId=N.TC_NewCarInquiriesId, 
		@TC_TDCalId=N.TC_TDCalendarId,
		@CalStatus=N.TDStatus, 
		@InquirySourceId = N.TC_InquirySourceId,
		@CustomerId = L.TC_CustomerId
		FROM TC_NewCarInquiries N 
								INNER JOIN TC_InquiriesLead L ON N.TC_InquiriesLeadId=L.TC_InquiriesLeadId 
																AND L.TC_LeadInquiryTypeId=3 AND L.TC_LeadId=@ActiveLeadId
																AND L.TC_LeadDispositionID IS NULL
																AND N.TC_LeadDispositionId IS NULL
		WHERE N.VersionId IN(SELECT v.ID FROM CarVersions v WHERE v.CarModelId=@ModelId)
		ORDER BY N.CreatedOn DESC
		
		
		IF(@CalStatus IS NULL OR @CalStatus IN(26,27,28))
		BEGIN
			SET @TC_TDCalId=NULL
		END
		RETURN 3
	END
	RETURN 4
END
