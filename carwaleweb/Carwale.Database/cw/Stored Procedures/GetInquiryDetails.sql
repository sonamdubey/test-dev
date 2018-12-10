IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetInquiryDetails]
GO

	
-- =============================================
-- Author:		Vikas
-- Create date: 24/01/2013
-- Description:	To get the primary information about the listing.
-- =============================================
CREATE PROCEDURE [cw].[GetInquiryDetails] 
	-- Add the parameters for the stored procedure here
	@InquiryId NUMERIC
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		(CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS CarName, 
		CSI.MakeYear, 
		CI.Name AS City, 
		CSI.Price
	FROM 
		CustomerSellInquiries CSI
		INNER JOIN CarVersions CV ON CV.ID = CSI.CarVersionId
		INNER JOIN CarModels CMO ON CMO.ID = CV.CarModelId
		INNER JOIN CarMakes CMA ON CMA.ID = CMO.CarMakeId
		INNER JOIN Cities CI ON CI.ID = CSI.CityId
	WHERE 
		CSI.ID = @InquiryId	 
END

