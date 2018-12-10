IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MM_SaveContactDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MM_SaveContactDetails]
GO

	
-- =============================================
-- Author:		Nilima More
-- Create date: 10 Oct 2016
-- Description:	TO save requested masking number details
-- Modified by: Kritika Choudhary on 12th oct 2016, added update part
-- exec [TC_MM_SaveContactDetails] null,null,null,null,null,2,0
-- =============================================
CREATE PROCEDURE [dbo].[TC_MM_SaveContactDetails]
@DealerId INT=NULL,
@Quantity INT=NULL,
@Email VARCHAR(100)=NULL,
@PhoneNumber VARCHAR(10) = NULL,
@CreatedBy INT=NULL,
@MMContactID INT =NULL OUTPUT,
@IsMailSent BIT = NULL
AS
BEGIN
IF(@MMContactID IS NULL)
	BEGIN
		-- Insert statements for procedure here
		INSERT INTO TC_MM_ContactDetails(DealerId,Quantity,Email,PhoneNumber ,CreatedOn,CreatedBy)
		VALUES(@DealerId,@Quantity,@Email,@PhoneNumber ,GETDATE(),@CreatedBy)

		SET @MMContactID = SCOPE_IDENTITY()
	END
ELSE
	BEGIN
		UPDATE TC_MM_ContactDetails
        SET IsMailSent = @IsMailSent
        WHERE TC_MM_ContactDetailsId = @MMContactID
	END
END

