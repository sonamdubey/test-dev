IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ViewDealerContent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ViewDealerContent]
GO

	-- =============================================
-- Author:		<Chetan Kane>
-- Modiefier: Two More content categories added "Services CaontentCategoryID=4" and "Career CaontentCategoryID=5"
-- Create date: <22/2/2012>
-- Description:	<Description,,>
-- Modified By: Tejashree Patil on 6 May 2013 at 3.30 pm, Added @ContentSubCatagoryId and fetched SubCatagoryDetails from Microsite_DealerContentSubCategories
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_ViewDealerContent]
	-- Add the parameters for the stored procedure here
	@DealerId int,
	@ContentCategoryId int,
	@ContentSubCategoryId SMALLINT=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	DealerContent,ContentTitle,ContentSubCatagoryId 
	FROM	Microsite_DealerContent 
	WHERE	DealerId=@DealerId AND ContentCatagoryId=@ContentCategoryId AND IsActive=1 
			AND ISNULL(ContentSubCatagoryId,0)=ISNULL(@ContentSubCategoryId,0)
	
	IF EXISTS(SELECT DealerId FROM TC_DealerMakes WHERE DealerId=@DealerId)
	BEGIN
		SELECT	SubCatagoryId,SubCatagoryName,UrlValue 
		FROM	Microsite_DealerContentSubCategories
		WHERE	IsActive=1 AND CategoryId=2 AND DealerId=@DealerId
	END
	
	
	
END
