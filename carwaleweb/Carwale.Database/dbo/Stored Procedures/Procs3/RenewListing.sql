IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RenewListing]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RenewListing]
GO

	-- =============================================
-- Author:		Amit verma
-- Create date: 27/11/2013
-- Edited By: Shikhar Maheshwari on July 16, 2014
-- Description:	Update the input InquiryId with renewal validty on July 24 2014
-- Edited the Flaws in the Date Renewable function -- Shikhar
-- Commented  code as it is not required by Aditi 8-8-2014
-- =============================================
CREATE PROCEDURE [dbo].[RenewListing]

	-- Add the parameters for the stored procedure here
	@InquiryId NUMERIC(18,0),
	@CustomersId NUMERIC(18,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @PackageID INT
	DECLARE @ClassifiedExpiryDate DATETIME, @PackageExpiryDate DATETIME
	--DECLARE @CustomerId INT

	SELECT 
		@PackageID = PackageId 
		,@ClassifiedExpiryDate = ClassifiedExpiryDate
		,@PackageExpiryDate = PackageExpiryDate
		--,@CustomerId = CustomerId --Commented this code as it is not required by Aditi 8-8-2014
	FROM 
		CustomerSellInquiries WITH(NOLOCK) WHERE ID = @InquiryId AND CustomerId = @CustomersId 

	DECLARE @RenewalValidity INT = (SELECT RenewValidity FROM Packages WITH(NOLOCK) WHERE ID = @PackageID)

	IF(DATEDIFF(DAY, GETDATE(), @ClassifiedExpiryDate) <= 7)
	BEGIN
		IF(@ClassifiedExpiryDate >= GETDATE())
		BEGIN
			SET @ClassifiedExpiryDate = CONVERT(DATETIME, CONVERT(DATE, DATEADD(day,@RenewalValidity,@ClassifiedExpiryDate) + 1))
			SET @ClassifiedExpiryDate = DATEADD(S,-1,@ClassifiedExpiryDate)
		END
		ELSE
		BEGIN
			SET @ClassifiedExpiryDate = CONVERT(DATETIME, CONVERT(DATE, DATEADD(day,@RenewalValidity,GETDATE()) + 1))
			SET @ClassifiedExpiryDate = DATEADD(S,-1,@ClassifiedExpiryDate)
		END

		IF(@ClassifiedExpiryDate > @PackageExpiryDate)
			SET @ClassifiedExpiryDate = CONVERT(DATETIME, CONVERT(DATE, @PackageExpiryDate))

		IF NOT EXISTS(SELECT * FROM CustomerSellInquiries WITH (NOLOCK) WHERE CustomerId=@CustomersId AND ClassifiedExpiryDate >= GETDATE() AND IsArchived=0 AND PackageId = @PackageID)
		BEGIN
			UPDATE CustomerSellInquiries 
				SET ClassifiedExpiryDate = @ClassifiedExpiryDate 
			WHERE 
				ID = @InquiryId 
			AND 
				CustomerId = @CustomersId	--Modified by Aditi 8-8-2014  Added the check of CustomerId.
		END
	END
END

