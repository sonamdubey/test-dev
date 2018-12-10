IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SEO_ContentSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SEO_ContentSave]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 15th oct,2013
-- Description:	Save SEO Content for dealer website
-- =============================================
CREATE PROCEDURE [dbo].[SEO_ContentSave]
	@DealerId INT,
	@PageId INT,
	@PageName VARCHAR(100),
	@Title VARCHAR(500),
	@MetaKeywords VARCHAR(1000),
	@MetaDescription VARCHAR(MAX),
	@IsActive BIT
AS
BEGIN	
	SET NOCOUNT ON;
    
	IF NOT EXISTS (SELECT PageId FROM DealerWebsite_SEOContent WITH(NOLOCK) WHERE PageId = @PageId AND DealerId = @DealerId)
	BEGIN
		INSERT INTO DealerWebsite_SEOContent 
						  (PageId,PageName,DealerId,Title,MetaKeywords,MetaDescription,DateCreated,IsActive)
			   VALUES  
						  (@PageId,@PageName,@DealerId,@Title,@MetaKeywords,@MetaDescription,GETDATE(),@IsActive)
	END
	ELSE
	BEGIN
	    UPDATE DealerWebsite_SEOContent
		SET PageName = @PageName,Title=@Title,MetaKeywords=@MetaKeywords,MetaDescription=@MetaDescription,DateUpdated=GETDATE(),IsActive = @IsActive
		WHERE PageId=@PageId AND DealerId = @DealerId
	END
END

