IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BD_InsertIntoOtherDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BD_InsertIntoOtherDetails]
GO

	
CREATE PROCEDURE [dbo].[BD_InsertIntoOtherDetails]
	@InquiryId				AS Numeric,
	@InterestedInFinance	AS BIT,
	@EmploymentType			AS Varchar(50),
	@Address				AS Varchar(250),
	@Pincode				AS Varchar(50),
	@AltContactNo			AS Varchar(50),
	@Testdrive				AS Varchar(50)

AS
	
BEGIN
	INSERT INTO BD_OtherDetails
	(
		NCPInquiryId,	InterestedInFinance,	EmploymentType,		Address,			
		Pincode,		AltContactNo,			Testdrive
	)
	Values
	(
		@InquiryId,		@InterestedInFinance,	@EmploymentType,	@Address,			
		@Pincode,		@AltContactNo,			@Testdrive
	)
	
END
