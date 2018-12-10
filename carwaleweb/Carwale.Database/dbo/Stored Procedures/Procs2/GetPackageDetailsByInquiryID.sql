IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPackageDetailsByInquiryID]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPackageDetailsByInquiryID]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 06/05/2013
-- Description:	get amount for a package
--modification: check InqPtCategoryId = 30 (also for individual) by amit 10/31/2013
/*
	Declare @InquiryID int = 2137
	declare @Amount int
	declare @PackageID int
	declare @DefaultPackageId int = 44
	exec GetPackageDetailsByInquiryID @InquiryID,@PackageID out,@Amount out,@DefaultPackageId
	select @Amount,@PackageID
*/
-- =============================================
CREATE PROCEDURE [dbo].[GetPackageDetailsByInquiryID] 

	-- Add the parameters for the stored procedure here
	@InquiryID int,
	@PackageID int out,
	@Amount int out,
	@DefaultPackageId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    --Insert statements for procedure here
	UPDATE CustomerSellInquiries
	SET PackageId = @DefaultPackageId WHERE ID = @InquiryID AND PackageId IS NULL
	
	SELECT @Amount = P.Amount,@PackageID = P.Id
	FROM Packages P WITH (NOLOCK)
	RIGHT JOIN CustomerSellInquiries CSI WITH (NOLOCK) ON P.Id = CSI.PackageId
	WHERE CSI.ID = @InquiryID
	    AND P.InqPtCategoryId in (31,30)  --check InqPtCategoryId = 30 (also for individual) by amit 10/31/2013
		AND P.isActive = 1
		AND P.ForIndividual = 1
		AND P.IsInternal = 0

END

