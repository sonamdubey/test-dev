IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireInsuranceQuotationInfo_SP ]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireInsuranceQuotationInfo_SP ]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 6/4/2012
-- Description:	Procedure will insert berkshire insurance quotation parameters into BerkshireInsuranceLeads table.
-- =============================================
CREATE PROCEDURE [dbo].[BerkshireInsuranceQuotationInfo_SP ]
	-- Add the parameters for the stored procedure here
	@CarwaleCustomerId			BIGINT,	
	@Name						VARCHAR(100),
    @Email						VARCHAR(50),
    @Mobile						VARCHAR(12),
	@BerkshireCityId			SMALLINT,
	@BerkshireMakeId			SMALLINT,
	@BerkshireModelId			SMALLINT,
	@BerkshireVersionId			SMALLINT,
	@CarMakeYear				SMALLINT,
	@CarRegistrationDate		DATE,
	@PolicyType					VARCHAR(20),
	@CurrentPolicyExpiryDate	DATE,	
	@LeadId						BIGINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Insert values into BerkshireInsuranceLeads table
	
	BEGIN TRY
		INSERT INTO dbo.BerkshireInsuranceLeads
		 (
		            CarwaleCustomerId,
		            CustomerName,
					CustomerEmail,
					CustomerMobile,
					BerkshireCityId,
					BerkshireMakeId,
					BerkshireModelId,
					BerkshireVersionId,
					CarMakeYear,
					CarRegistrationDate,
					PolicyType,
					CurrentPolicyExpiryDate,
					IsPushedToBerkshire,
					JsonRequestString,
					EntryDate,
					ReturnedBerkshireLeadId,
					PushStatusMessage
					
		)
		VALUES (@CarwaleCustomerId, @Name,@Email,@Mobile,@BerkshireCityId, @BerkshireMakeId, @BerkshireModelId,
				@BerkshireVersionId, @CarMakeYear, @CarRegistrationDate, @PolicyType,
				@CurrentPolicyExpiryDate, 0, '', GETDATE(), NULL, NULL);
				
		SET @LeadId = SCOPE_IDENTITY();
	END TRY
	BEGIN CATCH
		SET @LeadId = -1
	END CATCH
END




