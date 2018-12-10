IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IsCustomerAuthorizedToManageCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IsCustomerAuthorizedToManageCar]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 16 Nov 2013
-- Description:	Proc to check whether customer is allowed to manage the listing.
--				The customer can manage only cars listed by him/her.
-- =============================================
CREATE PROCEDURE IsCustomerAuthorizedToManageCar

	-- Add the parameters for the stored procedure here
	@CustomerId BIGINT,
	@InquiryId BIGINT,
	@IsAuthorized BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here	
	DECLARE @IdCount TINYINT = 0

	SET @IsAuthorized = 0

	SELECT @IdCount = COUNT(Id)
	FROM CustomerSellInquiries
	WHERE ID = @InquiryId AND CustomerId = @CustomerId

	IF(@IdCount = 1)
	BEGIN
		SET @IsAuthorized = 1		
	END	
END

